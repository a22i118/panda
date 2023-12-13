using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class GameManager
    {
        const int players = 4;

        Yama yama = new Yama();
        Tehai[] tehais = new Tehai[players];
        Kawa[] kawas = new Kawa[players];

        public GameManager()
        { 
            Init();
           
        }

        public void Init()
        {
            for (int i = 0; i < players; i++)
            {
                tehais[i] = new Tehai();

            }

            //四人に配る
            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
            }

            //手牌のソート
            for (int i = 0; i < players; i++)
            {
                tehais[i].Sort();
            }
        }
        public void Exec()
        {

        }
        public void ClickCheck(int x,int y)
        {
            for (int i = 0; i < players; i++)
            {
                tehais[i].Click(x,y);
                //kawas[i].Add(del.List);
#if false
                if (tehais[].Count == 13)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
#endif                
            }
        }


        
        public void Draw(Graphics g)
        {
            for (int i = 0;i < players;i++)
            {
                tehais[i].Draw(g, i);
                //kawas[i].Draw(g,i);
            }
        }
    }
}
