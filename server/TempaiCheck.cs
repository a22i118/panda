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
        Hai _tmp = null;

        public TempaiCheck(Tehai tehai, bool isoya, ulong yakumask)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < 3)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        _tmp = new Hai((Hai.eType)i, (Hai.eNumber)j);
                        checkTehai = new CheckTehai(tehai, isoya, yakumask, _tmp);
                        check();
                    }
                }
                else
                {
                    for (int j = 0; j < 7; j++)
                    {
                        _tmp = new Hai((Hai.eType)i, (Hai.eNumber)j);
                        checkTehai = new CheckTehai(tehai, isoya, yakumask, _tmp);
                        check();
                    }
                }
            }

            init();
        }
        public void Draw(Graphics g, int players)
        {
            int x = 300 - 48;

            for (int i = 0; i < _atariHais.Count; i++)
            {
                _atariHais[i].SetPos(x += 48, players * 200 - 50);
                _atariHais[i].Draw(g);
            }
        }
        private void init()
        {
            _atariHais.Clear();
        }

        private void check()
        {
            if (checkTehai.IsKokushimuso(_results))
            {
                _atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
            else if (checkTehai.IsChurempoto(_results))
            {
                _atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
            else
            {
                if (checkTehai.IsChitoitsu(_results))
                {
                    _atariHais.Add(_tmp);
                    //checktehais.Add(checkTehai);
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
                _atariHais.Add(_tmp);
                //checktehais.Add(checkTehai);
            }
        }


    }
}
