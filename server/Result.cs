using System;
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
        private string _str = "";
        const int bazoro = 2;
        private ulong _yakuMask = 0;
        public ulong YakuMask { get { return _yakuMask; } set { _yakuMask = value; } }


        public int Fu { get { return _fu; } }
        public int Han { get { return _han; } set { _han = value; } }
        public int Ten { get { return _ten; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fu">符</param>
        /// <param name="yakumask"></param>
        /// <param name="isMenzen"></param>
        /// <param name="isoya"></param>
        public Result(int fu, ulong yakumask, bool isMenzen, bool isoya)
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
            TenCalc(isoya);
        }
        /// <summary>
        /// 役名を配列で返す
        /// </summary>
        /// <returns></returns>
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
        private void TenCalc(bool isoya)
        {
            //親の点数 = 符 * 4 * 2の翻数乗 * 1.5
            //子の点数 = 符 * 4 * 2の翻数乗
            //下二桁は切り上げ
            int tmp;
            if (_han >= 13) { tmp = 32000; }
            else if (_han == 11 || _han == 12) { tmp = 24000; }
            else if (_han == 8 || _han == 9 || _han == 10) { tmp = 16000; }
            else if (_han == 6 || _han == 7) { tmp = 12000; }
            else if (_han == 5) { tmp = 8000; }
            else
            {
                tmp = _fu * 4 * (int)Math.Pow(2, _han + bazoro);
                if (tmp > 7700)
                {
                    tmp = 8000;
                }
            }
            // 親
            if (isoya)
            {
                tmp += tmp / 2;
                _ten = (tmp + 90) / 100 * 100;
                if (_ten == 12000)
                {
                    _str = "満貫";
                }
                else if (_ten == 18000)
                {
                    _str = "跳満";
                }
                else if (_ten == 24000)
                {
                    _str = "倍満";
                }
                else if (_ten == 32000)
                {
                    _str = "三倍満";
                }
                else if (_ten == 48000)
                {
                    _str = "数え役満";
                }
            }
            else
            {
                _ten = (tmp + 90) / 100 * 100;
                if (_ten == 8000)
                {
                    _str = "満貫";
                }
                else if (_ten == 12000)
                {
                    _str = "跳満";
                }
                else if (_ten == 16000)
                {
                    _str = "倍満";
                }
                else if (_ten == 24000)
                {
                    _str = "三倍満";
                }
                else if (_ten == 32000)
                {
                    _str = "数え役満";
                }
            }
        }

        public void Draw(Graphics g, PointF pos)
        {
            string str = "";
            string yakumanstr = "";
            int yakuman = 0;
            Font font_small = new Font(new FontFamily("HGS行書体"), 16, FontStyle.Bold);
            foreach (var yaku in sYakuTables)
            {
                if ((yaku.Mask & _yakuMask) != 0)
                {
                    str += yaku.Name;
                    str += " ";
                }
            }
            if (str.Contains("国士無双十三面待ち"))
            {
                yakuman += 2; yakumanstr += "国士無双十三面待ち ";
            }
            else if (str.Contains("国士無双"))
            {
                yakuman++; yakumanstr += "国士無双 ";
            }
            if (str.Contains("大四喜"))
            {
                yakuman += 2; yakumanstr += "大四喜 ";
            }
            if (str.Contains("小四喜"))
            {
                yakuman++; yakumanstr += "小四喜 ";
            }
            if (str.Contains("純正九蓮宝燈"))
            {
                yakuman += 2; yakumanstr += "純正九蓮宝燈 ";
            }
            else if (str.Contains("九蓮宝燈"))
            {
                yakuman++; yakumanstr += "九蓮宝燈 ";
            }
            if (str.Contains("四暗刻単騎"))
            {
                yakuman += 2; yakumanstr += "四暗刻単騎 ";
            }
            else if (str.Contains("四暗刻"))
            {
                yakuman++; yakumanstr += "四暗刻 ";
            }
            if (str.Contains("清老頭"))
            {
                yakuman++; yakumanstr += "清老頭 ";
            }
            if (str.Contains("大三元"))
            {
                yakuman++; yakumanstr += "大三元 ";
            }
            if (str.Contains("緑一色"))
            {
                yakuman++; yakumanstr += "緑一色 ";
            }
            if (str.Contains("字一色"))
            {
                yakuman++; yakumanstr += "字一色 ";
            }
            if (str.Contains("四槓子"))
            {
                yakuman++; yakumanstr += "四槓子 ";
            }
            if (str.Contains("天和"))
            {
                yakuman++; yakumanstr += "天和 ";
            }
            if (str.Contains("地和"))
            {
                yakuman++; yakumanstr += "地和 ";
            }
            if (str.Contains("人和"))
            {
                yakuman++; yakumanstr += "人和 ";
            }

            if (yakuman == 0)
            {
                str += _fu.ToString();
                str += " 符 ";
                str += _han.ToString();
                str += " 翻 ";
                str += _ten.ToString();
                str += " ";
                str += _str;
            }
            else
            {

                yakumanstr += (_ten * yakuman).ToString();
                yakumanstr += " ";
                _str = "";
                if (yakuman == 2)
                {
                    _str += "二倍";
                }
                else if (yakuman == 3)
                {
                    _str += "三倍";
                }
                else if (yakuman == 4)
                {
                    _str += "四倍";
                }
                else if (yakuman == 5)
                {
                    _str += "五倍";
                }
                else if (yakuman == 6)
                {
                    _str += "六倍";
                }

                _str += "役満";
                yakumanstr += _str;
                str = yakumanstr;
            }


            g.DrawString(str, font_small, Brushes.White, pos);
        }
    }
}
