using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

namespace server
{
    internal class Kotsu : IMentsu
    {
        private Hai[] _hais = new Hai[3];

        public Kotsu(Hai hai0, Hai hai1, Hai hai2)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

            eState state = Hai.sHaiStates[(int)_hais[0].Name].State;
            _state_and &= state;
            _state_or |= state;
        }

        public override eMachi Machi(Hai hai) { return hai.Name == _hais[0].Name ? eMachi.Shampon : eMachi.None; }
    }
}
