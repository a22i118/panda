using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Pon : INaki
    {
        private Hai[] _hais = new Hai[3];
        public override int Fu { get { return IsYaochu() ? 4 : 2; } }

        public Pon(Hai hai0, Hai hai1, Hai hai2, int from)
        {
            _from = from;
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;

            eState state = Hai.sHaiStates[(int)_hais[0].Name].State;
            _state.all &= state;
            _state.any |= state;
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

        public bool IsCanKaKan(Hai hai)
        {
            return hai.Name == _hais[0].Name;
        }

        public Kan KaKan(Hai hai)
        {
            return new Kan(hai, _hais[0], _hais[1], _hais[2], _from);
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
