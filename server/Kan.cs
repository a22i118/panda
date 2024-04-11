using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Kan : INaki
    {
        private Hai[] _hais = new Hai[4];

        public override int Fu(ulong undecidedMask) { return IsYaochu() ? (_from == 0 ? 32 : 16) : (_from == 0 ? 16 : 8); }
        public Kan(Hai hai0, Hai hai1, Hai hai2, Hai hai3, int from)
        {
            _from = from;
            hai0.ResetNakikouho();
            hai1.ResetNakikouho();
            hai2.ResetNakikouho();
            hai3.ResetNakikouho();
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;
            _hais[3] = hai3;

            eState state = Hai.sHaiStates[(int)_hais[0].Name].State;
            _state.all &= state;
            _state.any |= state;
        }

        public override int Draw(Graphics g, int x, int y)
        {
            _hais[0].SetPos(x, y);
            x += _hais[0].Draw(g, _from == 3, _from == 0);

            _hais[1].SetPos(x, y);
            x += _hais[1].Draw(g, _from == 2);

            _hais[2].SetPos(x, y);
            x += _hais[2].Draw(g);

            _hais[3].SetPos(x, y);
            x += _hais[3].Draw(g, _from == 1, _from == 0);

            return x;
        }
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
