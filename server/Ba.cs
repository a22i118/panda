using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Ba
    {
        public enum eKaze
        {
            Ton,
            Nan,
            Sha,
            Pei,

            Num
        }
        private eKaze _baKaze = eKaze.Ton;
        private int _oyaPlayer = 0;

        public Ba() { }


        public eKaze BaKaze { get { return _baKaze; } }
        public eKaze ZiKaze(int player)
        {
            if ((player - _oyaPlayer + (int)eKaze.Num) % (int)eKaze.Num == 0) { return eKaze.Ton; }
            else if ((player - _oyaPlayer + (int)eKaze.Num) % (int)eKaze.Num == 1) { return eKaze.Nan; }
            else if ((player - _oyaPlayer + (int)eKaze.Num) % (int)eKaze.Num == 2) { return eKaze.Sha; }
            else { return eKaze.Pei; }
        }
    }
}
