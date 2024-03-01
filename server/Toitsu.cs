using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Toitsu
    {
        private Hai[] _hais = new Hai[2];
        public Toitsu(Hai hai0, Hai hai1)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
        }
        public bool IsSangempai()
        {
            return Hai.HaiState.IsSangenpai(_hais[0].State);
        }
    }
}
