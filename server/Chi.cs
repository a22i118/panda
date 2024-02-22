using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class Chi : INaki
    {
        private Hai[] _hais = new Hai[3];

        //public Hai[] Hais { get { return _hais; } }

        public Chi(Hai hai0, Hai hai1, Hai hai2)
        {
            hai0.ResetNakikouho();
            hai1.ResetNakikouho();
            hai2.ResetNakikouho();
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;
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
