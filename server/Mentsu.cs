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
        protected (eState and, eState or) _state = (eState.All, 0);

        public eState StateAnd { get { return _state.and; } }
        public eState StateOr { get { return _state.or; } }

        // 待ちを返す
        // abstract : 基底クラスに実装を持たせないで継承先でoverrideする場合に付ける修飾子
        public abstract eMachi Machi(Hai hai);

        // 面前
        // virtual : 基底クラスに実装を持たせて継承先でoverrideする場合に付ける修飾子
        public virtual bool IsMenzen() { return true; }

        // すべて三元牌（対子、刻子）
        public bool IsSangempai() { return Hai.HaiState.IsAnd(_state, eState.Haku | eState.Hatu | eState.Thun); }

        public bool IsHaku() { return Hai.HaiState.IsAnd(_state, eState.Haku); }    // 白
        public bool IsHatu() { return Hai.HaiState.IsAnd(_state, eState.Hatu); }    // 発
        public bool IsThun() { return Hai.HaiState.IsAnd(_state, eState.Thun); }    // 中

        // すべて風牌
        public bool IsFuampai() { return Hai.HaiState.IsAnd(_state, eState.Fuampai); }

        // 1つでも幺九牌（１、９、字牌）がある
        public bool IsYaochu() { return Hai.HaiState.IsOr(_state, eState.Yaochu); }
    }
}
