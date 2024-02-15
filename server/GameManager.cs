using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class GameManager
    {
        const int players = 4;
        int turn_ = 0;
        Yama yama = new Yama();
        Tehai[] tehais = new Tehai[players];
        Kawa[] kawas = new Kawa[players];
        //Naki[] nakis = new Naki[players];
        WanPai wanPai = new WanPai();

        Hai sutehai = null;
        //List<Hai> list_ = new List<Hai>();
        ActionCommand[] _actionCommand = new ActionCommand[players];
        public bool Atari = false;
        public bool Ron = false;

        //public List<Hai> List { get { return list_; } }
        public enum eMode
        {
            Tsumo,
            Wait,
        }
        eMode mode_;

        public GameManager()
        {
            Init();

        }

        public void Init()
        {
            for (int i = 0; i < players; i++)
            {
                tehais[i] = new Tehai();
                kawas[i] = new Kawa();
                _actionCommand[i] = new ActionCommand(300, i * 200 + 74, 64, 32);
            }

            //王牌
            for (int i = 0; i < 14; i++)
            {
                wanPai.Add(yama.List[0]);
                yama.List.RemoveAt(0);
            }

            //四人に配る
            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
            }

            //手牌のソート
            for (int i = 0; i < players; i++)
            {
                tehais[i].Sort();
            }
        }
        public void Exec()
        {
            if (mode_ == eMode.Tsumo)
            {
                // コマンド入力を待っている間はツモらない
                foreach (var cmd in _actionCommand)
                {
                    if (cmd.IsCanAny())
                    {
                        return;
                    }
                }

                tehais[turn_].Tsumo(yama);
                AtariList atariList = new AtariList(tehais[turn_]);

                if (atariList.IsAtari())
                {
                    Atari = true;
                    //Console.WriteLine("アタリ");
                }

                mode_ = eMode.Wait;
            }
            else
            {
                if (tehais[turn_].List.Count < 14)
                {
                    turn_ = (turn_ + 1) % 4;
                    mode_ = eMode.Tsumo;
                }
                //tehais[turn_].Click(x, y);
            }


        }
        public void ClickCheck(int x, int y)
        {
            for(int i = 0;i<_actionCommand.Length;i++)
            //foreach (var cmd in _actionCommand)
            {
                var cmd = _actionCommand[i];
                // コマンドが選択された
                if (cmd.Click(x, y))
                {
                    if (cmd.IsCallChi()) { }
                    if (cmd.IsCallPon()) {
                        mode_ = eMode.Wait; 
                        tehais[i].Pon(sutehai);
                        
                        turn_= i;
                        //牌を捨てる
                        /* ポンをしてturn_をその人に変える */
                    }
                    if (cmd.IsCallKan()) { }
                    if (cmd.IsCallRon()) { }

                    // コマンドを初期化
                    Array.ForEach(_actionCommand, e => e.Init());

                    return;
                }
            }

            if (tehais[turn_].List.Count >= 14)
            {
                Hai del = tehais[turn_].Click(x, y, kawas[turn_]);

                if (del != null)
                {
                    sutehai = del;
                    // コマンドを初期化
                    Array.ForEach(_actionCommand, e => e.Init());

                    tehais[turn_].Sort();
                    for (int i = 0; i < players; i++)
                    {
                        if (i == turn_)
                        {
                            continue;
                        }

                        AtariList atariList = new AtariList(tehais[i], del);
                        if (atariList.IsAtari())
                        {
                            Ron = true;
                        }

                        // ポンのコマンドを有効にする
                        if (tehais[i].IsCanPon(del))
                        {
                            _actionCommand[i].CanPon = true;
                        }
                    }
                }
            }
        }



        public void Draw(Graphics g)
        {
            for (int i = 0; i < players; i++)
            {
                tehais[i].Draw(g, i);
                kawas[i].Draw(g, i);
                //naki[i].Draw(g, i);
                _actionCommand[i].Draw(g);
            }
        }
    }
}
