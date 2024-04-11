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
        public override int Fu(ulong undecidedMask)
        {

            Func<ulong, ulong, eName, int> TonNanShaPei = (dabuMask, yakuhaiMask, name) =>
            {
                int fu = 0;
                if (_hais[0].Name == name)
                {
                    if ((undecidedMask & dabuMask) == dabuMask)
                    {
                        fu = 4;
                    }
                    else if ((undecidedMask & yakuhaiMask) == yakuhaiMask)
                    {
                        fu = 2;
                    }
                }
                return fu;
            };

            int rt;

            if (0 != (rt = TonNanShaPei(DabuTon.Mask, Yakuhai_Ton.Mask, eName.Ton)) ||
                0 != (rt = TonNanShaPei(DabuNan.Mask, Yakuhai_Nan.Mask, eName.Nan)) ||
                0 != (rt = TonNanShaPei(DabuSha.Mask, Yakuhai_Sha.Mask, eName.Sha)) ||
                0 != (rt = TonNanShaPei(DabuPei.Mask, Yakuhai_Pei.Mask, eName.Pei)))
            {
                return rt;
            }

            return 0;
        }
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
