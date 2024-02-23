using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Pon : INaki
    {
        private Hai[] _hais = new Hai[3];

        public Pon(Hai hai0, Hai hai1, Hai hai2)
        {
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

        public bool IsCanKaKan(Hai hai)
        {
            return hai.Name == _hais[0].Name;
        }

        public Kan KaKan(Hai hai)
        {
            return new Kan(hai, _hais[0], _hais[1], _hais[2]);
        }
    }
}
