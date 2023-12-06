using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static reversi.Reversi;

namespace server
{
    internal class GamaManager
    {
        const int players = 4;

        Yama yama = new Yama();
        Tehai[] tehais = new Tehai[players];

        public GamaManager()
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

    }
}
