using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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
        private Ba _ba = new Ba();


        // これらをPlayerクラスに移行すること
        //private Tehai[] tehais = new Tehai[Player.Num] { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
        //private Kawa[] kawas = new Kawa[Player.Num] { new Kawa(), new Kawa(), new Kawa(), new Kawa() };
        //private ActionCommand[] _actionCommand = new ActionCommand[Player.Num];
        //private AtariList[] _atariList = new AtariList[Player.Num];

        private Player[] _players = new Player[Player.Num] {
            new Player(0),
            new Player(1),
            new Player(2),
            new Player(3)
        };

        private int _turn = 0;

        private Hai? _sutehai = null;
        private bool _tsumo = false;
        private bool _ron = false;
        private bool _ryukyoku = false;
        private bool _sufomtsurenda = false;
        private bool _sukannagare = false;
        private bool _suchaReach = false;
        private bool _kyushukyuhai = false;
        private bool _isReach = false;


        private int _kansCount = 0;
        private int _reachCount = 0;
        public enum eMode
        {
            Tsumo,
            Wait,
            RinshanTsumo,
        }
        eMode _mode = eMode.Tsumo;

        public GameManager()
        {
            gameStart();
        }

        private void gameStart()
        {
            _yama.Init();

            for (int i = 0; i < Player.Num; i++)
            {
                _players[i].Init();
            }

            _turn = 0;  // TODO:親を入れる
            _players[_turn].IsOya = true;
            _turn = (_turn - 1 + 4) % 4;    // ツモで進めるので１戻しておく

            _sutehai = null;

            _tsumo = false;
            _ron = false;
            _ryukyoku = false;
            _sufomtsurenda = false;
            _sukannagare = false;
            _suchaReach = false;
            _kyushukyuhai = false;
            _isReach = false;
            _mode = eMode.Tsumo;
            _kansCount = 0;
            _reachCount = 0;

            // 鳴きのテストのために積み込み
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Pinzu1, Pinzu2, Pinzu3, Pinzu4, Souzu1, Souzu2, Souzu3, Souzu4, Nan });
            //_yama.Tsumikomi(0, new Hai.eName[] { Ton, Ton, Ton, Nan, Nan, Nan, Nan, Sha, Sha, Sha, Sha, Pei, Pei });
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu9, Pinzu1, Pinzu9, Souzu1, Souzu9, Ton, Nan, Sha, Pei, Haku, Hatu, Thun });  // 国士無双十三面
            //_yama.Tsumikomi(0, new Hai.eName[] { Ton, Nan, Nan, Sha, Sha, Pei, Pei, Haku, Haku, Hatu, Hatu, Thun, Thun });   // 七対子
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu1, Manzu9, Manzu9, Manzu9, Pinzu9, Pinzu9, Pinzu9, Souzu1, Souzu1, Souzu9, Souzu9 });  // 清老頭
            //_yama.Tsumikomi(0, new Hai.eName[] { Pei, Pei, Pei, Haku, Haku, Haku, Hatu, Hatu, Thun, Thun, Thun, Souzu9, Souzu9 });  // 大三元
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu1, Manzu2, Manzu3, Manzu4, Manzu5, Manzu6, Manzu7, Manzu8, Manzu9, Manzu9, Manzu9 }); // 純正九蓮宝燈
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Manzu7, Manzu8, Manzu9, Thun }); // 一気通貫
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Pinzu1, Pinzu2, Pinzu3, Souzu1, Souzu2, Souzu3, Haku, Haku, Haku, Thun }); // 三色同順
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu9, Manzu9, Manzu9, Pinzu9, Pinzu9, Pinzu9, Souzu6, Souzu7, Souzu8, Souzu9, Souzu9, Souzu9, Thun }); // 三色同刻
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu7, Manzu8, Manzu9, Pinzu7, Pinzu8, Pinzu9, Souzu6, Souzu7, Souzu7, Souzu8, Souzu9, Ton, Ton }); // 約牌
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu7, Manzu8, Manzu9, Pinzu7, Pinzu8, Pinzu9, Souzu6, Souzu7, Souzu7, Souzu8, Souzu9, Sha, Sha }); // 約牌
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu2, Manzu3, Manzu4, Manzu5, Manzu5, Manzu6, Manzu7, Souzu1, Souzu2, Souzu3, Souzu4, Souzu5, Souzu6 });
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu2, Manzu3, Manzu4, Manzu5, Manzu6, Manzu7, Souzu2, Souzu3, Souzu4, Souzu3, Souzu7, Ton, Ton });
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu2, Manzu3, Manzu4, Manzu5, Manzu6, Manzu7, Souzu2, Souzu3, Souzu4, Manzu5, Ton, Ton, Ton });
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu3, Ton, Ton, Ton, Nan, Nan, Nan, Sha, Sha, Sha, Sha, Pei, Pei });
            //_yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu1, Manzu2, Manzu2, Manzu4, Manzu5, Manzu6, Manzu7, Manzu8, Haku, Haku, Haku });
            _yama.Tsumikomi(0, new Hai.eName[] { Manzu1, Manzu1, Manzu1, Manzu2, Manzu3, Manzu4, Pinzu4, Pinzu5, Pinzu6, Souzu7, Souzu7, Haku, Haku });

            _yama.Tsumikomi(1, new Hai.eName[] { Manzu1, Manzu9, Pinzu1, Pinzu9, Souzu1, Souzu2, Souzu2, Souzu3, Souzu3, Pei, Pei, Souzu7, Thun });
            //_yama.Tsumikomi(1, new Hai.eName[] { Manzu3, Manzu3, Manzu3, Manzu6, Manzu7, Manzu8, Manzu8, Pinzu2, Ton, Nan, Pei, Pei, Thun });

            //_yama.Tsumikomi(1, new Hai.eName[] { Manzu1, Manzu2, Manzu3, Manzu4, Manzu4, Manzu5, Manzu5, Manzu6, Manzu6, Manzu7, Manzu8, Manzu9, Nan });
            _yama.Tsumikomi(2, new Hai.eName[] { Pinzu1, Pinzu1, Pinzu1, Pinzu2, Pinzu3, Pinzu3, Pinzu3, Pinzu3, Pinzu5, Pinzu5, Pinzu6, Pinzu6, Nan });
            _yama.Tsumikomi(3, new Hai.eName[] { Manzu3, Souzu1, Souzu2, Souzu2, Souzu3, Souzu3, Souzu4, Souzu4, Souzu5, Souzu5, Souzu6, Souzu6, Nan });




            //王牌
            _wanPai.Init(_yama);
            //ドラ
            foreach (var hai in _yama.Hais)
            {
                if (hai.Name == _wanPai.DoraNames.Last())
                {
                    hai.Dora += 1;
                }
            }
            foreach (var hai in _wanPai.Rinshams)
            {
                if (hai.Name == _wanPai.DoraNames.Last())
                {
                    hai.Dora += 1;
                }
            }
            //四人に配る
            for (int i = 0; i < Player.Num; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    _players[i].Deal(_yama.Tsumo());
                }
            }

            //嶺上テスト
            //_yama.Hais[0] = new Hai(Hai.Manzu1);
            //_wanPai.Rinshams[0] = new Hai(Hai.Manzu2);

            //河底テスト
            //List<Hai> testList = new List<Hai>();
            //for (int i = 0; i < 64; i++)
            //{
            //    testList.Add(_yama.Tsumo());
            //}

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
            //Dora();
        }
        public void Exec()
        {
            if (_tsumo || _ron || _ryukyoku || _sukannagare || _suchaReach || _sufomtsurenda || _kyushukyuhai)
            {
                return;
            }
            if (_yama.Hais.Count() == 0)
            {
                foreach (var player in _players)
                {
                    player.IsHaiteiHoutei = true;
                }
            }
            if (SukanCheck())
            {
                _sukannagare = true;
            }
            if (_reachCount == 4)
            {
                _suchaReach = true;
            }
            if (_players.Count(e => e.SarashiCount() > 0) == 0) //全員鳴いていない
            {
                if (SufomtsuCheck())
                {
                    _sufomtsurenda = true;
                }
                if (_players[_turn].Tehai.KyushuCheck())
                {

                    //_kyushukyuhai = true;
                }
            }


            if (_mode == eMode.RinshanTsumo)
            {
                if (_players[_turn].IsCanTsumo())
                {
                    _players[_turn].Tehai.IsRinshan = true;
                    Hai hai = _wanPai.Tsumo();
                    if (hai != null)
                    {
                        _players[_turn].Tsumo(hai, yakuMask(_turn), _kansCount);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _mode = eMode.Wait;
            }
            else if (_mode == eMode.Tsumo)
            {

                // コマンド入力を待っている間はツモらない
                foreach (var player in _players)
                {
                    if (player.IsCanAny())
                    {
                        return;
                    }
                }

                _turn = (_turn + 1) % 4;

                //リーチ後のフリテンチェック
                foreach (var player in _players)
                {
                    player.HuritenCheck(_sutehai);
                }

                if (_players[_turn].IsCanTsumo())
                {
                    Hai hai = _yama.Tsumo();
                    if (hai != null)
                    {
                        _players[_turn].Tsumo(hai, yakuMask(_turn), _kansCount);
                    }
                    else
                    {
                        _ryukyoku = true;
                        return;
                    }
                }

                _mode = eMode.Wait;
            }
            else
            {
                if (_players[_turn].IsCanTsumo())
                {

                    if (_players[_turn].Tehai.NowReach == false)
                    {
                        _players[_turn].Tempai(_players[_turn].Tehai, yakuMask(_turn));
                    }
                    _mode = eMode.Tsumo;
                }
            }
        }


        public void ClickCheck(int x, int y)
        {
            if (_tsumo || _ron || _ryukyoku)
            {
                gameStart();

                return;
            }
            foreach (var player in _players)
            {
                // コマンドが選択された
                if (player.CommandUpdate(x, y))
                {
                    bool init = true;
                    if (player.IsCallChi())
                    {
                        player.ActionCommand.CanPon = false;
                        player.ActionCommand.CanKan = false;
                        foreach (var Allplayer in _players)
                        {
                            Allplayer.Tehai.IsIppatsu = false;
                        }
                        if (player.Chi(_sutehai))
                        {
                            player.IsEnableReach(yakuMask(player.Id));
                            player.ChoiceTempai();
                            _mode = eMode.Wait;
                            _turn = player.Id;
                        }
                        else
                        {
                            init = false;
                        }
                    }
                    if (player.IsCallPon())
                    {
                        foreach (var Allplayer in _players)
                        {
                            Allplayer.Tehai.IsIppatsu = false;
                        }
                        player.Pon(_sutehai, _turn);
                        player.IsEnableReach(yakuMask(player.Id));
                        player.ChoiceTempai();
                        _mode = eMode.Wait;
                        _turn = player.Id;
                    }
                    if (player.IsCallKan())
                    {
                        _kansCount += 1;
                        Dora();

                        if (player.IsCanKaKan())
                        {
                            Hai hai = player.Tehai.Pons[player.Tehai.Pons.Count() - 1].Hais[0];
                        }
                        if (player.IsCanTsumo())
                        {
                            player.MinKan(_sutehai, _turn);
                            _mode = eMode.RinshanTsumo;
                            _turn = player.Id;
                        }
                        else
                        {
                            if (player.AnKan())
                            {
                                Hai anKanHai = player.Tehai.Kans[player.Tehai.Kans.Count() - 1].Hais[0];
                                _mode = eMode.RinshanTsumo;
                                _turn = player.Id;
                            }
                            else
                            {
                                init = false;
                            }
                        }
                    }
                    //ロン
                    if (player.Huriten == false)
                    {
                        if (player.IsCallRon())
                        {
                            _turn = player.Id;
                            _mode = eMode.Wait;
                            _ron = true;
                        }
                    }
                    //リーチ
                    if (player.Tehai.NowReach == false && player.NakiCount() == 0)
                    {
                        if (player.IsCallRichi())
                        {
                            _isReach = true;
                            player.Reach();
                            _turn = player.Id;
                            _mode = eMode.Wait;

                        }
                    }



                    if (player.IsCallTsumo())
                    {
                        _turn = player.Id;
                        _mode = eMode.Wait;
                        _tsumo = true;
                    }

                    // コマンドを初期化
                    if (init)
                    {
                        Array.ForEach(_players, e => e.ResetNakikouho());
                    }

                    return;
                }
            }
            if (!_players[_turn].IsCanTsumo())
            {


                if (_players[_turn].IsCallKan())
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
                                Hai anKanHai = _players[_turn].Tehai.Kans[_players[_turn].Tehai.Kans.Count() - 1].Hais[0];

                                _kansCount += 1;
                                Dora();
                                _mode = eMode.RinshanTsumo;
                                Array.ForEach(_players, e => e.ResetNakikouho());
                            }
                            hai.Nakichoice = true;
                        }
                    }
                }
                else
                {
                    ulong reachiMask = 0;
                    Hai hai = _players[_turn].Throw(x, y);

                    //リーチ中は待ち表示を固定
                    if (_players[_turn].Tehai.DeclareReach && _players[_turn].Tehai.NowReach)
                    {
                        _reachCount += 1;
                        if (_players.Count(e => e.SarashiCount() > 0) == 0 && _yama.TsumoCount <= 4)
                        {
                            _players[_turn].IsDabReach = true;
                        }
                        else
                        {
                            _players[_turn].IsReach = true;
                        }
                        _players[_turn].RichiAtariHais = new List<Hai>(_players[_turn].ChoiceAtariHais);
                        _players[_turn].ChoiceAtariHais.Clear();
                        _players[_turn].Tehai.DeclareReach = false;
                    }

                    if (_players[_turn].Tehai.NowReach == false)
                    {
                        _players[_turn].ChoiceTempai();
                    }

                    //振聴チェック
                    _players[_turn].HuritenCheck(hai);

                    if (hai != null)
                    {
                        _sutehai = hai;
                        // コマンドを初期化

                        _players[_turn].Sort();

                        for (int shimocha = 1; shimocha < Player.Num; shimocha++)
                        {
                            int player = (_turn + shimocha) % Player.Num;
                            _players[player].CommandValid(hai, _players[player].Tehai, _kansCount, reachiMask | yakuMask(player), shimocha == 1);
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
                                Array.ForEach(_players, e => e.ResetNakikouho());
                            }
                            hai.Nakichoice = true;
                        }
                    }
                }
            }
        }

        // 場で決まる役
        private ulong yakuMask(int player)
        {
            ulong yakuMask = 0;

            Ba.eKaze bakaze = _ba.BaKaze;
            Ba.eKaze jikaze = _ba.ZiKaze(player);

            if (bakaze == jikaze)
            {
                yakuMask |= bakaze == Ba.eKaze.Ton ? Yaku.DabuTon.Mask : 0; // ダブ東
                yakuMask |= bakaze == Ba.eKaze.Nan ? Yaku.DabuNan.Mask : 0; // ダブ南
                yakuMask |= bakaze == Ba.eKaze.Sha ? Yaku.DabuSha.Mask : 0; // ダブ西
                yakuMask |= bakaze == Ba.eKaze.Pei ? Yaku.DabuPei.Mask : 0; // ダブ北
            }
            else
            {
                yakuMask |= bakaze == Ba.eKaze.Ton ? Yaku.Yakuhai_Ton.Mask : 0; // 東
                yakuMask |= bakaze == Ba.eKaze.Nan ? Yaku.Yakuhai_Nan.Mask : 0; // 南
                yakuMask |= bakaze == Ba.eKaze.Sha ? Yaku.Yakuhai_Sha.Mask : 0; // 西
                yakuMask |= bakaze == Ba.eKaze.Pei ? Yaku.Yakuhai_Pei.Mask : 0; // 北
                yakuMask |= jikaze == Ba.eKaze.Ton ? Yaku.Yakuhai_Ton.Mask : 0; // 東
                yakuMask |= jikaze == Ba.eKaze.Nan ? Yaku.Yakuhai_Nan.Mask : 0; // 南
                yakuMask |= jikaze == Ba.eKaze.Sha ? Yaku.Yakuhai_Sha.Mask : 0; // 西
                yakuMask |= jikaze == Ba.eKaze.Pei ? Yaku.Yakuhai_Pei.Mask : 0; // 北
            }

            // 誰も鳴いていない
            int sarashiPlayerNum = _players.Count(e => e.SarashiCount() > 0);
            if (sarashiPlayerNum == 0)
            {
                // 天和
                if (_yama.TsumoCount == 1 && jikaze == Ba.eKaze.Ton) { yakuMask |= Yaku.Tenho.Mask; }

                // 地和
                if (_yama.TsumoCount == 2 && jikaze == Ba.eKaze.Nan ||
                    _yama.TsumoCount == 3 && jikaze == Ba.eKaze.Sha ||
                    _yama.TsumoCount == 4 && jikaze == Ba.eKaze.Pei) { yakuMask |= Yaku.Chiho.Mask; }

                // 人和
                if (_yama.TsumoCount < 2 && jikaze == Ba.eKaze.Nan ||
                    _yama.TsumoCount < 3 && jikaze == Ba.eKaze.Sha ||
                    _yama.TsumoCount < 4 && jikaze == Ba.eKaze.Pei) { yakuMask |= Yaku.Renho.Mask; }
            }

            return yakuMask;
        }
        private bool SukanCheck()
        {
            int tmp = 0;
            foreach (var player in _players)
            {
                if (player.KansCount != 4)
                {
                    tmp += player.KansCount;
                }
            }
            if (tmp == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SufomtsuCheck()
        {
            if (_players.Count(e => e.Kawa.Hais.Count() == 1) == 4) // 河が全員１
            {
                (Hai.eState all, Hai.eState any) state = (Hai.eState.All, 0);

                foreach (var player in _players)
                {
                    state.all &= player.Kawa.Hais[0].State;
                    state.any |= player.Kawa.Hais[0].State;
                }

                return
                    Hai.HaiInfo.IsAll(state, Hai.eState.Ton) ||
                    Hai.HaiInfo.IsAll(state, Hai.eState.Nan) ||
                    Hai.HaiInfo.IsAll(state, Hai.eState.Sha) ||
                    Hai.HaiInfo.IsAll(state, Hai.eState.Pei);
            }
            else
            {
                return false;
            }
        }
        private void Dora()
        {
            foreach (var hai in _yama.Hais)
            {
                if (hai.Name == _wanPai.DoraNames.Last())
                {
                    hai.Dora += 1;
                }
            }
            foreach (var hai in _wanPai.Rinshams)
            {
                if (hai.Name == _wanPai.DoraNames.Last())
                {
                    hai.Dora += 1;
                }
            }
            foreach (var player in _players)
            {
                player.Tehai.IsIppatsu = false;
                foreach (var hai in player.Tehai.Hais)
                {
                    if (hai.Name == _wanPai.DoraNames.Last())
                    {
                        hai.Dora += 1;
                    }
                }
                foreach (var hai in player.Kawa.Hais)
                {
                    if (hai.Name == _wanPai.DoraNames.Last())
                    {
                        hai.Dora += 1;
                    }
                }
            }
        }
        public void Draw(Graphics g)
        {
            _wanPai.Draw(g, _kansCount);
            for (int i = 0; i < Player.Num; i++)
            {
                _players[i].Draw(g, i == _turn);
            }
            Font font2 = new Font(new FontFamily("HGS行書体"), 24, FontStyle.Bold);
            g.DrawString("余", font2, Brushes.White, new PointF(50, 50));
            g.DrawString(_yama.Hais.Count().ToString(), font2, Brushes.White, new PointF(100, 50));

            Font font = new Font(new FontFamily("HGS行書体"), 48, FontStyle.Bold);

            if (_tsumo || _ron || _suchaReach || _sufomtsurenda || _sukannagare || _ryukyoku || _kyushukyuhai)
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                g.FillRectangle(brush, 25, 40, 1500, 900);

                if (_tsumo || _ron)
                {
                    g.DrawString(_tsumo ? "ツモ" : "ロン", font, Brushes.Red, new PointF(1050, _turn * 200 + 150));
                    List<Result> results = _players[_turn].Results;
                    int index = 0;
                    //foreach (Result result in results)
                    //{
                    //    result.Draw(g, new PointF(40, 64 + 32 * index++));
                    //}
                    results[0].Draw(g, new PointF(40, 64 + 32 * index++));
                    _wanPai.AgariDraw(g, _isReach, _kansCount);
                    _players[_turn].AgariDraw(g, _tsumo ? null : _sutehai);
                }
                else if (_sukannagare)
                {
                    g.DrawString("四槓流れ", font, Brushes.Purple, new PointF(512, 304));
                }
                else if (_suchaReach)
                {
                    g.DrawString("四家立直", font, Brushes.Purple, new PointF(512, 304));
                }
                else if (_sufomtsurenda)
                {
                    g.DrawString("四風子連打", font, Brushes.Purple, new PointF(512, 304));
                }
                else if (_kyushukyuhai)
                {
                    g.DrawString("九種九牌", font, Brushes.Purple, new PointF(512, 304));
                }
                else if (_ryukyoku)
                {
                    g.DrawString("流局", font, Brushes.Purple, new PointF(512, 304));
                }
            }
        }
    }
}
