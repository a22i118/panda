using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

namespace server
{
    abstract class IMentsu
    {
        protected (eState all, eState any) _state = (eState.All, 0);

        public eState StateAll { get { return _state.all; } }
        public eState StateAny { get { return _state.any; } }

        public virtual int Fu(ulong undecidedMask) { return 0; }

        // 待ちを返す
        // abstract : 基底クラスに実装を持たせないで継承先でoverrideする場合に付ける修飾子
        public abstract eMachi Machi(Hai hai);

        // 面前
        // virtual : 基底クラスに実装を持たせて継承先でoverrideする場合に付ける修飾子
        public virtual bool IsMenzen() { return true; }
        public virtual void IsMenzen(bool flag) { }

        public bool IsAll(eState mask) { return Hai.HaiInfo.IsAll(_state, mask); }

        // すべて三元牌（対子、刻子）
        public bool IsSangempai() { return IsAll(eState.Haku | eState.Hatu | eState.Thun); }

        public bool IsTon() { return IsAll(eState.Ton); }      // 東
        public bool IsNan() { return IsAll(eState.Nan); }      // 南
        public bool IsSha() { return IsAll(eState.Sha); }      // 西
        public bool IsPei() { return IsAll(eState.Pei); }      // 北
        public bool IsHaku() { return IsAll(eState.Haku); }    // 白
        public bool IsHatu() { return IsAll(eState.Hatu); }    // 発
        public bool IsThun() { return IsAll(eState.Thun); }    // 中

        // すべて風牌
        public bool IsFuampai() { return IsAll(eState.Fuampai); }

        // 1つでも幺九牌（１、９、字牌）がある
        public bool IsYaochu() { return Hai.HaiInfo.IsOr(_state, eState.Yaochu); }

        public virtual (uint manzu, uint pinzu, uint souzu) ShuntsuMask()
        {
            return (0, 0, 0);
        }
        public virtual (uint manzu, uint pinzu, uint souzu) KotsuMask()
        {
            return (0, 0, 0);
        }
    }
}
