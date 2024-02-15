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
        const int players = 4;
        List<Hai> _list = new List<Hai>();
        public List<Hai> List { get { return _list; } }
        private List<Pon> pon = new List<Pon>();
        //private List<Hai> hais;
        public Tehai() { }

        public void Add(Hai hai)
        {
            _list.Add(hai);
        }

        public void Sort()
        {
            _list.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public void Draw(Graphics g, int players)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].SetPos(300 + i * 48, players * 200);
                _list[i].Draw(g);
            }

            

        }
        public Hai Click(int x, int y, Kawa kawas)
        {
            Hai del = null;

            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].IsClick(x, y))
                {

                    kawas.Add(_list[i]);

                    del = _list[i];
                    break;

                }
            }

            if (del != null)
            {
                _list.Remove(del);

            }

            return del;
            //foreach (Hai hai in del)
            //{
            //    list_.Remove(hai);
            //}
        }

        public void Tsumo(Yama yama)
        {
            if (IsCanTsumo())
            {
                _list.Add(yama.Tsumo());
            }
        }

        //public void Ron()
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
            return _list.Count + 3 * pon.Count < 14;
        }

        public bool IsCanPon(Hai hai)
        {
            return _list.Count(item => item.Name == hai.Name) >= 2;
        }

        public void Pon(Hai sutehai)
        {
            Hai sutehai1 = null;
            Hai sutehai2 = null;
            int i;
            for (i = 0; i < _list.Count; i++)
            {
                if (sutehai.Name == _list[i].Name)
                {
                    sutehai1 = _list[i];
                    i++;
                    break;
                }
            }

            for (; i < _list.Count; i++)
            {
                if (sutehai.Name == _list[i].Name)
                {
                    sutehai2 = _list[i]; break;
                }
            }

            if (sutehai1 != null && sutehai2 != null)
            {
                pon.Add(new server.Pon(sutehai, sutehai1, sutehai2));
                _list.Remove(sutehai1);
                _list.Remove(sutehai2);
            }

            //if(list_.Count(item => item == del) >= 2)
            //{
            //    //int Poncnt = 0;
            //    for (int i = 0; i < list_.Count; ++i)
            //    {
            //        //Add(new Pon(hais[0], hais[1], hais[2]));
            //        //if (list_[i] == del && Poncnt <= 1)
            //        //{
            //        //    list_.RemoveAt(i);
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
