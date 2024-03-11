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
        //static public bool IsSanshokudoko(List<Kotsu> list)
        //{
        //    for (int i = 0; i < list.Count - 1; i++)
        //    {
        //        for (int j = i + 1; j < list.Count; j++)
        //        {
        //            if (list[i]._hais[0].Number == list[j]._hais[0].Number &&
        //                list[i]._hais[0].Type != list[j]._hais[0].Type)
        //            {
        //                for (int k = j + 1; k < list.Count; k++)
        //                {
        //                    if (list[j]._hais[0].Number == list[k]._hais[0].Number &&
        //                        list[i]._hais[0].Type != list[k]._hais[0].Type &&
        //                        list[j]._hais[0].Type != list[k]._hais[0].Type)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
