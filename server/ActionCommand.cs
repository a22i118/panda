using reversi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class ActionCommand
    {
        private int _x;
        private int _y;
        private int _w;
        private int _h;

        private enum eCommand : int
        {
            Chi,
            Pon,
            Kan,
            Ron,
            Tsumo,
            Richi,
            Cancel,
            Num = Cancel
        };

        private static string[] s_string = new string[] { "チー", "ポン", "カン","ロン", "ツモ", "リーチ", "キャンセル" };
        //private static string[] s_string = new string[] { "チー", "ポン", "カン", "ロン", "ツモ",  "キャンセル" };

        public ActionCommand(int x, int y, int w, int h)
        {
            _x = x;
            _y = y;
            _w = w;
            _h = h;
        }

        public void Init()
        {
            Array.Fill(_can, false);
            Array.Fill(_call, false);
        }

        private bool[] _can = new bool[(int)eCommand.Num];
        private bool[] _call = new bool[(int)eCommand.Num];

        public bool IsCanAny() { return _can.Any(value => value); }
        private void pushCancel()
        {
            for (int i = 0; i < _can.Length; i++)
            {
                _can[i] = false;
            }

        }
        public bool CanChi { set { _can[(int)eCommand.Chi] = value; } }
        public bool CanPon { set { _can[(int)eCommand.Pon] = value; } }
        public bool CanKan { set { _can[(int)eCommand.Kan] = value; } }
        public bool CanRon { set { _can[(int)eCommand.Ron] = value; } }
        public bool CanTsumo { set { _can[(int)eCommand.Tsumo] = value; } }
        public bool CanRichi { set { _can[(int)eCommand.Richi] = value; } }


        public bool IsCallChi() { return _call[(int)eCommand.Chi]; }
        public bool IsCallPon() { return _call[(int)eCommand.Pon]; }
        public bool IsCallKan() { return _call[(int)eCommand.Kan]; }
        public bool IsCallRon() { return _call[(int)eCommand.Ron]; }
        public bool IsCallTsumo() { return _call[(int)eCommand.Tsumo]; }

        public bool IsCallRichi() { return _call[(int)(eCommand.Richi)]; }
        public bool Click(int x, int y)
        {
            if (_y <= y && y <= _y + _h)
            {
                int i;
                for (i = 0; i < (int)eCommand.Num; i++)
                {
                    if (_x + _w * i <= x && x <= _x + _w * (i + 1))
                    {
                        _call[i] = _can[i];
                        return true;
                    }
                }
                {
                    if (_x + _w * i <= x && x <= _x + _w * i + _w / 2 * 5)
                    {
                        pushCancel();
                        return false;
                    }
                }
            }
            return false;
        }

        public void Draw(Graphics g, bool teban)
        {
            Font font = new Font(new FontFamily("Arial"), 16, FontStyle.Bold);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            if (teban)
            {
                g.DrawString("＞", font, Brushes.Black, new PointF(_x - 48, _y));
            }

            int i;
            for (i = 0; i < (int)eCommand.Num; i++)
            {
                Brush fontColor = _can[i] ? Brushes.Black : Brushes.Gray;
                int x = _x + _w * i;
                g.FillRectangle(whiteBrush, x + 1, _y, _w - 2, _h);
                g.DrawString(String.Format("{0, 2}", s_string[i]), font, fontColor, new PointF(x, _y));
            }

            {
                Brush fontColor = IsCanAny() ? Brushes.Black : Brushes.Gray;
                int x = _x + _w * i;
                g.FillRectangle(whiteBrush, x + 1, _y, _w / 2 * 5 - 2, _h);
                g.DrawString(String.Format("{0, 2}", s_string[i]), font, fontColor, new PointF(x, _y));
            }
        }
    }
}
