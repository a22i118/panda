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
        private Yama _yama = new Yama();
        private WanPai _wanPai = new WanPai();

        // これらをPlayerクラスに移行すること
        //private Tehai[] tehais = new Tehai[Player.Num] { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
        private Kawa[] kawas = new Kawa[Player.Num] { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
        private ActionCommand[] _actionCommand = new ActionCommand[Player.Num];
        private AtariList[] _atariList = new AtariList[Player.Num];

        private Player[] _players = new Player[Player.Num] {
            new Player(0),
            new Player(1),
            new Player(2),
            new Player(3)
        };

        int _turn = 0;

        Hai _sutehai = null;
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
            _yama.Init();

            for (int i = 0; i < Player.Num; i++)
            {
                _players[i].Init();
            }

            _turn = 0;  // TODO:親を入れる
            _turn = (_turn - 1 + 4) % 4;    // ツモで進めるので１戻しておく

            _sutehai = null;

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
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Manzu7, Manzu8, Manzu9, Thun }); // 一気通貫
            //yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Pinzu1, Pinzu2, Pinzu3, Souzu1, Souzu2, Souzu3, Haku, Haku, Haku, Thun }); // 三色同順
            _yama.Tsumikomi(0, new Hai.eName[] { Manzu9, Manzu9, Manzu9, Pinzu9, Pinzu9, Pinzu9, Souzu6, Souzu7, Souzu8, Souzu9, Souzu9, Souzu9, Thun }); // 三色同刻
            _yama.Tsumikomi(1, new Hai.eName[] { Manzu9, Manzu1, Manzu2, Manzu2, Manzu3, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Thun });
            _yama.Tsumikomi(2, new Hai.eName[] { Pinzu1, Pinzu1, Pinzu2, Pinzu2, Pinzu3, Pinzu3, Pinzu4, Pinzu4, Pinzu5, Pinzu5, Pinzu6, Pinzu6, Manzu7 });
            _yama.Tsumikomi(3, new Hai.eName[] { Souzu1, Souzu1, Souzu2, Souzu2, Souzu3, Souzu3, Souzu4, Souzu4, Souzu5, Souzu5, Souzu6, Souzu6, Souzu7 });

            //王牌
            _wanPai.Init(_yama);

            //四人に配る
            for (int i = 0; i < Player.Num; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    _players[i].Tsumo(_yama.Tsumo());
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
                _players[i].Sort();
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
                if (_players[_turn].IsCanTsumo())
                {
                    Hai hai = _wanPai.Tsumo();
                    if (hai != null)
                    {
                        _players[_turn].Tsumo(hai);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _atariList[_turn] = new AtariList(_players[_turn]._tehai);

                if (_atariList[_turn].IsAtari())
                {
                    _actionCommand[_turn].CanTsumo = true;
                    //Console.WriteLine("アタリ");
                }

                if (_players[_turn].IsCanAnKan() || _players[_turn].IsCanKaKan())
                {
                    _actionCommand[_turn].CanKan = true;
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

                _turn = (_turn + 1) % 4;

                if (_players[_turn].IsCanTsumo())
                {
                    Hai hai = _yama.Tsumo();
                    if (hai != null)
                    {
                        _players[_turn].Tsumo(hai);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _atariList[_turn] = new AtariList(_players[_turn]._tehai);

                if (_atariList[_turn].IsAtari())
                {
                    _actionCommand[_turn].CanTsumo = true;
                    //Console.WriteLine("アタリ");
                }

                if (_players[_turn].IsCanAnKan() || _players[_turn].IsCanKaKan())
                {
                    _actionCommand[_turn].CanKan = true;
                }

                _mode = eMode.Wait;
            }
            else
            {
                if (_players[_turn].IsCanTsumo())
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
                        if (_players[i].Chi(_sutehai))
                        {
                            _mode = eMode.Wait;
                            _turn = i;
                        }
                        else
                        {
                            init = false;
                        }
                    }
                    if (cmd.IsCallPon())
                    {
                        _players[i].Pon(_sutehai, _turn);
                        _mode = eMode.Wait;
                        _turn = i;
                    }
                    if (cmd.IsCallKan())
                    {
                        if (_players[i].IsCanTsumo())
                        {
                            _players[i].MinKan(_sutehai, _turn);
                            _mode = eMode.RinshanTsumo;
                            _turn = i;
                        }
                        else
                        {
                            if (_players[i].AnKan())
                            {
                                _mode = eMode.RinshanTsumo;
                                _turn = i;
                            }
                            else
                            {
                                init = false;
                            }
                        }
                    }
                    if (cmd.IsCallRon())
                    {
                        _turn = i;
                        _mode = eMode.Wait;
                        _ron = true;
                    }
                    if (cmd.IsCallTsumo())
                    {
                        _turn = i;
                        _mode = eMode.Wait;
                        _tsumo = true;
                    }

                    // コマンドを初期化
                    if (init)
                    {
                        Array.ForEach(_actionCommand, e => e.Init());
                        Array.ForEach(_players, e => e.ResetNakikouho());
                    }

                    return;
                }
            }

            if (!_players[_turn].IsCanTsumo())
            {
                if (_actionCommand[_turn].IsCallKan())
                {
                    Hai hai = _players[_turn].Click(x, y);
                    if (hai != null)
                    {
                        if (hai.Nakichoice)
                        {
                            hai.Nakichoice = false;
                        }
                        else if (hai.Nakikouho)
                        {
                            hai.Nakichoice = true;
                            if (_players[_turn].AnKan())
                            {
                                _mode = eMode.RinshanTsumo;
                                //turn_ = player;
                                Array.ForEach(_actionCommand, e => e.Init());
                                Array.ForEach(_players, e => e.ResetNakikouho());
                            }
                            hai.Nakichoice = true;
                        }
                    }
                }
                else
                {
                    Hai hai = _players[_turn].Click(x, y, kawas[_turn]);
                    if (hai != null)
                    {
                        _sutehai = hai;
                        // コマンドを初期化
                        Array.ForEach(_actionCommand, e => e.Init());

                        _players[_turn].Sort();

                        for (int shimocha = 1; shimocha < Player.Num; shimocha++)
                        {
                            int player = (_turn + shimocha) % Player.Num;

                            _atariList[player] = new AtariList(_players[player]._tehai, hai);

                            if (_atariList[player].IsAtari())
                            {
                                _actionCommand[player].CanRon = true;
                            }

                            // チーのコマンドを有効にする
                            if (shimocha == 1 && _players[player].IsCanChi(hai))
                            {
                                _actionCommand[player].CanChi = true;
                            }

                            // ポンのコマンドを有効にする
                            if (_players[player].IsCanPon(hai))
                            {
                                _actionCommand[player].CanPon = true;
                            }

                            // カンのコマンドを有効にする
                            if (_players[player].IsCanMinKan(hai))
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
                    Hai hai = _players[player].Click(x, y);
                    if (hai != null)
                    {
                        if (hai.Nakichoice)
                        {
                            hai.Nakichoice = false;
                        }
                        else if (hai.Nakikouho)
                        {
                            hai.Nakichoice = true;
                            if (_players[player].Chi(_sutehai))
                            {
                                _mode = eMode.Wait;
                                _turn = player;
                                Array.ForEach(_actionCommand, e => e.Init());
                                Array.ForEach(_players, e => e.ResetNakikouho());
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
                _players[i].Draw(g);
                kawas[i].Draw(g, i);
                //naki[i].Draw(g, i);
                _actionCommand[i].Draw(g, i == _turn);
            }

            Font font = new Font(new FontFamily("Arial"), 48, FontStyle.Bold);
            Font font_small = new Font(new FontFamily("Arial"), 16, FontStyle.Bold);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            if (_tsumo || _ron)
            {
                g.DrawString(_tsumo ? "ツモ" : "ロン", font, Brushes.White, new PointF(512, 304));

                string[] yakus = _atariList[_turn].YakuString();
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
