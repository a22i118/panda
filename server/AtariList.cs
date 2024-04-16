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
        private List<Result> _results = new List<Result>();
        private List<CheckTehai> checktehais = new List<CheckTehai>();
        public List<Result> Results { get { return _results; } }
        public AtariList(Tehai tehai, ulong yakuMask)
        {
            this.checkTehai = new CheckTehai(tehai, yakuMask);
            check();
            _results.Sort((a, b) => b.Ten - a.Ten);
        }

        public AtariList(Tehai tehai, ulong yakuMask, Hai hai)
        {
            this.checkTehai = new CheckTehai(tehai, yakuMask, hai);
            check();
            _results.Sort((a, b) => b.Ten - a.Ten);
        }

        public bool IsAtari()
        {
            return checktehais.Count > 0;
        }

        private void check()
        {
            if (checkTehai.IsKokushimuso(_results))
            {
                checktehais.Add(checkTehai);
            }
            else if (checkTehai.IsChurempoto(_results))
            {
                checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu(_results))
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
                checkTehai.Yakuhantei(_results);
                checktehais.Add(checkTehai);
            }
        }

        public string[] YakuString()
        {
            List<string> result = new List<string>();

            foreach (var item in _results)
            {
                result.AddRange(item.YakuString());
            }

            return result.ToArray();

        }
        public string[] FuString()
        {
            List<string> result = new List<string>();

            foreach (var item in _results)
            {
                result.Add(item.FuString());
            }

            return result.ToArray();
        }

        public string[] HanString()
        {
            List<string> result = new List<string>();

            foreach (var item in _results)
            {
                result.Add(item.HanString());
            }

            return result.ToArray();
        }

        public string[] TenString()
        {
            List<string> result = new List<string>();

            foreach (var item in _results)
            {
                result.Add(item.TenString());
            }
            return result.ToArray();

        }
    }
}
