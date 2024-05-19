using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kawa
    {
        private List<Hai> _hais = new List<Hai>();
        public List<Hai> Hais { get { return _hais; } }
        private bool _richiSet = false;
        public bool RichiSet { get { return _richiSet; } set { _richiSet = value; } }
        public Kawa() { }

        public void Init() { _hais.Clear(); }

        public void Add(Hai hai) { _hais.Add(hai); }

        public void Draw(Graphics g, int players)
        {
            int x = 1100 - 48;
            int y = players * 200 + 50;

            for (int i = 0; i < _hais.Count; i++)
            {
                if (i == 6 || i == 12)
                {
                    x = 1100 - 48;
                    y += 64;
                }
                if (_richiSet)
                {
                    //90度回転させる
                    _hais[i].ThrowChoice = false;
                    _hais[i].SetPos(x += 48, y);
                    _hais[i].Draw(g);
                    _richiSet = false;
                }
                else
                {
                    _hais[i].ThrowChoice = false;
                    _hais[i].SetPos(x += 48, y);
                    _hais[i].Draw(g);

                }

            }
        }
    }
}
