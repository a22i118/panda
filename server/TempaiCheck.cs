using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class TempaiCheck
    {
        public CheckTehai? checkTehai = null;
        private List<Result> _results = new List<Result>();
        private List<Hai> _atariHais = new List<Hai>();
        public List<Hai> AtariHais { get { return _atariHais; } }
        public TempaiCheck(Tehai tehai, bool isOya, ulong yakumask)
        {
            Init();
            if (tehai.Hais.Count + (tehai.Chis.Count + tehai.Pons.Count + tehai.Kans.Count) * 3 == 13)
            {
                foreach (var haiState in Hai.sHaiStates)
                {
                    Hai tmp = new Hai(haiState);
                    checkTehai = new CheckTehai(tehai, isOya, yakumask, null, tmp);
                    if (check())
                    {
                        _atariHais.Add(tmp);
                    }
                }
            }
        }
        public void Draw(Graphics g, int players)
        {
            int x = 300 - 48;

            for (int i = 0; i < _atariHais.Count; i++)
            {
                _atariHais[i].SetPos(x += 48, players * 200 + 110 + 50);
                _atariHais[i].Draw(g);
            }
        }
        public void Init()
        {
            _atariHais.Clear();
            _results.Clear();
        }

        private bool check()
        {
            if (checkTehai.IsKokushimuso(_results))
            {
                return true;
            }
            else if (checkTehai.IsChurempoto(_results))
            {
                return true;
            }
            else
            {
                if (checkTehai.IsChitoitsu(_results))
                {
                    return true;
                }

                return checkRecursive(this.checkTehai, false);
            }
        }
        private bool checkRecursive(CheckTehai checkTehai, bool isToitsu)
        {
            {
                CheckTehai tmp = checkTehai.AddToitsu(isToitsu);

                if (tmp != null && checkRecursive(tmp, true))
                {
                    return true;
                }
            }
            {
                CheckTehai tmp = checkTehai.AddKotsu();

                if (tmp != null && checkRecursive(tmp, isToitsu))
                {
                    return true;
                }
            }
            {
                CheckTehai tmp = checkTehai.AddShuntsu();

                if (tmp != null && checkRecursive(tmp, isToitsu))
                {
                    return true;
                }
            }

            if (checkTehai.IsAgari())
            {
                return true;
            }
            return false;
        }


    }
}
