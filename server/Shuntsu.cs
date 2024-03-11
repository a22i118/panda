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
                _state.all &= state;
                _state.any |= state;
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
        //static public bool IsIkkitsukan(List<Shuntsu> list)
        //{
        //    for (int i = 0; i < list.Count - 1; i++)
        //    {
        //        for (int j = i + 1; j < list.Count; j++)
        //        {
        //            if (list[i]._hais[0].Type == list[j]._hais[0].Type &&
        //                list[i]._hais[0].Number == Hai.eNumber.Num1 &&
        //                list[j]._hais[0].Number == Hai.eNumber.Num4)
        //            {
        //                for (int k = j + 1; k < list.Count; k++)
        //                {
        //                    if (list[j]._hais[0].Type == list[k]._hais[0].Type &&
        //                       list[k]._hais[0].Number == Hai.eNumber.Num7)
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        //static public bool IsSanshokudojun(List<Shuntsu> list)
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
