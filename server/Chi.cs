using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Chi : INaki
    {
        private Hai[] _hais = new Hai[3];

        public Chi(Hai hai0, Hai hai1, Hai hai2, int from)
        {
            _from = from;
            hai0.ResetNakikouho();
            hai1.ResetNakikouho();
            hai2.ResetNakikouho();
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

            foreach (var hai in _hais)
            {
                eState state = hai.State;
                _state.all &= state;
                _state.any |= state;
            }
        }

        public override (uint manzu, uint pinzu, uint souzu) ShuntsuMask()
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

        public override int Draw(Graphics g, int x, int y)
        {
            _hais[0].SetPos(x, y);
            x += _hais[0].Draw(g, _from == 3);

            _hais[1].SetPos(x, y);
            x += _hais[1].Draw(g, _from == 2);

            _hais[2].SetPos(x, y);
            x += _hais[2].Draw(g, _from == 1);

            return x;
        }
    }
}
