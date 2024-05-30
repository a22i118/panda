using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace server
{
    internal class CheckTehai
    {
        private List<Toitsu> _toitsu;
        private List<Kotsu> _kotsu;
        private List<Shuntsu> _shuntsu;

        private List<Chi> _chis;
        private List<Pon> _pons;
        private List<Kan> _kans;
        private List<Hai> _hais;

        private Hai _atariHai;
        private bool _ronAgari;
        private eMachi _machi = eMachi.None;
        private bool _menzen = true;

        private List<IMentsu> _mentsus = new List<IMentsu>();

        private ulong _yakuMask = 0;

        private ulong _undecidedMask = 0;
        private int _fu = 0;
        private bool _isOya;
        private bool _IsIppatsu = false;
        private (eState all, eState any) _state = (eState.All, 0);

        public bool IsAgari() { return _hais.Count == 0; }

        public CheckTehai(Tehai tehai, bool isoya, ulong undecidedMask, Hai? add = null)
        {
            this._toitsu = new List<Toitsu>();
            this._kotsu = new List<Kotsu>();
            this._shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this._hais = new List<Hai>(tehai.Hais);
            this._isOya = isoya;
            if (tehai.IsIppatsu)
            {
                _IsIppatsu = true;
            }

            if (add != null)
            {
                // ロンと重複しない天和、地和を消す
                _undecidedMask = undecidedMask & ~(Yaku.Tenho.Mask | Yaku.Chiho.Mask);
                _ronAgari = true;
                _atariHai = add;
                this._hais.Add(add);
            }
            else
            {
                // ツモと重複しない人和を消す
                _undecidedMask = undecidedMask & ~(Yaku.Renho.Mask);
                _ronAgari = false;
                _atariHai = tehai.Hais.Last();
            }


            this._hais.Sort((a, b) => (int)a.Name - (int)b.Name);


            init();
        }


        public CheckTehai(CheckTehai checkTehai)
        {
            this._toitsu = new List<Toitsu>(checkTehai._toitsu);
            this._kotsu = new List<Kotsu>(checkTehai._kotsu);
            this._shuntsu = new List<Shuntsu>(checkTehai._shuntsu);

            this._chis = new List<Chi>(checkTehai._chis);
            this._pons = new List<Pon>(checkTehai._pons);
            this._kans = new List<Kan>(checkTehai._kans);
            this._hais = new List<Hai>(checkTehai._hais);

            this._undecidedMask = checkTehai._undecidedMask;
            this._ronAgari = checkTehai._ronAgari;
            this._atariHai = checkTehai._atariHai;
            this._isOya = checkTehai._isOya;

            init();
        }

        private void init()
        {
            _mentsus.Clear();
            _state.all = eState.All;
            _state.any = 0;
            //_IsIppatsu = false;

            _mentsus.AddRange(this._toitsu);
            _mentsus.AddRange(this._kotsu);
            _mentsus.AddRange(this._shuntsu);
            _mentsus.AddRange(this._chis);
            _mentsus.AddRange(this._pons);
            _mentsus.AddRange(this._kans);

            foreach (var mentsu in _mentsus)
            {
                _state.all &= mentsu.StateAll;
                _state.any |= mentsu.StateAny;
            }
            foreach (var hai in _hais)
            {
                _state.all &= hai.State;
                _state.any |= hai.State;
            }
            _menzen = true;
            _mentsus.ForEach(e => { _menzen &= e.IsMenzen(); });
        }

        public CheckTehai AddToitsu(bool isToitsu)
        {
            if (!isToitsu && _hais.Count >= 2 && _hais[0].Name == _hais[1].Name)
            {
                CheckTehai tmp = new CheckTehai(this);
                tmp._toitsu.Add(new Toitsu(tmp._hais[0], tmp._hais[1]));
                tmp._hais.RemoveAt(1);
                tmp._hais.RemoveAt(0);
                return tmp;
            }
            return null;
        }

        public CheckTehai AddKotsu()
        {
            if (_hais.Count >= 3 && _hais[0].Name == _hais[1].Name && _hais[0].Name == _hais[2].Name)
            {
                CheckTehai tmp = new CheckTehai(this);
                tmp._kotsu.Add(new Kotsu(tmp._hais[0], tmp._hais[1], tmp._hais[2]));
                tmp._hais.RemoveAt(2);
                tmp._hais.RemoveAt(1);
                tmp._hais.RemoveAt(0);
                return tmp;
            }
            return null;
        }

        public CheckTehai AddShuntsu()
        {
            if (_hais.Count >= 3)
            {
                Hai.eType type = _hais[0].Type;
                Hai.eNumber number = _hais[0].Number;

                // 字牌は順子にならない
                if (type == Hai.eType.Zihai) { return null; }

                int idx1 = _hais.FindIndex(a => a.Type == type && a.Number == number + 1);
                int idx2 = _hais.FindIndex(a => a.Type == type && a.Number == number + 2);
                if (idx1 >= 0 && idx2 >= 0)
                {
                    CheckTehai tmp = new CheckTehai(this);
                    tmp._shuntsu.Add(new Shuntsu(tmp._hais[0], tmp._hais[idx1], tmp._hais[idx2]));
                    tmp._hais.Remove(_hais[idx2]);
                    tmp._hais.Remove(_hais[idx1]);
                    tmp._hais.Remove(_hais[0]);
                    return tmp;
                }
            }
            return null;
        }

        // 国士無双
        public bool IsKokushimuso(List<Result> results)
        {
            if (_hais.Count >= 14)
            {
                foreach (var a in _hais)
                {
                    if (_hais.Count(e => e.Name == a.Name) == 2)
                    {
                        if (_hais.Find(e => e.Name == Hai.eName.Manzu1) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Manzu9) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Souzu1) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Souzu9) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Pinzu1) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Pinzu9) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Ton) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Nan) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Sha) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Pei) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Haku) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Hatu) != null &&
                            _hais.Find(e => e.Name == Hai.eName.Thun) != null)
                        {
                            // 国士無双十三面待ち(当たり牌が２枚）
                            if (_hais.Count(e => e.Name == _atariHai.Name) == 2)
                            {
                                _yakuMask |= Kokushijusammen.Mask;
                            }
                            // 国士無双
                            else
                            {
                                _yakuMask |= Kokushimuso.Mask;
                            }

                            // 天和、地和、人和
                            _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
                            results.Add(new Result(0, _yakuMask, _menzen, _isOya));
                            _yakuMask = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsChurempoto(List<Result> results)
        {
            //門前
            if (_menzen)
            {
                // 清一色チェック
                if (HaiInfo.IsChiniso(_state))
                {
                    // 純正九蓮宝燈
                    if (_hais.Count(e => e.Number == Hai.eNumber.Num1 && _atariHai != e) >= 3 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num2 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num3 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num4 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num5 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num6 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num7 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num8 && _atariHai != e) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num9 && _atariHai != e) >= 3)
                    {
                        _yakuMask |= Junseichuren.Mask;

                        // 天和、地和、人和
                        _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
                        results.Add(new Result(0, _yakuMask, _menzen, _isOya));
                        _yakuMask = 0;
                        return true;
                    }
                    // 九蓮宝燈
                    else if (_hais.Count(e => e.Number == Hai.eNumber.Num1) >= 3 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num2) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num3) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num4) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num5) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num6) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num7) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num8) != 0 &&
                        _hais.Count(e => e.Number == Hai.eNumber.Num9) >= 3)
                    {
                        _yakuMask |= Churempoto.Mask;

                        // 天和、地和、人和
                        _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
                        results.Add(new Result(0, _yakuMask, _menzen, _isOya));
                        _yakuMask = 0;

                        return true;
                    }

                }
            }

            return false;
        }

        public bool Yakumanhantei()
        {
            // 字一色
            if (HaiInfo.IsTsuiso(_state))
            {
                _yakuMask |= Tsuiso.Mask;
            }
            // 緑一色
            if (HaiInfo.IsRyuiso(_state))
            {
                _yakuMask |= Ryuiso.Mask;
            }
            // 清老頭
            if (HaiInfo.IsChinroto(_state))
            {
                _yakuMask |= Chinroto.Mask;
            }

            // 雀頭がある
            if (_toitsu.Count == 1)
            {
                // 大三元（三元牌の面子が３個かつ雀頭が三元牌ではない）
                if (_mentsus.Count(e => e.IsSangempai()) == 3 && !_toitsu[0].IsSangempai())
                {
                    _yakuMask |= Daisangen.Mask;
                }

                // 大四喜
                if (_mentsus.Count(e => e.IsFuampai()) == 4 && !_toitsu[0].IsFuampai())
                {
                    _yakuMask |= Daisushi.Mask;
                }
                // 小四喜
                else if (_mentsus.Count(e => e.IsFuampai()) == 4)
                {
                    _yakuMask |= Shosushi.Mask;
                }

                // 四槓子（槓子が４個）
                if (_kans.Count == 4)
                {
                    _yakuMask |= Sukantsu.Mask;
                }

                // 暗刻＋暗槓が４個
                if (_kotsu.Count(e => e.IsMenzen()) + _kans.Count(e => e.IsMenzen()) == 4)
                {
                    // 四暗刻単騎（単騎待ち）
                    if (_machi == eMachi.Tanki)
                    {
                        _yakuMask |= Suankotanki.Mask;
                    }
                    // 四暗刻（双碰待ち＋ツモ）
                    else if (_machi == eMachi.Shampon && !_ronAgari)
                    {
                        _yakuMask |= Suanko.Mask;
                    }
                }

            }

            if (_yakuMask != 0)
            {
                // 天和、地和、人和
                _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
            }

            return _yakuMask != 0;
        }

        public bool IsChitoitsu(List<Result> results)
        {
            // 一発
            _yakuMask |= (_undecidedMask & Ippatsu.Mask);
            //ツモ
            _yakuMask |= (_undecidedMask & Tsumo.Mask);

            // 鳴いてはいけない
            if (_hais.Count < 14) { return false; }

            // 偶数のみ、６番目の対子までを判定
            for (int i = 0; i < 12; i += 2)
            {
                // 隣の牌と同じでない
                if (_hais[i].Name != _hais[i + 1].Name) { return false; }

                // 隣の隣の牌と同じ（同じ対子があってはいけない）
                if (_hais[i].Name == _hais[i + 2].Name) { return false; }
            }

            // ７番目の対子の判定
            if (_hais[12].Name == _hais[13].Name)
            {
                // 字一色
                if (HaiInfo.IsTsuiso(_state))
                {
                    _yakuMask |= Tsuiso.Mask;
                    results.Add(new Result(0, _yakuMask, _menzen, _isOya));
                    _yakuMask = 0;
                    return true;
                }

                _yakuMask |= Chitoitsu.Mask;

                // 混老頭
                if (HaiInfo.IsHonroto(_state))
                {
                    _yakuMask |= Honroto.Mask;
                }

                // 混一色
                if (HaiInfo.IsHoniso(_state))
                {
                    _yakuMask |= Honiso.Mask;
                }

                // 清一色
                if (HaiInfo.IsChiniso(_state))
                {
                    _yakuMask |= Chiniso.Mask;
                }

                // 天和、地和、人和
                _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
                results.Add(new Result(25, _yakuMask, _menzen, _isOya));
                _yakuMask = 0;
                return true;
            }

            return false;
        }

        public void Yakuhantei(List<Result> results)
        {
            init();

            // 待ち毎に役を判定する
            foreach (var mentsu in _mentsus)
            {
                _machi = mentsu.Machi(_atariHai);
                if (_machi != eMachi.None)
                {
                    _yakuMask = 0;
                    _fu = 20;
                    if (_machi == eMachi.Shampon && _ronAgari)
                    {
                        mentsu.IsMenzen(false);
                    }

                    yakuhantei();
                    //メンツ
                    foreach (var mentsu2 in _mentsus)
                    {
                        _fu += mentsu2.Fu(_undecidedMask);

                    }
                    //ロン上がり
                    if (_ronAgari)
                    {
                        _fu += 10;
                    }
                    //ツモ
                    else
                    {
                        _fu += 2;
                    }
                    //頭
                    if (_toitsu[0].IsSangempai())
                    {
                        _fu += 2;
                    }
                    //待ち
                    if (_machi == eMachi.Kanchan || _machi == eMachi.Penchan || _machi == eMachi.Tanki)
                    {
                        _fu += 2;
                    }

                    _fu = (_fu + 9) / 10 * 10;

                    results.Add(new Result(_fu, _yakuMask, _menzen, _isOya));


                    mentsu.IsMenzen(true);

                }
            }
        }

        private void yakuhantei()
        {
            if (Yakumanhantei())
            {
                return;
            }
            // 一発
            _yakuMask |= (_undecidedMask & Ippatsu.Mask);

            // タンヤオ
            if (HaiInfo.IsTanyao(_state))
            {
                _yakuMask |= Tanyao.Mask;
            }

            // 混一色
            if (HaiInfo.IsHoniso(_state))
            {
                _yakuMask |= Honiso.Mask;
            }
            // 清一色
            if (HaiInfo.IsChiniso(_state))
            {
                _yakuMask |= Chiniso.Mask;
            }
            // 対々和
            if (_kotsu.Count == 4)
            {
                _yakuMask |= Toitoi.Mask;
            }
            // 小三元
            if (_mentsus.Count(e => e.IsSangempai()) == 3)
            {
                _yakuMask |= Shosangen.Mask;
            }
            else
            {
                // 白
                if (_mentsus.Count(e => e.IsHaku()) != 0 && !_toitsu[0].IsHaku())
                {
                    _yakuMask |= Yakuhai_Haku.Mask;
                }
                // 發
                if (_mentsus.Count(e => e.IsHatu()) != 0 && !_toitsu[0].IsHatu())
                {
                    _yakuMask |= Yakuhai_Hatu.Mask;
                }
                // 中
                if (_mentsus.Count(e => e.IsThun()) != 0 && !_toitsu[0].IsThun())
                {
                    _yakuMask |= Yakuhai_Thun.Mask;
                }
            }
            // 混老頭
            if (HaiInfo.IsHonroto(_state))
            {
                _yakuMask |= Honroto.Mask;
            }
            else
            {
                // チャンタ
                bool yaochu = true;
                _mentsus.ForEach(e => { yaochu &= e.IsYaochu(); });

                if (yaochu)
                {
                    if (HaiInfo.IsTsuhai(_state))
                    {
                        _yakuMask |= Chanta.Mask;
                    }
                    // 純チャン
                    else
                    {
                        _yakuMask |= Junchan.Mask;
                    }
                }

            }

            (uint manzu, uint pinzu, uint souzu) shuntsumask = (0, 0, 0);
            foreach (var mentsu in _mentsus)
            {
                var m = mentsu.ShuntsuMask();
                shuntsumask.manzu |= m.manzu;
                shuntsumask.pinzu |= m.pinzu;
                shuntsumask.souzu |= m.souzu;
            }

            // 一気通貫
            const int num147 = (1 << (int)eNumber.Num1) | (1 << (int)eNumber.Num4) | (1 << (int)eNumber.Num7);
            if ((shuntsumask.manzu & num147) == num147 ||
                (shuntsumask.pinzu & num147) == num147 ||
                (shuntsumask.souzu & num147) == num147)
            {
                _yakuMask |= Ikkitsukan.Mask;
            }
            // 三色同順
            if ((shuntsumask.manzu & shuntsumask.pinzu & shuntsumask.souzu) != 0)
            {
                _yakuMask |= Sanshokudojun.Mask;
            }

            // 三色同刻
            (uint manzu, uint pinzu, uint souzu) kotsumask = (0, 0, 0);
            foreach (var mentsu in _mentsus)
            {
                var m = mentsu.KotsuMask();
                kotsumask.manzu |= m.manzu;
                kotsumask.pinzu |= m.pinzu;
                kotsumask.souzu |= m.souzu;
            }
            if ((kotsumask.manzu & kotsumask.pinzu & kotsumask.souzu) != 0)
            {
                _yakuMask |= Sanshokudoko.Mask;
            }
            // 三槓子
            if (_kans.Count == 3)
            {
                _yakuMask |= Sankantsu.Mask;
            }

            // 三暗刻
            if (_kotsu.Count(e => e.IsMenzen()) + _kans.Count(e => e.IsMenzen()) == 3)
            {
                if (!(_machi == eMachi.Shampon && _ronAgari))
                {
                    _yakuMask |= Sananko.Mask;
                }
            }
            // 風牌：東南西北
            bool is_pinfu = true;
            Action<ulong, eState> TonNanShaPei = (yakuTarget, state) =>
            {
                // _undecidedMaskから対象の役をyakuFuampaiに取り出す
                ulong yakuFuampai = _undecidedMask & yakuTarget;
                if (yakuFuampai != 0)
                {
                    // 風牌の面子があれば
                    if (_mentsus.Count(e => e.IsAll(state)) != 0)
                    {
                        // 平和は成立しない
                        is_pinfu = false;

                        // 風牌の面子が対子でないならば役が成立
                        if (!_toitsu[0].IsAll(state))
                        {
                            _yakuMask |= yakuFuampai;
                        }
                    }
                }
            };
            TonNanShaPei(DabuTon.Mask | Yakuhai_Ton.Mask, eState.Ton);  // 東
            TonNanShaPei(DabuNan.Mask | Yakuhai_Nan.Mask, eState.Nan);  // 南
            TonNanShaPei(DabuSha.Mask | Yakuhai_Sha.Mask, eState.Sha);  // 西
            TonNanShaPei(DabuPei.Mask | Yakuhai_Pei.Mask, eState.Pei);  // 北

            if (_menzen)
            {
                //リーチ
                _yakuMask |= (_undecidedMask & Reach.Mask);
                //ダブルリーチ
                _yakuMask |= (_undecidedMask & DabuRe.Mask);
                // 二盃口
                if (Shuntsu.IsRyampeiko(_shuntsu))
                {
                    _yakuMask |= Ryampeiko.Mask;
                }
                // 一盃口
                else if (Shuntsu.IsIpeiko(_shuntsu))
                {
                    _yakuMask |= Ipeiko.Mask;
                }
                //ツモ
                _yakuMask |= (_undecidedMask & Tsumo.Mask);

                // 平和
                if (_toitsu.Count == 1 && _shuntsu.Count == 4
                    && !_toitsu[0].IsSangempai() && is_pinfu && _machi == eMachi.Ryammen)
                {
                    _yakuMask |= Pinfu.Mask;
                }
            }

            if (_yakuMask != 0)
            {
                // 天和、地和、人和
                _yakuMask |= _undecidedMask & (Yaku.Tenho.Mask | Yaku.Chiho.Mask | Yaku.Renho.Mask);
            }
        }

        public string[] YakuString()
        {
            List<string> result = new List<string>();

            foreach (var yaku in sYakuTables)
            {
                if ((yaku.Mask & _yakuMask) != 0)
                {
                    result.Add(yaku.Name);
                }
            }
            return result.ToArray();
        }

    }
}
