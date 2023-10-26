using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Yama
    {
        List<Hai> list = new List<Hai>();
        public List<Hai> List { get { return list; } }
        public Yama() {
            //list.Add(new Hai(Hai.Type.Pinzu,Hai.Number.Num1));

            for(int k=0; k<4; k++) {
                for (int i = 0; i < 4; i++)
                {
                    if (i < 3)
                    {
                        for (int j = 0; j < 9; j++)
                        {

                            list.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));

                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {

                            list.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));

                        }
                    }

                }
            }
            // シャッフルする
            Shuffle();
        }

        /// <summary>
        /// シャッフルする関数
        /// </summary>
        public void Shuffle ()
        {
            for(int i=list.Count -1;i>0;i--) {
                var rand = new Random(); 
                var x = rand.Next(0,i + 1);
                var a = list[i];
                list[i] = list[x];
                list[x] = a;
            }
        }
    }
}
