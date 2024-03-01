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
        protected eState _state_and = eState.All;
        protected eState _state_or = 0;

        public eState StateAnd { get { return _state_and; } }
        public eState StateOr { get { return _state_or; } }

        // 待ちを返す
        // abstract : 基底クラスに実装を持たせないで継承先でoverrideする場合に付ける修飾子
        public abstract eMachi Machi(Hai hai);

        // 面前
        // virtual : 基底クラスに実装を持たせて継承先でoverrideする場合に付ける修飾子
        public virtual bool IsMenzen() { return true; }


        // すべて三元牌（対子、刻子）
        public bool IsSangempai() { return Hai.HaiState.IsSangenpai(_state_and); }

        // 1つでも幺九牌（１、９、字牌）がある
        public bool IsYaochu() { return Hai.HaiState.IsYaochu(_state_or); }
    }
}
