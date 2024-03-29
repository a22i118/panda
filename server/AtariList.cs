﻿using System;
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
        //private Result result = null;
        private List<CheckTehai> checktehais = new List<CheckTehai>();

        public AtariList(Tehai tehai, ulong yakuMask)
        {
            this.checkTehai = new CheckTehai(tehai, yakuMask);
            check();
        }

        public AtariList(Tehai tehai, ulong yakuMask, Hai hai)
        {
            this.checkTehai = new CheckTehai(tehai, yakuMask, hai);
            check();
        }

        public bool IsAtari()
        {
            return checktehais.Count > 0;
        }

        private void check()
        {
            if (checkTehai.IsKokushimuso())
            {
                checktehais.Add(checkTehai);
            }
            else if (checkTehai.IsChurempoto())
            {
                checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu())
                {
                    checktehais.Add(checkTehai);
                }

                checkRecursive(this.checkTehai, false);
            }
        }


        private void checkRecursive(CheckTehai checkTehai, bool isToitsu)
        {
            {
                CheckTehai tmp = checkTehai.AddToitsu(isToitsu);

                if (tmp != null)
                {
                    checkRecursive(tmp, true);
                }
            }
            {
                CheckTehai tmp = checkTehai.AddKotsu();

                if (tmp != null)
                {
                    checkRecursive(tmp, isToitsu);
                }
            }
            {
                CheckTehai tmp = checkTehai.AddShuntsu();

                if (tmp != null)
                {
                    checkRecursive(tmp, isToitsu);
                }
            }

            if (checkTehai.IsAgari())
            {
                checkTehai.Yakuhantei();
                checktehais.Add(checkTehai);
            }
        }

        public string[] YakuString()
        {
            List<string> result = new List<string>();

            foreach (var item in checktehais)
            {
                result.AddRange(item.YakuString());
            }

            return result.ToArray();
        }
    }
}
