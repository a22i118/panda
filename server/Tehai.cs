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
        public Hai Click(int x, int y, Kawa kawas)
        {
            Hai del = null;

            for (int i = 0; i < list_.Count; i++)
            {
                if (list_[i].IsClick(x, y))
                {

                    kawas.Add(list_[i]);
                    
                    del = list_[i];
                    break;

                }
            }

            if(del != null)
            {
                list_.Remove(del);

            }

            return del;
            //foreach (Hai hai in del)
            //{
            //    list_.Remove(hai);
            //}
        }
        public void Tsumo(Yama yama)
        {
            if (list_.Count == 13)
            {
                list_.Add(yama.List[0]);
                yama.List.RemoveAt(0);
            }
        }

        //public void Ron()
        //{
        //    List<Hai> thro = new List<Hai>();
        //}

        public void MinKan(Hai del)
        {
            if (IsKotsu)
            {

            }
        }
    }
}
