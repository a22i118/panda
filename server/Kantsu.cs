using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kantsu : INaki
    {
        private Hai[] _hais = new Hai[4];

        public Kantsu(Hai hai0, Hai hai1, Hai hai2, Hai hai3)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;
            _hais[3] = hai3;
        }

        public int Draw(Graphics g, int x, int y)
        {
            foreach (var item in _hais)
            {
                item.SetPos(x += 48, y);
                item.Draw(g);
            }
            return x;
        }
    }
}
