using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace server
{
    internal class AtariList
    {
        private CheckTehai checkTehai = null;
        private List<CheckTehai> checktehais = new List<CheckTehai>();

        public AtariList(Tehai tehai)
        {
            this.checkTehai = new CheckTehai(tehai);
            if (checkTehai.IsKokushimuso())
            {
                checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu())
                {
                    checktehais.Add(checkTehai);
                }

                check(this.checkTehai, false);
            }
        }

        public AtariList(Tehai tehai, Hai hai)
        {
            this.checkTehai = new CheckTehai(tehai, hai);
            if (checkTehai.IsKokushimuso())
            {
                checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu())
                {
                    checktehais.Add(checkTehai);
                }

                check(this.checkTehai, false);
            }
        }

        public bool IsAtari()
        {
            return checktehais.Count > 0;
        }

        private void check(CheckTehai checkTehai, bool isToitsu)
        {
            {
                CheckTehai tmp = checkTehai.AddToitsu(isToitsu);

                if (tmp != null)
                {
                    check(tmp, true);
                }
            }
            {
                CheckTehai tmp = checkTehai.AddKotsu();

                if (tmp != null)
                {
                    check(tmp, isToitsu);
                }
            }
            {
                CheckTehai tmp = checkTehai.AddShuntsu();

                if (tmp != null)
                {
                    check(tmp, isToitsu);
                }
            }


            if (checkTehai.IsAgari())
            {
                checktehais.Add(checkTehai);
            }
        }
    }
}
