﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Yaku;

namespace server
{
    internal class Result
    {
        private int _fu = 0;
        private int _han = 0;
        private int _ten = 0;
        const int bazoro = 2;
        private ulong _yakuMask = 0;
        public int Fu { get { return _fu; } }
        public int Han { get { return _han; } }
        public int Ten { get { return _ten; } }

        public Result(int fu, ulong yakumask, bool isMenzen)
        {
            _yakuMask = yakumask;
            _fu = fu;


            foreach (var yaku in sYakuTables)
            {
                if ((yaku.Mask & _yakuMask) != 0)
                {
                    _han += isMenzen ? yaku.Han : yaku.NakiHan;
                }
            }
            TenCalc(_fu, _han);
        }
        public string[] YakuString()
        {
            List<string> result = new List<string>();

            foreach (var yaku in sYakuTables)
            {
                if ((yaku.Mask & _yakuMask) != 0)
                {
                    result.Add(yaku.Name);
                }
            }
            return result.ToArray();
        }

        public string FuString()
        {
            return _fu.ToString();
        }
        public string HanString()
        {
            return _han.ToString();
        }
        public string TenString()
        {
            return _ten.ToString();
        }
        private void TenCalc(int fu, int han)
        {
            bool oya = false;
            //親の点数 = 符 * 4 * 2の翻数乗 * 1.5
            //子の点数 = 符 * 4 * 2の翻数乗
            //下二桁は切り上げ
            int tmp;
            if (han >= 13) { tmp = 32000; }
            else if (han == 11 || han == 12) { tmp = 24000; }
            else if (han == 8 || han == 9 || han == 10) { tmp = 16000; }
            else if (han == 6 || han == 7) { tmp = 12000; }
            else if (han == 5) { tmp = 8000; }
            else
            {
                tmp = fu * 4 * (int)Math.Pow(2, han + bazoro);
                if (tmp > 7700)
                {
                    tmp = 8000;
                }
            }
            // 親
            if (oya)
            {
                tmp += tmp / 2;
            }
            _ten = (tmp + 90) / 100 * 100;
        }
    }
    //対応表：https://00m.in/gLZbi
    //親　()はツモった場合の子一人分
    //         1ハン    2ハン    3ハン    4ハン
    //20 符      -	      -        -        -
    //                  (700)   (1300)  (2600)

    //25 符	     -      2,400    4800    9600
    //                   (-)    (1600)  (3200)

    //30 符	   1500    2900    5800   11600
    //         (500)   (1000)  (2000)  (3900)

    //40 符	   2000    3900    7700   12000
    //         (700)   (1300)  (2600)  (4000)

    //50 符	   2400    4800    9600   12000    
    //         (800)   (1600)  (3200)  (4000)

    //60 符	   2900    5800   11600   12000 
    //        (1000)  (2000)  (3900)  (4000)

    //70 符	   3400    6800   12000   12000 
    //        (1200)  (2300)  (4000)  (4000)

    //80 符     3900    7700   12000   12000
    //        (1300)  (2600)  (4000)  (4000)

    //90 符     4400    8700   12000   12000
    //        (1500)  (2900)  (4000)  (4000)

    //100 符    4800    9600   12000   12000
    //        (1600)  (3200)  (4000)  (4000)

    //110 符    5300   10600   12000   12000
    //          (-)    (3600)  (4000)  (40000)

    // 5ハン    6,7ハン   8,9,10ハン  11,12ハン  13ハン以上
    //12000    18000     24000      36000      48000
    //(4000)   (6000)    (8000)    (12000)    (16000)


    //子　()はツモった場合
    //         1ハン    2ハン    3ハン    4ハン
    //20 符      -	      -        -        -
    //   子             (500)    (700)   (1300)
    //   親             (700)   (1300)  (2600)

    //25 符	     -      1600    3200    6400
    //   子              (-)     (800)   (1600)
    //   親              (-)    (1600)  (3200)

    //30 符	   1000    2000    3900    7700
    //   子    (300)    (500)   (1000)  (2000)
    //   親    (500)   (1000)  (2000)  (3900)

    //40 符	   1300    2600    5200    8000
    //   子    (400)    (700)   (1300)  (2000)
    //   親    (700)   (1300)  (2600)  (4000)

    //50 符	   1600    3200    6400    8000
    //   子    (400)    (800)   (1600)  (2000)
    //   親    (800)   (1600)  (3200)  (4000)

    //60 符	   2000    3900    7700    8000
    //   子    (500)   (1000)  (2000)  (2000)
    //   親   (1000)  (2000)  (3900)  (4000)

    //70 符	   2300    4500    8000    8000
    //   子    (600)   (1200)  (2000)  (2000)
    //   親   (1200)  (2300)  (4000)  (4000)

    //80 符    2600    5200    8000    8000
    //   子    (700)   (1300)  (2000)  (2000)
    //   親   (1300)  (2600)  (4000)  (4000)

    //90 符    2900    5800    8000    8000
    //   子    (800)   (1500)  (2000)  (2000) 
    //   親   (1500)  (2900)  (4000)  (4000)

    //100 符   3200    6400    8000    8000
    //   子    (800)   (1600)  (2000)  (2000)
    //   親   (1600)  (3200)  (4000)  (4000)

    //110 符   3600    7100    8000    8000
    //   子     (-)    (1800)  (2000)  (2000)
    //   親     (-)    (3600)  (4000)  (4000)

    //    5ハン    6,7ハン   8,9,10ハン  11,12ハン  13ハン以上
    //    8000    12000     16000      24000      32000
    //子 (2000)   (3000)    (4000)     (6000)     (8000)
    //親 (4000)   (6000)    (8000)    (12000)    (16000)
}
