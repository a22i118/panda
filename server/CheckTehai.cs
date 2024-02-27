﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
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

        //private bool isKokushimuso = false;
        public bool IsAgari()
        {
            return _hais.Count == 0 || _yakuMask != 0;
        }

        public CheckTehai(Tehai tehai)
        {
            this._toitsu = new List<Toitsu>();
            this._kotsu = new List<Kotsu>();
            this._shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this._hais = new List<Hai>(tehai.Hais);
            this._hais.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public CheckTehai(Tehai tehai, Hai hai)
        {
            this._toitsu = new List<Toitsu>();
            this._kotsu = new List<Kotsu>();
            this._shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this._hais = new List<Hai>(tehai.Hais);
            this._hais.Add(hai);
            this._hais.Sort((a, b) => (int)a.Name - (int)b.Name);
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
            //if(_hais.Find(e => e.Name == Hai.eName.Souzu2) != null ||
            //    _hais.Find(e => e.Name == Hai.eName.Souzu3 != null ||
            //    _hais.Find(e => e.Name == Hai.eName.Souzu4 != null ||
            //    _hais.Find(e => e.Name == Hai.eName.Souzu6 != null ||
            //    _hais.Find(e => e.Name == Hai.eName.Souzu8 != null ||
            //    _hais.Find(e => e.Name == Hai.eName.Hatu
            //    ){

            //}



        }

        public bool IsChitoitsu()
        {
            if (_hais.Count >= 14)
            {
                if (_toitsu.Count == 7)
                {
                    _yakuMask |= Chitoitsu.Mask;
                    return true;
                }
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
