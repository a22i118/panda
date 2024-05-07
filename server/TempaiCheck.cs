using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class TempaiCheck
    {
        public CheckTehai checkTehai = null;
        private List<Result> _results = new List<Result>();
        private List<Hai> _atariHais = new List<Hai>();
        public List<Hai> AtariHais { get { return _atariHais; } }
        public TempaiCheck(Tehai tehai, bool isoya, ulong yakumask)
        {
            if (tehai.Hais.Count == 13)
            {
                for (int i = 0; i < (int)Hai.eName.Num; i++)
                {
                    Hai tmp = new Hai((Hai.eName)i);
                    checkTehai = new CheckTehai(tehai, isoya, yakumask, tmp);
                    if (check())
                    {
                        _atariHais.Add(tmp);
                    }
                }
                init();
            }
        }
        public void Draw(Graphics g, int players)
        {
            int x = 300 - 48;

            for (int i = 0; i < _atariHais.Count; i++)
            {
                _atariHais[i].SetPos(x += 48, players * 200 + 50);
                _atariHais[i].Draw(g);
            }
        }
        private void init()
        {
            _atariHais.Clear();
            _results.Clear();
        }

        private bool check()
        {
            if (checkTehai.IsKokushimuso(_results))
            {
                return true;
                //_atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
            else if (checkTehai.IsChurempoto(_results))
            {
                return true;
                //_atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu(_results))
                {
                    return true;
                    //checktehais.Add(checkTehai);
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
                    //checkRecursive(tmp, true);
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
                //checkTehai.Yakuhantei(_results);
                //_atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
            return false;
        }


    }
}
