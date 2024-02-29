using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

namespace server
{
    internal class CheckTehai
    {
        private List<Toitsu> _toitsu = new List<Toitsu>();
        private List<Kotsu> _kotsu = new List<Kotsu>();
        private List<Shuntsu> _shuntsu = new List<Shuntsu>();

        private List<Chi> _chis;
        private List<Pon> _pons;
        private List<Kan> _kans;
        private List<Hai> _hais;
        private ulong _yakuMask = 0;

        private eState _state_and = 0;
        private eState _state_or = 0;

        //private bool isKokushimuso = false;
        public bool IsAgari()
        {
            return _hais.Count == 0 || _yakuMask != 0;
        }

        public CheckTehai(Tehai tehai, Hai add = null)
        {
            this._toitsu = new List<Toitsu>();
            this._kotsu = new List<Kotsu>();
            this._shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this._hais = new List<Hai>(tehai.Hais);

            if (add != null)
            {
                this._hais.Add(add);
            }
            this._hais.Sort((a, b) => (int)a.Name - (int)b.Name);

            foreach (var hai in _hais)
            {
                eState state = Hai.sHaiStates[(int)hai.Name].State;
                _state_and &= state;
                _state_or |= state;
            }

            foreach (var chi in _chis)
            {
                _state_and &= chi.StateAnd;
                _state_or |= chi.StateOr;
            }

            foreach (var pon in _pons)
            {
                _state_and &= pon.StateAnd;
                _state_or |= pon.StateOr;
            }

            foreach (var kan in _kans)
            {
                _state_and &= kan.StateAnd;
                _state_or |= kan.StateOr;
            }
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

        public bool IsKokushimuso()//国士無双
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
                            //isKokushimuso = true;
                            _yakuMask |= Kokushimuso.Mask;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Yakumanhantei()
        {
            // 字一色
            if (HaiState.IsTsuiso(_state_or))
            {
                _yakuMask |= Tsuiso.Mask;
            }


        }

        public bool IsChitoitsu()
        {
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
                if (HaiState.IsTsuiso(_state_or))
                {
                    _yakuMask |= Tsuiso.Mask;
                    return false;
                }

                _yakuMask |= Chitoitsu.Mask;

                // 混老頭
                if (HaiState.IsHonroto(_state_or))
                {
                    _yakuMask |= Honroto.Mask;
                    return false;
                }

                // 混一色
                if (HaiState.IsHoniso(_state_or))
                {
                    _yakuMask |= Honiso.Mask;
                    return false;
                }

                // 清一色
                if (HaiState.IsChiniso(_state_or))
                {
                    _yakuMask |= Chiniso.Mask;
                    return false;
                }


                return true;
            }

            return false;
        }

        public void Yakuhantei()
        {
            if (_toitsu.Count == 1 && _shuntsu.Count == 4)
            {
                _yakuMask |= Pinfu.Mask;
            }

            //if (jihaisikanai)
            //{
            //    yakuMask |= Ipeiko.Mask;
            //}

            //int han = 0;

            //foreach (var item in sYakuTables)
            //{
            //    if ((yakuMask & item.Mask) != 0)
            //    {
            //        han += item.Han;
            //    }
            //}




            //CheckTehai tmp = checkTehai;
            //int hu = 0;
            //int han = 0;
            //int ten = 0;

            //for (int i = 0; i < hais.Count; i++)
            //{
            //    if (tmp.hais.etype ==)
            //    {

            //    }
            //}
        }
    }
}
