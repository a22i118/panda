using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Toitsu : IMentsu
    {
        private Hai[] _hais = new Hai[2];

        public Toitsu(Hai hai0, Hai hai1)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;

            eState state = Hai.sHaiStates[(int)_hais[0].Name].State;
            _state_and &= state;
            _state_or |= state;
        }

        public override bool IsSangempai() { return Hai.HaiState.IsSangenpai(_state_and); }
    }
}
