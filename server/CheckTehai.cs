using System;
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
        private List<Toitsu> toitsu = new List<Toitsu>();
        private List<Kotsu> kotsu = new List<Kotsu>();
        private List<Shuntsu> shuntsu = new List<Shuntsu>();

        private List<Chi> _chis;
        private List<Pon> _pons;
        private List<Kan> _kans;
        private List<Hai> hais;

        public bool IsAgari()
        {
            return hais.Count == 0;
        }

        public CheckTehai(Tehai tehai)
        {
            this.toitsu = new List<Toitsu>();
            this.kotsu = new List<Kotsu>();
            this.shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this.hais = new List<Hai>(tehai.Hais);
            this.hais.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public CheckTehai(Tehai tehai, Hai hai)
        {
            this.toitsu = new List<Toitsu>();
            this.kotsu = new List<Kotsu>();
            this.shuntsu = new List<Shuntsu>();

            this._chis = new List<Chi>(tehai.Chis);
            this._pons = new List<Pon>(tehai.Pons);
            this._kans = new List<Kan>(tehai.Kans);
            this.hais = new List<Hai>(tehai.Hais);
            this.hais.Add(hai);
            this.hais.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public CheckTehai(CheckTehai checkTehai)
        {
            this.toitsu = new List<Toitsu>(checkTehai.toitsu);
            this.kotsu = new List<Kotsu>(checkTehai.kotsu);
            this.shuntsu = new List<Shuntsu>(checkTehai.shuntsu);

            this._chis = new List<Chi>(checkTehai._chis);
            this._pons = new List<Pon>(checkTehai._pons);
            this._kans = new List<Kan>(checkTehai._kans);
            this.hais = new List<Hai>(checkTehai.hais);
        }

        public CheckTehai AddToitsu(bool isToitsu)
        {
            if (!isToitsu && hais.Count >= 2 && hais[0].Name == hais[1].Name)
            {
                CheckTehai tmp = new CheckTehai(this);
                tmp.toitsu.Add(new Toitsu(tmp.hais[0], tmp.hais[1]));
                tmp.hais.RemoveAt(1);
                tmp.hais.RemoveAt(0);
                return tmp;
            }
            return null;
        }

        public CheckTehai AddKotsu()
        {
            if (hais.Count >= 3 && hais[0].Name == hais[1].Name && hais[0].Name == hais[2].Name)
            {
                CheckTehai tmp = new CheckTehai(this);
                tmp.kotsu.Add(new Kotsu(tmp.hais[0], tmp.hais[1], tmp.hais[2]));
                tmp.hais.RemoveAt(2);
                tmp.hais.RemoveAt(1);
                tmp.hais.RemoveAt(0);
                return tmp;
            }
            return null;
        }

        public CheckTehai AddShuntsu()
        {
            if (hais.Count >= 3)
            {
                Hai.eType type = hais[0].Type;
                Hai.eNumber number = hais[0].Number;

                // 字牌は順子にならない
                if (type == Hai.eType.Zihai) { return null; }

                int idx1 = hais.FindIndex(a => a.Type == type && a.Number == number + 1);
                int idx2 = hais.FindIndex(a => a.Type == type && a.Number == number + 2);
                if (idx1 >= 0 && idx2 >= 0)
                {
                    CheckTehai tmp = new CheckTehai(this);
                    tmp.shuntsu.Add(new Shuntsu(tmp.hais[0], tmp.hais[idx1], tmp.hais[idx2]));
                    tmp.hais.Remove(hais[idx2]);
                    tmp.hais.Remove(hais[idx1]);
                    tmp.hais.Remove(hais[0]);
                    return tmp;
                }
            }
            return null;
        }

        public void Yakuhantei()
        {
            uint yakuMask = 0;

            if (toitsu.Count == 1 && shuntsu.Count == 4)
            {
                yakuMask |= Pinfu.Mask;
            }
            CheckTehai tmp = checkTehai;
            int hu = 0;
            int han = 0;
            int ten = 0;

            for (int i = 0; i < hais.Count; i++)
            {
                if (tmp.hais.etype ==)
                {

                }
            }
        }
    }
}
