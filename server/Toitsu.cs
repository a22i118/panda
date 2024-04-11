using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

namespace server
{
    internal class Toitsu : IMentsu
    {
        public override int Fu { get { return IsSangempai() ? 2 : 0; } }
        private Hai[] _hais = new Hai[2];
        public Toitsu(Hai hai0, Hai hai1)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;

            eState state = Hai.sHaiStates[(int)_hais[0].Name].State;
            _state.all &= state;
            _state.any |= state;
        }

        public override eMachi Machi(Hai hai) { return hai.Name == _hais[0].Name ? eMachi.Tanki : eMachi.None; }
    }
}
