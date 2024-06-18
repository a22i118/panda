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
        private bool _isMenzen = true;
        public override bool IsMenzen() { return _isMenzen; }
        public override void IsMenzen(bool flag) { _isMenzen = flag; }

        private Hai[] _hais = new Hai[3];

        public override int Fu(ulong undecidedMask) { return IsYaochu() ? (_isMenzen ? 8 : 4) : (_isMenzen ? 4 : 2); }

        public Kotsu(Hai hai0, Hai hai1, Hai hai2)
        {
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

            eState state = hai0.State;
            _state.all &= state;
            _state.any |= state;
        }
        public override eMachi Machi(Hai hai) { return hai.Name == _hais[0].Name ? eMachi.Shampon : eMachi.None; }

        public override (uint manzu, uint pinzu, uint souzu) KotsuMask()
        {
            if (_hais[0].Type == eType.Manzu)
            {
                return (1u << (int)_hais[0].Number, 0, 0);
            }
            else if (_hais[0].Type == eType.Pinzu)
            {
                return (0, 1u << (int)_hais[0].Number, 0);
            }
            else
            {
                return (0, 0, 1u << (int)_hais[0].Number);
            }
        }
    }
}
