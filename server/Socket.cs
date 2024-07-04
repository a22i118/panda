using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    internal class Socket
    {
        private static readonly object _receiversLock = new object();
        private static List<HTTPReceiver> _receivers = new List<HTTPReceiver>();
        public Socket()
        {
            //IPv4とIPv6の全てのIPアドレスをListenする
            TcpListener listener = new TcpListener(System.Net.IPAddress.IPv6Any, 443);

            //IPv6Onlyを0にする
            listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, 0);
            listener.Start();

            StartAccept(listener);

            Task update = Task.Run(() => { Update(); });
            //Task.WaitAll(new Task[] { update });
        }

        public static void Update()
        {
            while (true)
            {
                Thread.Sleep(1000);

                lock (_receiversLock)
                {
                    List<HTTPReceiver> listRemove = new List<HTTPReceiver>();

                    foreach (var receiver in _receivers)
                    {
                        if (receiver.IsDisconnect)
                        {
                            listRemove.Add(receiver);
                        }
                    }

                    foreach (var receiver in listRemove)
                    {
                        receiver.Dispose();
                        _receivers.Remove(receiver);
                    }
                }
            }
        }
        // クライアントの接続待ちスタート
        private static void StartAccept(TcpListener listener)
        {
            //接続要求待機を開始する
            listener.BeginAcceptTcpClient(new System.AsyncCallback(AcceptCallback), listener);
        }

        private static void DoErrorHandling(TcpListener listener)
        {
            listener.Stop();
            listener.Start();

            StartAccept(listener);
        }

        // BeginAcceptのコールバック
        private static void AcceptCallback(System.IAsyncResult ar)
        {
            // サーバーSocketの取得
            TcpListener listener = (TcpListener)ar.AsyncState;

            if (listener == null)
            {
                return;
            }

            // 接続要求を受け入れる
            TcpClient client;
            try
            {
                // クライアントSocketの取得
                client = listener.EndAcceptTcpClient(ar);

                lock (_receiversLock)
                {
                    _receivers.Add(new HTTPReceiver(client));
                }

                Debug.WriteLine("Listen connected. {0}", _receivers.Count);
            }
            catch (SocketException ex)
            {
                // the _client is corrupt
                Debug.WriteLine("Error accepting TCP connection: {0}", ex.Message);

                DoErrorHandling(listener);

                return;
            }
            catch (ObjectDisposedException)
            {
                // The listener was Stop()'d, disposing the underlying socket and
                // triggering the completion of the callback. We're already exiting,
                // so just return.
                Debug.WriteLine("Listen canceled.");

                DoErrorHandling(listener);

                return;
            }

            // 接続要求待機を再開する
            listener.BeginAcceptTcpClient(new System.AsyncCallback(AcceptCallback), listener);
        }
    }
}
