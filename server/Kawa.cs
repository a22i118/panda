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
        List<Hai> _hais = new List<Hai>();

        public Kawa() { }

        public void Init() { _hais.Clear(); }

        public void Add(Hai hai) { _hais.Add(hai); }

        public void Draw(Graphics g, int player)
        {
            int x = 1100 - 48;
            int y = player * 200 + 64;

            for (int i = 0; i < _hais.Count; i++)
            {
                if (i == 6 || i == 12)
                {
                    x = 1100 - 48;
                    y += 64;
                }
                _hais[i].SetPos(x += 48, y);
                _hais[i].Draw(g);
            }
        }
    }
}
