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
        //public List<Hai> List { get { return _hais; } }

        public Kawa() { }

        public void Init() { _hais.Clear(); }

        public void Add(Hai hai) { _hais.Add(hai); }

        public void Draw(Graphics g, int players)
        {
            for (int i = 0; i < _hais.Count; i++)
            {
                _hais[i].SetPos(1100 + i * 48, players * 200);
                _hais[i].Draw(g);
            }
        }
    }
}
