using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class Tehai
    {
        //const int players = 4;
        private List<Hai> _hais = new List<Hai>();
        private List<Pon> _pons = new List<Pon>();

        public List<Hai> List { get { return _hais; } }
        //private List<Hai> hais;

        public Tehai() { }

        public void Init()
        {
            _hais.Clear();
            _pons.Clear();
        }

        public void Add(Hai hai)
        {
            _hais.Add(hai);
        }

        public void Sort()
        {
            _hais.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public void Draw(Graphics g, int players)
        {
            for (int i = 0; i < _hais.Count; i++)
            {
                _hais[i].SetPos(300 + i * 48, players * 200);
                _hais[i].Draw(g);
            }
        }

        public Hai Click(int x, int y, Kawa kawas)
        {
            Hai del = null;

            for (int i = 0; i < _hais.Count; i++)
            {
                if (_hais[i].IsClick(x, y))
                {

                    kawas.Add(_hais[i]);

                    del = _hais[i];
                    break;

                }
            }

            if (del != null)
            {
                _hais.Remove(del);

            }

            return del;
            //foreach (Hai hai in del)
            //{
            //    _hais.Remove(hai);
            //}
        }

        public void Tsumo(Yama yama)
        {
            if (IsCanTsumo())
            {
                _hais.Add(yama.Tsumo());
            }
        }

        //public void _ron()
        //{
        //    List<Hai> thro = new List<Hai>();
        //}

        //public void MinKan(Hai del)
        //{
        //    if (IsKotsu)
        //    {

        //    }
        //}

        public bool IsCanTsumo()
        {
            return _hais.Count + 3 * _pons.Count < 14;
        }

        public bool IsCanPon(Hai hai)
        {
            return _hais.Count(item => item.Name == hai.Name) >= 2;
        }

        public void Pon(Hai sutehai)
        {
            Hai sutehai1 = null;
            Hai sutehai2 = null;
            int i;
            for (i = 0; i < _hais.Count; i++)
            {
                if (sutehai.Name == _hais[i].Name)
                {
                    sutehai1 = _hais[i];
                    i++;
                    break;
                }
            }

            for (; i < _hais.Count; i++)
            {
                if (sutehai.Name == _hais[i].Name)
                {
                    sutehai2 = _hais[i]; break;
                }
            }

            if (sutehai1 != null && sutehai2 != null)
            {
                _pons.Add(new server.Pon(sutehai, sutehai1, sutehai2));
                _hais.Remove(sutehai1);
                _hais.Remove(sutehai2);
            }

            //if(_hais.Count(item => item == del) >= 2)
            //{
            //    //int Poncnt = 0;
            //    for (int i = 0; i < _hais.Count; ++i)
            //    {
            //        //Add(new Pon(hais[0], hais[1], hais[2]));
            //        //if (_hais[i] == del && Poncnt <= 1)
            //        //{
            //        //    _hais.RemoveAt(i);
            //        //    Poncnt++;
            //        //}
            //    }

            //    //Pon(del,del,del);
            //}

            //if (hais.Count >= 3 && hais[0].Name == hais[1].Name && hais[0].Name == hais[2].Name)
            //{
            //    CheckTehai tmp = new CheckTehai(this);

            //    tmp.kotsu.Add(new Kotsu(tmp.hais[0], tmp.hais[1], tmp.hais[2]));
            //    tmp.hais.RemoveAt(2);
            //    tmp.hais.RemoveAt(1);
            //    tmp.hais.RemoveAt(0);

            //}


        }
    }
}
