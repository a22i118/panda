using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kan : INaki
    {
        private Hai[] _hais = new Hai[4];

        public Kan(Hai hai0, Hai hai1, Hai hai2, Hai hai3, int from)
        {
            _from = from;
            hai0.ResetNakikouho();
            hai1.ResetNakikouho();
            hai2.ResetNakikouho();
            hai3.ResetNakikouho();
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;
            _hais[3] = hai3;
        }

        public override int Draw(Graphics g, int x, int y)
        {
            _hais[0].SetPos(x, y);
            x += _hais[0].Draw(g, _from == 3, _from == 0);

            _hais[1].SetPos(x, y);
            x += _hais[1].Draw(g, _from == 2);

            _hais[2].SetPos(x, y);
            x += _hais[2].Draw(g);

            _hais[3].SetPos(x, y);
            x += _hais[3].Draw(g, _from == 1, _from == 0);

            return x;
        }
    }
}
