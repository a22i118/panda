using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace server
{
    internal class HTTPReceiver
    {
        TcpClient _client;

        private IPAddress _address;
        private int _port;

        private bool _isDisconnect = false;

        public bool IsDisconnect { get { return _isDisconnect; } }

        public HTTPReceiver(TcpClient c)
        {
            _client = c;

            _address = ((IPEndPoint)_client.Client.LocalEndPoint).Address;
            _port = ((IPEndPoint)_client.Client.LocalEndPoint).Port;

            Debug.WriteLine("接続しました。({0}:{1})", _address, _port);

            StartReceive(_client.Client);
        }

        ~HTTPReceiver() { }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        // 非同期データ受信のための状態オブジェクト
        private class AsyncStateObject
        {
            public System.Net.Sockets.Socket Socket;
            public byte[] ReceiveBuffer;
            public System.IO.MemoryStream ReceivedData;

            public AsyncStateObject(System.Net.Sockets.Socket soc)
            {
                this.Socket = soc;
                this.ReceiveBuffer = new byte[1024];
                this.ReceivedData = new System.IO.MemoryStream();
            }
        }

        // データ受信スタート
        private void StartReceive(System.Net.Sockets.Socket soc)
        {
            AsyncStateObject so = new AsyncStateObject(soc);
            // 非同期受信を開始
            soc.BeginReceive(so.ReceiveBuffer,
                0,
                so.ReceiveBuffer.Length,
                System.Net.Sockets.SocketFlags.None,
                new System.AsyncCallback(ReceiveDataCallback),
                so);
        }

        // BeginReceiveのコールバック
        private void ReceiveDataCallback(System.IAsyncResult ar)
        {
            // 状態オブジェクトの取得
            AsyncStateObject so = (AsyncStateObject)ar.AsyncState;

            if (so == null)
            {
                return;
            }

            // 読み込んだ長さを取得
            int len = 0;
            try
            {
                len = so.Socket.EndReceive(ar);
            }
            catch (System.ObjectDisposedException)
            {
                // 閉じた時
                Debug.WriteLine("閉じました。({0}:{1})", _address, _port);
                so.Socket.Close();
                _isDisconnect = true;
                return;
            }
            catch (SocketException ex)
            {
                // 閉じた時
                Debug.WriteLine("閉じられました。({0}:{1})", _address, _port);
                Debug.WriteLine("Error accepting TCP connection: {0}", ex.Message);
                so.Socket.Close();
                _isDisconnect = true;
                return;
            }

            // 切断されたか調べる
            if (len <= 0)
            {
                Debug.WriteLine("切断されました。({0}:{1})", _address, _port);
                so.Socket.Close();
                _isDisconnect = true;
                return;
            }

            // 受信したデータを蓄積する
            so.ReceivedData.Write(so.ReceiveBuffer, 0, len);
            if (so.Socket.Available == 0)
            {
                // 最後まで受信した時
                // 受信したデータを文字列に変換
                // string str = System.Text.Encoding.UTF8.GetString(so.ReceivedData.ToArray());
                byte[] bytes = so.ReceivedData.ToArray();
                string header = System.Text.Encoding.UTF8.GetString(bytes);

                if (Regex.IsMatch(header, "^GET", RegexOptions.IgnoreCase))
                {
                    Debug.WriteLine("=====Handshaking from client=====\n{0}", header);

                    // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                    // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                    // 3. Compute SHA-1 and Base64 hash of the new value
                    // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                    string swk = Regex.Match(header, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                    string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                    byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                    string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                    // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                    byte[] response = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 101 Switching Protocols\r\n" +
                        "Connection: Upgrade\r\n" +
                        "Upgrade: websocket\r\n" +
                        "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                    //stream.Write(response, 0, response.Length);
                    so.Socket.Send(response);
                }
                else
                {
                    bool fin = (bytes[0] & 0b10000000) != 0,
                        mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the _client to the server have this bit set"

                    int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
                        msglen = bytes[1] - 128, // & 0111 1111
                        offset = 2;

                    if (msglen == 126)
                    {
                        // was ToUInt16(bytes, offset) but the result is incorrect
                        msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
                        offset = 4;
                    }
                    else if (msglen == 127)
                    {
                        Debug.WriteLine("TODO: msglen == 127, needs qword to store msglen");
                        // i don't really know the byte order, please edit this
                        // msglen = BitConverter.ToUInt64(new byte[] { bytes[5], bytes[4], bytes[3], bytes[2], bytes[9], bytes[8], bytes[7], bytes[6] }, 0);
                        // offset = 10;
                    }

                    if (msglen == 0)
                    {
                        Debug.WriteLine("msglen == 0");
                    }
                    else if (msglen < 0)
                    {
                        Debug.WriteLine("msglen < 0");
                    }
                    else if (mask)
                    {
                        byte[] decoded = new byte[msglen];
                        byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
                        offset += 4;

                        for (int i = 0; i < msglen; ++i)
                            decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]);

                        string text = Encoding.UTF8.GetString(decoded);

                        if (text != "\u0003�")  // end of text	テキスト終了
                        {
                            Debug.WriteLine(text);

                            byte[] second = Encoding.UTF8.GetBytes(text);
                            byte[] first = new byte[] { bytes[0], (byte)second.Length };
                            byte[] buffer = new byte[first.Length + second.Length];
                            Buffer.BlockCopy(first, 0, buffer, 0, first.Length);
                            Buffer.BlockCopy(second, 0, buffer, first.Length, second.Length);
                            so.Socket.Send(buffer, buffer.Length, SocketFlags.None);
                        }
                    }
                    else
                        Debug.WriteLine("mask bit not set");

                    //Debug.WriteLine();
                }

                so.ReceivedData.Close();
                so.ReceivedData = new System.IO.MemoryStream();
            }

            // 再び受信開始
            so.Socket.BeginReceive(so.ReceiveBuffer,
                0,
                so.ReceiveBuffer.Length,
                System.Net.Sockets.SocketFlags.None,
                new System.AsyncCallback(ReceiveDataCallback),
                so);
        }
    }
}
