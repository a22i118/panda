using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Shuntsu : IMentsu
    {
        private Hai[] _hais = new Hai[3];

        public Shuntsu(Hai hai0, Hai hai1, Hai hai2)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

            foreach (var hai in _hais)
            {
                eState state = Hai.sHaiStates[(int)hai.Name].State;
                _state_and &= state;
                _state_or |= state;
            }
        }
    }
}
