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
        List<Hai> list_ = new List<Hai>();
        public List<Hai> List { get { return list_; } }
        public Tehai(){ }
            
        
        public void Add(Hai hai)
        {
            list_.Add(hai);
        }

        public void Sort() {
            list_.Sort((a,b) => (int)a.Name- (int)b.Name);
        }
        
        public void Draw(Graphics g,int players)
        {
            for (int i = 0; i < list_.Count; i++)
            {
                list_[i].SetPos(300 + i * 48, players * 200);
                list_[i].Draw(g);
            }
        }

        public void Click(int x,int y) 
        {
            List<Hai> del = new List<Hai>();


            foreach (Hai hai in list_)
            {
                if (hai.IsClick(x, y))
                {
                    del.Add(hai);
                }
            }


            foreach (Hai hai in del)
            {
                
                list_.Remove(hai);
            }
        }
    }
}
