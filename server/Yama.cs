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
        public Yama() {
            //list.Add(new Hai(Hai.Type.Pinzu,Hai.Number.Num1));

            for(int k=0; k<4; k++) {
                for (int i = 0; i < 4; i++)
                {
                    if (i < 3)
                    {
                        for (int j = 0; j < 9; j++)
                        {

                            list.Add(new Hai((Hai.Type)i, (Hai.Number)j));

                        }
                    }
                    else
                    {
                        for (int j = 9; j < 16; j++)
                        {

                            list.Add(new Hai((Hai.Type)i, (Hai.Number)j));

                        }
                    }

                }
            }
            Shuffle();
            
        }


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
