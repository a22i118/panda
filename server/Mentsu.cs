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

        public virtual int Fu { get { return 0; }  }

        // 待ちを返す
        // abstract : 基底クラスに実装を持たせないで継承先でoverrideする場合に付ける修飾子
        public abstract eMachi Machi(Hai hai);

        // 面前
        // virtual : 基底クラスに実装を持たせて継承先でoverrideする場合に付ける修飾子
        public virtual bool IsMenzen() { return true; }

        public bool IsAll(eState mask) { return Hai.HaiState.IsAll(_state, mask); }

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
        public bool IsYaochu() { return Hai.HaiState.IsOr(_state, eState.Yaochu); }

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
//　符計算：https://qr.paps.jp/5eZbd

//　基本符　　　      　　　                     20　符

//　上がり方　　  　                        ロン 10
//　　　　　　　  　                        ツモ  2

//　各メンツ                　　　中張牌　　明刻　2
//　                                        暗刻  4
//　                                        明槓  8
//　                                        暗槓 16
//　                              幺九牌    明刻　4
//　                                        暗刻  8
//　                                        明槓 16
//　                                        暗槓 32

//　頭              役牌（三元牌、場風牌、自風牌）2
//　　　　　　　　　　　　　　　　　　　　連風牌　4

//　待ち　カンチャン、ペンチャン、単騎、ノベタン　2

//　例外
//　平和、ツモ　　　　　　　　　　　　一律20符
//　七対子　　　　　　　　　　　　　　一律25符
//　喰いタン　（タンヤオのみ）両面待ち　　30符
//　喰いピン　（三色同順のみ）両面待ち　　30符
