using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace server
{
    internal class AtariList
    {
        private List<Toitsu> toitsu = new List<Toitsu>();
        private List<Kotsu> kotsu = new List<Kotsu>();
        //private List<Hai> hais;
        private CheckTehai checkTehai;
        private List<CheckTehai> checktehais = new List<CheckTehai>();
        
        public AtariList(Tehai tehai) {
            this.checkTehai = new CheckTehai(tehai);
            check(this.checkTehai, false);

        }

    public bool IsAtari()
        {
            return checktehais.Count > 0;
        }

        private void check(CheckTehai checkTehai, bool isToitsu)
        {

            {
                CheckTehai tmp = checkTehai.AddToitsu(isToitsu);

                if (tmp != null)
                {
                    check(tmp, true);
                }
            }

            {
                CheckTehai tmp = checkTehai.AddKotsu();

                if (tmp != null)
                {
                    check(tmp, isToitsu);
                }
            }

            {
                CheckTehai tmp = checkTehai.AddShuntsu();

                if (tmp != null)
                {
                    check(tmp, isToitsu);
                }
            }
            

            if (checkTehai.IsAgari())
            {
                checktehais.Add(checkTehai);
            }
            
            

            
        }




#if false
            if (!isToitsu && hais.Count>=2&&   hais[0].Name == hais[1].Name){
                toitsu.Add(new Toitsu(hais[0], hais[1]));

                //List<Hai> tmp=new List<Hai>(hais);
                CheckTehai tmp = new CheckTehai (checkTehai);
                tmp.Remove(hais[0]);
                tmp.Remove(hais[1]);

                check(tmp,true);
            }
#endif
#if false
            if(hais.Count>=3 && hais[0].Name == hais[1].Name && hais[0].Name == hais[2].Name)
            {
                kotsus.Add(new Kotsu(hais[0], hais[1], hais[2]));
                //List<Hai> tmp = new List<Hai>(hais);
                CheckTehai tmp = new CheckTehai (checkTehai);
                tmp.Remove(hais[0]);
                tmp.Remove(hais[1]);
                tmp.Remove(hais[2]);

                check(tmp,isToitsu);
            }
#endif
#if false
            if (hais.Count >= 3)
            {
                Hai.eType type = hais[0].Type;
                Hai.eNumber number = hais[0].Number;

                int idx1 = hais.FindIndex(a => a.Type == type && a.Number == number + 1);
                int idx2 = hais.FindIndex(a => a.Type == type && a.Number == number + 2);
                if(idx1 >=0 &&  idx2 >=0)
                {
                    //List<Hai> tmp = new List<Hai>(hais);
                    CheckTehai tmp = new CheckTehai (checkTehai);
                    tmp.Remove(hais[idx2]);
                    tmp.Remove(hais[idx1]);
                    tmp.Remove(hais[0]);

                    check(tmp,isToitsu);
                }
            }
#endif


        
    }
}
