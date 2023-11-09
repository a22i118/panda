using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class CheckTehai
    {
        private List<Toitsu> toitsu = new List<Toitsu>();
        private List<Kotsu> kotsu = new List<Kotsu>();
        private List<Shuntsu> shuntsu = new List<Shuntsu>();
        private List<Hai> hais;
        public CheckTehai(Tehai tehai) {
            this.hais = new List<Hai> (tehai.List);
        }

        public CheckTehai(CheckTehai checkTehai)
        {
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

        public CheckTehai AddKostu(bool isKostu)
        {
            if (hais.Count >= 3 && hais[0].Name == hais[1].Name && hais[0].Name == hais[2].Name)
            {
                CheckTehai tmp = new CheckTehai(this);

                tmp.kotsu.Add(new Kotsu(tmp.hais[0], tmp.hais[1], tmp.hais[2]));
                tmp.hais.RemoveAt(2);
                tmp.hais.RemoveAt(1);
                tmp.hais.RemoveAt(0);
                
                

            }
        }
    }
}
