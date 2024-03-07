using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace server
{
    internal class Yaku
    {
        public enum eMachi
        {
            Tanki,
            Penchan,
            Kanchan,
            Ryammen,
            Shampon,

            None
        }

        //麻雀のすべて：http://mjall.jp/
        //[Flags]
        public enum eYaku
        {
            Tsumo,          // 門前自摸(メンゼンツモ) 1 飜- 門前役
                            // ポン・チーなど、鳴かずにツモあがると成立する役です。

            Reach,          // 立直(リーチ) 1 飜- 門前役
                            // テンパイしている時に「リーチ」と宣言することで成立する役です。

            Ippatsu,        // 一発(イッパツ) 1 飜- 門前役
                            // リーチ後、鳴きの無い１巡以内にあがると成立する役です。
                            // 次の自分のツモを含めて１巡です。

            Tanyao,         // タンヤオ 1 飜- 鳴き１飜
                            // ２～８の牌のみで揃えると成立する役です。

            Pinfu,          // 平和(ピンフ) 1 飜- 門前役
                            // 下記を満たしている時に成立する役です。
                            // ・４面子が順子（連番で揃えた面子）で構成されている。
                            // ・頭は役牌（三元牌、場風牌、自風牌）以外。
                            // ・和了牌の待ち方は両面待ち。

            Ipeiko,         // 一盃口(イーペイコー) 1 飜- 門前役
                            // 同じ順子（連番で揃えた面子）が２つあると成立する役です。

            Yakuhai_Haku,   // 役牌(三元牌)／白發中 1 飜- 鳴き１飜
            Yakuhai_Hatu,   // 三元牌（白、發、中）のいずれかが３枚以上揃うと成立する役です。
            Yakuhai_Thun,

            Yakuhai_Ton,    // 役牌／東南西北 1 飜- 鳴き１飜
            Yakuhai_Nan,    // 場の風（東場は東）、自分の風（親から反時計回りに東、南、
            Yakuhai_Sha,    // 西、北）のいずれかが３枚以上揃うと成立する役です。
            Yakuhai_Pei,

            Chankan,        // 槍槓(チャンカン) 1 飜- 鳴き１飜
                            // 他家が加槓（ポンしている牌にもう１枚加えて槓）した牌であがると成立する役です。
                            // 他に役がなくても槍槓のみであがれます。

            Rinshankaiho,   // 嶺上開花(リンシャンカイホウ） 1 飜- 鳴き１飜
                            // カンをした際にツモる嶺上牌であがると成立する役です。
                            // 他に役がなくても嶺上開花のみであがれます。

            Haiteiraoyue,   // 海底撈月(ハイテイラオユエ） 1 飜- 鳴き１飜
                            // 局の最後の牌（海底牌）でツモあがると成立する役です。

            Hoteiraoyui,    // 河底撈魚(ホウテイラオユイ) 1 飜- 鳴き１飜
                            // 局の最後の打牌（河底牌）であがると成立する役です。

            Daburi,         // ダブリー 2 飜- 門前役
                            // 鳴きのない１巡目でテンパイしている時に「リーチ」と宣言することで成立する役です。
                            // ダブル立直（リーチ）の略称です。

            Rempuhai,       // 連風牌(レンプウハイ） 2 飜- 鳴き２飜
                            // 場の風（東場は東）と、自分の風（親から反時計回りに東、南、西、北）が同じ時に、３枚以上揃うと成立する役です。

            Chitoitsu,      // 七対子(チートイツ) 2 飜- 門前役
                            // 対子（同じ牌が２つある形）が７組あると成立する役です。

            Toitoi,         // 対々和(トイトイ） 2 飜- 鳴き２飜
                            // ４面子を刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）で揃えると成立する役です。

            Sananko,        // 三暗刻(サンアンコウ） 2 飜- 鳴き２飜
                            // 暗刻（手牌で同じ牌３つを揃えた面子）や暗槓の槓子（手牌で同じ牌４つを揃え槓した面子）を３つ揃えると成立する役です。

            Sanshokudoko,   // 三色同刻(サンショクドウコウ） 2 飜- 鳴き２飜
                            // 萬子、筒子、索子の同じ数字で刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）を揃えると成立する役です。

            Sanshokudojun,  // 三色同順(サンショクドウジュン） 2 飜- 鳴き１飜
                            // 萬子、筒子、索子で同じ順子（連番で揃えた面子）を揃えると成立する役です。

            Honroto,        // 混老頭(ホンロウトウ） 2 飜- 鳴き２飜
                            // 字牌と、１・９牌（両方もしくは片方）で揃えると成立する役です。

            Ikkitsukan,     // 一気通貫(イッキツウカン） 2 飜- 鳴き１飜
                            // １つの種類で、１２３、４５６、７８９の順子（連番で揃えた面子）を揃えると成立する役です。

            Chanta,         // チャンタ 2 飜- 鳴き１飜
                            // 面子と頭すべて、字牌と、１・９牌（両方もしくは片方）を含めて揃えると成立する役です。

            Shosangen,      // 小三元(ショウサンゲン） 2 飜- 鳴き２飜
                            // 白・發・中のいずれか１つを頭、他の２つを刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）で揃えると成立する役です。

            Sankantsu,      // 三槓子(サンカンツ） 2 飜- 鳴き２飜
                            // 槓子（同じ牌４つを揃え槓した面子）を３つ揃えると成立する役です。

            Honiso,         // 混一色(ホンイーソー） 3 飜- 鳴き２飜
                            // 萬子、筒子、索子の１種類と、字牌で揃えると成立する役です。

            Junchan,        // 純チャン(ジュンチャン） 3 飜- 鳴き２飜
                            // 面子と頭すべて、１・９牌（両方もしくは片方）を含めて揃えると成立する役です。

            Ryampeiko,      // 二盃口(リャンペイコー） 3 飜- 門前役
                            // 一盃口 （同じ順子を２つ）を２組み揃えると成立する役です。

            Nagashimangan,  // 流し満貫(ナガシマンガン） 5 飜- 鳴き 5 飜
                            // 自分の捨牌がすべて１・９・字牌で流局すると成立する役です。
                            // 捨牌を他家に鳴かれると成立しません。

            Chiniso,        // 清一色(チンイーソー） 6 飜- 鳴き 5 飜
                            // 萬子、筒子、索子の１種類で揃えると成立する役です。


            Tenho,          // 天和(テンホー） 役満- 門前役
                            // 親が配牌であがっていると成立する役です。

            Chiho,          // 地和(チーホー） 役満- 門前役
                            // 鳴きのない１巡目に、子がツモであがると成立する役です。

            Renho,          // 人和(レンホー） 役満- 門前役
                            // 鳴きのない１巡目で、自分のツモ番までに他家からあがると成立する役です。
            Ryuiso,         // 緑一色(リューイーソー） 役満- 鳴き可
                            // 發と、索子の２・３・４・６・８（すべて使わなくてもよい）で揃えると成立する役です。
                            // 發を含めなければ成立しないルールもあります。

            Daisangen,      // 大三元(ダイサンゲン） 役満- 鳴き可
                            // 白・發・中すべてを刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）で揃えると成立する役です。

            Suanko,         // 四暗刻(スーアンコウ） 役満- 門前役
                            // 暗刻（手牌で同じ牌３つを揃えた面子）や暗槓の槓子（手牌で同じ牌４つを揃え槓した面子）を４つ揃えると成立する役です。

            Chinroto,       // 清老頭(チンロウトウ） 役満- 鳴き可
                            // １・９牌だけで揃えると成立する役です。

            Kokushimuso,    // 国士無双(コクシムソウ） 役満- 門前役
                            // １・９・字牌の頭と、１・９・字牌を全種類揃えると成立する役です。

            Churempoto,     // 九蓮宝燈(チューレンポートウ） 役満- 門前役
                            // 萬子、筒子、索子の１種類で、１・９を３つ、２～８を１つに１～９のいずれか１つを加えた形で揃えると成立する役です。

            Tsuiso,         // 字一色(ツーイーソー） 役満- 鳴き可
                            // 字牌で揃えると成立する役です。

            Shosushi,       // 小四喜(ショウスーシー） 役満- 鳴き可
                            // 東・南・西・北のいずれか１つを頭、他の３つを刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）で揃えると成立する役です。

            Sukantsu,       // 四槓子(スーカンツ） 役満- 鳴き可
                            // 槓子（同じ牌４つを揃え槓した面子）を４つ揃えると成立する役です。

            Suankotanki,    // 四暗刻単騎(スーアンコウタンキ） ダブル役満- 門前役
                            // 暗刻（手牌で同じ牌３つを揃えた面子）や暗槓の槓子（手牌で同じ牌４つを揃え槓した面子）を４つ揃えて、単騎待ちであがると成立する役です。

            Daisushi,       // 大四喜(ダイスーシー） ダブル役満- 鳴き可
                            // 東・南・西・北を刻子（同じ牌３つで揃えた面子）や槓子（同じ牌４つを揃え槓した面子）で揃えると成立する役です。

            Junseichuren,   // 純正九蓮宝燈(ジュンセイチューレンポートウ） ダブル役満- 門前役
                            // 萬子、筒子、索子の１種類で、１・９を３つ、２～８を１つずつ揃えると成立する役です。１～９すべてが待ち牌です。

            Kokushijusammen,// 国士無双十三面待ち(コクシムソウジュウサンメンマチ） ダブル役満- 門前役
                            // １・９・字牌を全種類１つずつ揃えると成立する役です。１・９・字牌の１３種類すべてが待ち牌です。
            Num,
            None = Num,
        };

        public struct YakuTable
        {
            string _name;
            ulong _mask;
            int _han;
            int _nakihan;

            public string Name { get { return _name; } }
            public ulong Mask { get { return _mask; } }
            public int Han { get { return _han; } }
            public int NakiHan { get { return _nakihan; } }

            public YakuTable(string name, eYaku yaku, int han, int nakihan)
            {
                this._name = name;
                this._mask = 1ul << (int)yaku;
                this._han = han;
                this._nakihan = nakihan;
            }
        };

        // 門前自摸(メンゼンツモ) 1 飜- 門前役
        public static YakuTable Tsumo = new YakuTable("門前自摸", eYaku.Tsumo, 1, 0);
        // 立直(リーチ) 1 飜- 門前役
        public static YakuTable Reach = new YakuTable("立直", eYaku.Reach, 1, 0);
        // 一発(イッパツ) 1 飜- 門前役
        public static YakuTable Ippatsu = new YakuTable("一発", eYaku.Ippatsu, 1, 0);
        // タンヤオ 1 飜- 鳴き１飜
        public static YakuTable Tanyao = new YakuTable("タンヤオ", eYaku.Tanyao, 1, 1);
        // 平和(ピンフ) 1 飜- 門前役
        public static YakuTable Pinfu = new YakuTable("平和", eYaku.Pinfu, 1, 0);
        // 一盃口(イーペイコー) 1 飜- 門前役
        public static YakuTable Ipeiko = new YakuTable("一盃口", eYaku.Ipeiko, 1, 0);
        // 役牌(白) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Haku = new YakuTable("役牌：白", eYaku.Yakuhai_Haku, 1, 1);
        // 役牌(發) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Hatu = new YakuTable("役牌：發", eYaku.Yakuhai_Hatu, 1, 1);
        // 役牌(中) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Thun = new YakuTable("役牌：中", eYaku.Yakuhai_Thun, 1, 1);
        // 役牌(東) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Ton = new YakuTable("役牌：東", eYaku.Yakuhai_Ton, 1, 1);
        // 役牌(南) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Nan = new YakuTable("役牌：南", eYaku.Yakuhai_Nan, 1, 1);
        // 役牌(西) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Sha = new YakuTable("役牌：西", eYaku.Yakuhai_Sha, 1, 1);
        // 役牌(北) 1 飜- 鳴き１飜
        public static YakuTable Yakuhai_Pei = new YakuTable("役牌：北", eYaku.Yakuhai_Pei, 1, 1);
        // 槍槓(チャンカン) 1 飜- 鳴き１飜
        public static YakuTable Chankan = new YakuTable("槍槓", eYaku.Chankan, 1, 1);
        // 嶺上開花(リンシャンカイホウ） 1 飜- 鳴き１飜
        public static YakuTable Rinshankaiho = new YakuTable("嶺上開花", eYaku.Rinshankaiho, 1, 1);
        // 海底撈月(ハイテイラオユエ） 1 飜- 鳴き１飜
        public static YakuTable Haiteiraoyue = new YakuTable("海底撈月", eYaku.Haiteiraoyue, 1, 1);
        // 河底撈魚(ホウテイラオユイ) 1 飜- 鳴き１飜
        public static YakuTable Hoteiraoyui = new YakuTable("河底撈魚", eYaku.Hoteiraoyui, 1, 1);
        // ダブリー 2 飜- 門前役
        public static YakuTable Daburi = new YakuTable("ダブリー", eYaku.Daburi, 2, 0);
        // 連風牌(レンプウハイ） 2 飜- 鳴き２飜
        public static YakuTable Rempuhai = new YakuTable("連風牌", eYaku.Rempuhai, 2, 2);
        // 七対子(チートイツ) 2 飜- 門前役
        public static YakuTable Chitoitsu = new YakuTable("七対子", eYaku.Chitoitsu, 2, 0);
        // 対々和(トイトイ） 2 飜- 鳴き２飜
        public static YakuTable Toitoi = new YakuTable("対々和", eYaku.Toitoi, 2, 2);
        // 三暗刻(サンアンコウ） 2 飜- 鳴き２飜
        public static YakuTable Sananko = new YakuTable("三暗刻", eYaku.Sananko, 2, 2);
        // 三色同刻(サンショクドウコウ） 2 飜- 鳴き２飜
        public static YakuTable Sanshokudoko = new YakuTable("三色同刻", eYaku.Sanshokudoko, 2, 2);
        // 三色同順(サンショクドウジュン） 2 飜- 鳴き１飜
        public static YakuTable Sanshokudojun = new YakuTable("三色同順", eYaku.Sanshokudojun, 2, 1);
        // 混老頭(ホンロウトウ） 2 飜- 鳴き２飜
        public static YakuTable Honroto = new YakuTable("混老頭", eYaku.Honroto, 2, 2);
        // 一気通貫(イッキツウカン） 2 飜- 鳴き１飜
        public static YakuTable Ikkitsukan = new YakuTable("一気通貫", eYaku.Ikkitsukan, 2, 1);
        // チャンタ 2 飜- 鳴き１飜
        public static YakuTable Chanta = new YakuTable("チャンタ", eYaku.Chanta, 2, 1);
        // 小三元(ショウサンゲン） 2 飜- 鳴き２飜
        public static YakuTable Shosangen = new YakuTable("小三元", eYaku.Shosangen, 2, 2);
        // 三槓子(サンカンツ） 2 飜- 鳴き２飜
        public static YakuTable Sankantsu = new YakuTable("三槓子", eYaku.Sankantsu, 2, 2);
        // 混一色(ホンイーソー） 3 飜- 鳴き２飜
        public static YakuTable Honiso = new YakuTable("混一色", eYaku.Honiso, 3, 2);
        // 純チャン(ジュンチャン） 3 飜- 鳴き２飜
        public static YakuTable Junchan = new YakuTable("純チャン", eYaku.Junchan, 3, 2);
        // 二盃口(リャンペイコー） 3 飜- 門前役
        public static YakuTable Ryampeiko = new YakuTable("二盃口", eYaku.Ryampeiko, 3, 0);
        // 流し満貫(ナガシマンガン） 5 飜- 鳴き 5 飜
        public static YakuTable Nagashimangan = new YakuTable("流し満貫", eYaku.Nagashimangan, 5, 5);
        // 清一色(チンイーソー） 6 飜- 鳴き 5 飜
        public static YakuTable Chiniso = new YakuTable("清一色", eYaku.Chiniso, 6, 5);
        // 天和(テンホー） 役満- 門前役
        public static YakuTable Tenho = new YakuTable("天和", eYaku.Tenho, 13, 0);
        // 地和(チーホー） 役満- 門前役
        public static YakuTable Chiho = new YakuTable("地和", eYaku.Chiho, 13, 0);
        // 人和(レンホー） 役満- 門前役
        public static YakuTable Renho = new YakuTable("人和", eYaku.Renho, 13, 0);
        // 緑一色(リューイーソー） 役満- 鳴き可
        public static YakuTable Ryuiso = new YakuTable("緑一色", eYaku.Ryuiso, 13, 13);
        // 大三元(ダイサンゲン） 役満- 鳴き可
        public static YakuTable Daisangen = new YakuTable("大三元", eYaku.Daisangen, 13, 13);
        // 四暗刻(スーアンコウ） 役満- 門前役
        public static YakuTable Suanko = new YakuTable("四暗刻", eYaku.Suanko, 13, 0);
        // 清老頭(チンロウトウ） 役満- 鳴き可
        public static YakuTable Chinroto = new YakuTable("清老頭", eYaku.Chinroto, 13, 13);
        // 国士無双(コクシムソウ） 役満- 門前役
        public static YakuTable Kokushimuso = new YakuTable("国士無双", eYaku.Kokushimuso, 13, 0);
        // 九蓮宝燈(チューレンポートウ） 役満- 門前役
        public static YakuTable Churempoto = new YakuTable("九蓮宝燈", eYaku.Churempoto, 13, 0);
        // 字一色(ツーイーソー） 役満- 鳴き可
        public static YakuTable Tsuiso = new YakuTable("字一色", eYaku.Tsuiso, 13, 13);
        // 小四喜(ショウスーシー） 役満- 鳴き可
        public static YakuTable Shosushi = new YakuTable("小四喜", eYaku.Shosushi, 13, 13);
        // 四槓子(スーカンツ） 役満- 鳴き可
        public static YakuTable Sukantsu = new YakuTable("四槓子", eYaku.Sukantsu, 13, 13);
        // 四暗刻単騎(スーアンコウタンキ） ダブル役満- 門前役
        public static YakuTable Suankotanki = new YakuTable("四暗刻単騎", eYaku.Suankotanki, 26, 0);
        // 大四喜(ダイスーシー） ダブル役満- 鳴き可
        public static YakuTable Daisushi = new YakuTable("大四喜", eYaku.Daisushi, 26, 26);
        // 純正九蓮宝燈(ジュンセイチューレンポートウ） ダブル役満- 門前役
        public static YakuTable Junseichuren = new YakuTable("純正九蓮宝燈", eYaku.Junseichuren, 26, 0);
        // 国士無双十三面待ち(コクシムソウジュウサンメンマチ） ダブル役満- 門前役
        public static YakuTable Kokushijusammen = new YakuTable("国士無双十三面待ち", eYaku.Kokushijusammen, 26, 0);

        public static YakuTable[] sYakuTables = {
            Tsumo,
            Reach,
            Ippatsu,
            Tanyao,
            Pinfu,
            Ipeiko,
            Yakuhai_Haku,
            Yakuhai_Hatu,
            Yakuhai_Thun,
            Yakuhai_Ton,
            Yakuhai_Nan,
            Yakuhai_Sha,
            Yakuhai_Pei,
            Chankan,
            Rinshankaiho,
            Haiteiraoyue,
            Hoteiraoyui,
            Daburi,
            Rempuhai,
            Chitoitsu,
            Toitoi,
            Sananko,
            Sanshokudoko,
            Sanshokudojun,
            Honroto,
            Ikkitsukan,
            Chanta,
            Shosangen,
            Sankantsu,
            Honiso,
            Junchan,
            Ryampeiko,
            Nagashimangan,
            Chiniso,
            Tenho,
            Chiho,
            Renho,
            Ryuiso,
            Daisangen,
            Suanko,
            Chinroto,
            Kokushimuso,
            Churempoto,
            Tsuiso,
            Shosushi,
            Sukantsu,
            Suankotanki,
            Daisushi,
            Junseichuren,
            Kokushijusammen,
        };
    }
}
