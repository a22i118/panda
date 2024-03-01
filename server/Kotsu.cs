using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kotsu
    {
        private Hai[] _hais = new Hai[3];
        public Kotsu(Hai hai0, Hai hai1, Hai hai2)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

        }
        public bool IsSangempai()
        {
            return Hai.HaiState.IsSangenpai(_hais[0].State);
        }
    }

}
