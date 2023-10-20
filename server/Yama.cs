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

            for(int i=0; i<4; i++)
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
                    for (int j = 0; j < 7; j++)
                    {

                        list.Add(new Hai((Hai.Type)i, (Hai.Number)j));

                    }
                }
                
            }
            
        }
    }
}
