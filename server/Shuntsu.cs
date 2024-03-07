using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

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

        public override eMachi Machi(Hai hai)
        {
            if (hai.Name == _hais[1].Name) { return eMachi.Kanchan; }
            else if (hai.Name == _hais[0].Name) { return hai.Number == eNumber.Num7 ? eMachi.Penchan : eMachi.Ryammen; }
            else if (hai.Name == _hais[2].Name) { return hai.Number == eNumber.Num3 ? eMachi.Penchan : eMachi.Ryammen; }
            return eMachi.None;
        }

        static public bool IsIpeiko(List<Shuntsu> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i]._hais[0].Name == list[j]._hais[0].Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool IsRyampeiko(List<Shuntsu> list)
        {
            int i = 0;
            for (; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i]._hais[0].Name == list[j]._hais[0].Name)
                    {
                        i++;
                        goto label;
                    }
                }
            }
        label:
            for (; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i]._hais[0].Name == list[j]._hais[0].Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
