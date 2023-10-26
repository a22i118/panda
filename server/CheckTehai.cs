using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class CheckTehai
    {
        private List<Toitsu> toitsu =new List<Toitsu>();
        private List<Koutsu> koutsus =new List<Koutsu>();
        private List<Hai> hais;
        public CheckTehai(List<Hai> hais) {
            this.hais = new List<Hai>(hais);

            check(this.hais);
        }

        private void check(List<Hai> hais)
        {

#if false
            if(hais.Count>=2&&   hais[0].Name == hais[1].Name){
                toitsu.Add(new Toitsu(hais[0], hais[1]));

                List<Hai> tmp=new List<Hai>(hais);
                tmp.Remove(hais[0]);
                tmp.Remove(hais[1]);

                check(tmp);
            }
            if(hais.Count>=3 && hais[0].Name == hais[1].Name && hais[0].Name == hais[2].Name)
            {
                koutsus.Add(new Koutsu(hais[0], hais[1], hais[2]));
                List<Hai> tmp = new List<Hai>(hais);
                tmp.Remove(hais[0]);
                tmp.Remove(hais[1]);
                tmp.Remove(hais[2]);

                check(tmp);
            }
#endif
            if (hais.Count >= 3)
            {
                Hai.eType type = hais[0].Type;
                Hai.eNumber number = hais[0].Number;

                int idx1 = hais.FindIndex(a => a.Type == type && a.Number == number + 1);
                int idx2 = hais.FindIndex(a => a.Type == type && a.Number == number + 2);
            }

        }

    }
}
