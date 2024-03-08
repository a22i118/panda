using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static server.Hai.eName;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace server
{
    internal class GameManager
    {
        // _を付ける
        private Yama yama = new Yama();
        // _を付ける
        private WanPai wanPai = new WanPai();

        // これらをPlayerクラスに移行すること
        private Tehai[] tehais = new Tehai[Player.Num] { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
        private Kawa[] kawas = new Kawa[Player.Num] { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
        private ActionCommand[] _actionCommand = new ActionCommand[Player.Num];
        private AtariList[] _atariList = new AtariList[Player.Num];

        private Player[] _players = new Player[Player.Num] {
            new Player(0),
            new Player(1),
            new Player(2),
            new Player(3)
        };

        // _は前に付ける
        int turn_ = 0;

        // _を付ける
        Hai sutehai = null;
        private bool _tsumo = false;
        private bool _ron = false;
        private bool _ryukyoku = false;

        public enum eMode
        {
            Tsumo,
            Wait,
            RinshanTsumo,
        }
        eMode _mode = eMode.Tsumo;

        public GameManager()
        {
            Init();
        }

        private void gameStart()
        {
            yama.Init();

            for (int i = 0; i < Player.Num; i++)
            {
                tehais[i].Init();
                kawas[i].Init();
            }

            turn_ = 0;  // TODO:親を入れる
            turn_ = (turn_ - 1 + 4) % 4;    // ツモで進めるので１戻しておく

            sutehai = null;

            _tsumo = false;
            _ron = false;
            _ryukyoku = false;
            _mode = eMode.Tsumo;

            // 鳴きのテストのために積み込み
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Pinzu1, Pinzu2, Pinzu3, Pinzu4, Souzu1, Souzu2, Souzu3, Souzu4, Souzu5 });
            //yama.Tsumikomi(0, new Hai.eName[] { Ton, Ton, Ton, Nan, Nan, Nan, Nan, Sha, Sha, Sha, Sha, Pei, Pei });
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu9, Pinzu1, Pinzu9, Souzu1, Souzu9, Ton, Nan, Sha, Pei, Haku, Hatu, Thun });  // 国士無双
            //yama.Tsumikomi(0, new Hai.eName[] { Ton, Nan, Nan, Sha, Sha, Pei, Pei, Haku, Haku, Hatu, Hatu, Thun, Thun });   // 七対子
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu1, Manzu9, Manzu9, Manzu9, Pinzu9, Pinzu9, Pinzu9, Souzu1, Souzu1, Souzu9, Souzu9 });  // 清老頭
            //yama.Tsumikomi(0, new Hai.eName[] { Pei, Pei, Pei, Haku, Haku, Haku, Hatu, Hatu, Thun, Thun, Thun, Souzu9, Souzu9 });  // 大三元
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu2, Manzu2, Manzu3, Manzu4, Manzu5, Manzu6, Manzu7, Manzu8, Manzu9, Manzu9, Manzu9 }); // 九蓮宝燈
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Manzu7, Manzu8, Manzu9, Ton }); // 一気通貫
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Pinzu1, Pinzu2, Pinzu3, Souzu1, Souzu2, Souzu3, Haku, Haku, Haku, Thun }); // 三色同順
            yama.Tsumikomi(0, new Hai.eName[] { Manzu9, Manzu9, Manzu9, Pinzu9, Pinzu9, Pinzu9, Souzu6, Souzu7, Souzu8, Souzu9, Souzu9, Souzu9, Thun }); // 三色同刻
            yama.Tsumikomi(1, new Hai.eName[] { Manzu1, Manzu1, Manzu2, Manzu2, Manzu3, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Thun });
            yama.Tsumikomi(2, new Hai.eName[] { Pinzu1, Pinzu1, Pinzu2, Pinzu2, Pinzu3, Pinzu3, Pinzu4, Pinzu4, Pinzu5, Pinzu5, Pinzu6, Pinzu6, Manzu7 });
            yama.Tsumikomi(3, new Hai.eName[] { Souzu1, Souzu1, Souzu2, Souzu2, Souzu3, Souzu3, Souzu4, Souzu4, Souzu5, Souzu5, Souzu6, Souzu6, Souzu7 });

            //王牌
            wanPai.Init(yama);

            //四人に配る
            for (int i = 0; i < Player.Num; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    tehais[i].Add(yama.Tsumo());
                }
            }

            // 流局のテスト
            //{
            //    int yama_num = yama.List.Count;
            //    for (int i = 0; i < yama_num - 4; i++)
            //    {
            //        yama.Tsumo();
            //    }
            //}

            //手牌のソート
            for (int i = 0; i < Player.Num; i++)
            {
                tehais[i].Sort();
            }
        }

        public void Init()
        {
            for (int i = 0; i < Player.Num; i++)
            {
                _actionCommand[i] = new ActionCommand(300, i * 200 + 74, 64, 32);
            }

            gameStart();
        }

        //async Task Restart(int sec)
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(sec));
        //    gameStart();
        //}

        public void Exec()
        {
            if (_tsumo || _ron || _ryukyoku)
            {
                return;
            }

            if (_mode == eMode.RinshanTsumo)
            {
                if (tehais[turn_].IsCanTsumo())
                {
                    Hai hai = wanPai.Tsumo();
                    if (hai != null)
                    {
                        tehais[turn_].Add(hai);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _atariList[turn_] = new AtariList(tehais[turn_]);

                if (_atariList[turn_].IsAtari())
                {
                    _actionCommand[turn_].CanTsumo = true;
                    //Console.WriteLine("アタリ");
                }

                if (tehais[turn_].IsCanAnKan() || tehais[turn_].IsCanKaKan())
                {
                    _actionCommand[turn_].CanKan = true;
                }

                _mode = eMode.Wait;
            }
            else if (_mode == eMode.Tsumo)
            {
                // コマンド入力を待っている間はツモらない
                foreach (var cmd in _actionCommand)
                {
                    if (cmd.IsCanAny())
                    {
                        return;
                    }
                }

                turn_ = (turn_ + 1) % 4;

                if (tehais[turn_].IsCanTsumo())
                {
                    Hai hai = yama.Tsumo();
                    if (hai != null)
                    {
                        tehais[turn_].Add(hai);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _atariList[turn_] = new AtariList(tehais[turn_]);

                if (_atariList[turn_].IsAtari())
                {
                    _actionCommand[turn_].CanTsumo = true;
                    //Console.WriteLine("アタリ");
                }

                if (tehais[turn_].IsCanAnKan() || tehais[turn_].IsCanKaKan())
                {
                    _actionCommand[turn_].CanKan = true;
                }

                _mode = eMode.Wait;
            }
            else
            {
                if (tehais[turn_].IsCanTsumo())
                {
                    _mode = eMode.Tsumo;
                }
                //tehais[turn_].Click(x, y);
            }
        }

        public void ClickCheck(int x, int y)
        {
            if (_tsumo || _ron || _ryukyoku)
            {
                gameStart();

                return;
            }

            for (int i = 0; i < _actionCommand.Length; i++)
            //foreach (var cmd in _actionCommand)
            {
                var cmd = _actionCommand[i];
                // コマンドが選択された
                if (cmd.Click(x, y))
                {
                    bool init = true;
                    if (cmd.IsCallChi())
                    {
                        if (tehais[i].Chi(sutehai))
                        {
                            _mode = eMode.Wait;
                            turn_ = i;
                        }
                        else
                        {
                            init = false;
                        }
                    }
                    if (cmd.IsCallPon())
                    {
                        tehais[i].Pon(sutehai, (turn_ - i + Player.Num) % Player.Num);
                        _mode = eMode.Wait;
                        turn_ = i;
                    }
                    if (cmd.IsCallKan())
                    {
                        if (tehais[i].IsCanTsumo())
                        {
                            tehais[i].MinKan(sutehai, (turn_ - i + Player.Num) % Player.Num);
                            _mode = eMode.RinshanTsumo;
                            turn_ = i;
                        }
                        else
                        {
                            if (tehais[i].AnKan())
                            {
                                _mode = eMode.RinshanTsumo;
                                turn_ = i;
                            }
                            else
                            {
                                init = false;
                            }
                        }
                    }
                    if (cmd.IsCallRon())
                    {
                        turn_ = i;
                        _mode = eMode.Wait;
                        _ron = true;
                    }
                    if (cmd.IsCallTsumo())
                    {
                        turn_ = i;
                        _mode = eMode.Wait;
                        _tsumo = true;
                    }

                    // コマンドを初期化
                    if (init)
                    {
                        Array.ForEach(_actionCommand, e => e.Init());
                        Array.ForEach(tehais, e => e.ResetNakikouho());
                    }

                    return;
                }
            }

            if (!tehais[turn_].IsCanTsumo())
            {
                if (_actionCommand[turn_].IsCallKan())
                {
                    Hai hai = tehais[turn_].Click(x, y);
                    if (hai != null)
                    {
                        if (hai.Nakichoice)
                        {
                            hai.Nakichoice = false;
                        }
                        else if (hai.Nakikouho)
                        {
                            hai.Nakichoice = true;
                            if (tehais[turn_].AnKan())
                            {
                                _mode = eMode.RinshanTsumo;
                                //turn_ = player;
                                Array.ForEach(_actionCommand, e => e.Init());
                                Array.ForEach(tehais, e => e.ResetNakikouho());
                            }
                            hai.Nakichoice = true;
                        }
                    }
                }
                else
                {
                    Hai hai = tehais[turn_].Click(x, y, kawas[turn_]);
                    if (hai != null)
                    {
                        sutehai = hai;
                        // コマンドを初期化
                        Array.ForEach(_actionCommand, e => e.Init());

                        tehais[turn_].Sort();

                        for (int shimocha = 1; shimocha < Player.Num; shimocha++)
                        {
                            int player = (turn_ + shimocha) % Player.Num;

                            _atariList[player] = new AtariList(tehais[player], hai);

                            if (_atariList[player].IsAtari())
                            {
                                _actionCommand[player].CanRon = true;
                            }

                            // チーのコマンドを有効にする
                            if (shimocha == 1 && tehais[player].IsCanChi(hai))
                            {
                                _actionCommand[player].CanChi = true;
                            }

                            // ポンのコマンドを有効にする
                            if (tehais[player].IsCanPon(hai))
                            {
                                _actionCommand[player].CanPon = true;
                            }

                            // カンのコマンドを有効にする
                            if (tehais[player].IsCanMinKan(hai))
                            {
                                _actionCommand[player].CanKan = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int player = 0; player < Player.Num; player++)
                {
                    Hai hai = tehais[player].Click(x, y);
                    if (hai != null)
                    {
                        if (hai.Nakichoice)
                        {
                            hai.Nakichoice = false;
                        }
                        else if (hai.Nakikouho)
                        {
                            hai.Nakichoice = true;
                            if (tehais[player].Chi(sutehai))
                            {
                                _mode = eMode.Wait;
                                turn_ = player;
                                Array.ForEach(_actionCommand, e => e.Init());
                                Array.ForEach(tehais, e => e.ResetNakikouho());
                            }
                            hai.Nakichoice = true;
                        }
                    }
                }
            }
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < Player.Num; i++)
            {
                tehais[i].Draw(g, i);
                kawas[i].Draw(g, i);
                //naki[i].Draw(g, i);
                _actionCommand[i].Draw(g, i == turn_);
            }

            Font font = new Font(new FontFamily("Arial"), 48, FontStyle.Bold);
            Font font_small = new Font(new FontFamily("Arial"), 16, FontStyle.Bold);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            if (_tsumo || _ron)
            {
                g.DrawString(_tsumo ? "ツモ" : "ロン", font, Brushes.White, new PointF(512, 304));

                string[] yakus = _atariList[turn_].YakuString();
                List<string> list = new List<string>();

                foreach (var yaku in yakus)
                {
                    if (list.Find(e => e == yaku) == null)
                    {
                        list.Add(yaku);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    g.DrawString($"{list[i]}", font_small, Brushes.White, new PointF(32, 64 + 32 * i));
                }
            }
            if (_ryukyoku)
            {
                g.DrawString("流局", font, Brushes.White, new PointF(512, 304));
            }
        }
    }
}
