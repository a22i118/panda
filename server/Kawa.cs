using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kawa
    {
        List<Hai> list_ = new List<Hai>();
        public List<Hai> List { get { return list_; } }
        public Kawa() { }

        public void Draw(Graphics g, int players)
        {
            for (int i = 0; i < list_.Count; i++)
            {
                list_[i].SetPos(1000 + i * 48, players * 200);
                list_[i].Draw(g);
            }
        }
    }
}
