using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Hai
    {
        public enum eType
        {
            Manzu,
            Pinzu,
            Souzu,
            Zihai,
        }

        public enum eNumber
        {
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,

            Ton = 0,
            Nan,
            Sha,
            Pei,
            Haku,
            Hatu,
            Thun,
        }

        public enum eName
        {
            Manzu1,
            Manzu2,
            Manzu3,
            Manzu4,
            Manzu5,
            Manzu6,
            Manzu7,
            Manzu8,
            Manzu9,

            Pinzu1,
            Pinzu2,
            Pinzu3,
            Pinzu4,
            Pinzu5,
            Pinzu6,
            Pinzu7,
            Pinzu8,
            Pinzu9,

            Souzu1,
            Souzu2,
            Souzu3,
            Souzu4,
            Souzu5,
            Souzu6,
            Souzu7,
            Souzu8,
            Souzu9,

            Ton,
            Nan,
            Sha,
            Pei,
            Haku,
            Hatu,
            Thun,

            Null
        }

        public eName Next(int idx)
        {
            int n = (int)Number + idx;
            if (Type == eType.Zihai || n < (int)eNumber.Num1 || (int)eNumber.Num9 < n) { return eName.Null; }
            return name(_type, (eNumber)n);
        }

        private eType _type;
        private eNumber _num;

        private bool _nakikouho;
        public bool Nakikouho
        {
            get { return _nakikouho; }
            set { _nakikouho = value; }
        }

        private bool _nakichoice;
        public bool Nakichoice
        {
            get { return _nakichoice; }
            set { _nakichoice = value; }
        }

        public void ResetNakikouho()
        {
            _nakikouho = false;
            _nakichoice = false;
        }

        static Int32 s_width = 48;
        static Int32 s_height = 64;

        private Int32 _rot = 0;

        private Bitmap _bmp;
        private Rectangle _bmpRect;

        private static Bitmap s_bmpUra = Properties.Resources.hai_back;
        private static Rectangle s_bmpUraRect = new Rectangle(0, 0, s_bmpUra.Width, s_bmpUra.Height);

        private Point[] _points = {
            new Point(      0,        0),
            new Point(s_width,        0),
            new Point(      0, s_height)
        };

        private Point[] _points_yoko = {
            new Point(       0, (s_width + s_height) / 2),
            new Point(       0, (s_width + s_height) / 2 - s_width),
            new Point(s_height, (s_width + s_height) / 2)
        };

        public Hai(eType type, eNumber num)
        {
            this._type = type;
            this._num = num;
            this._nakikouho = false;

            if (_type == eType.Manzu)
            {
                _bmp = Properties.Resources.hai_manzu;
            }
            else if (_type == eType.Pinzu)
            {
                _bmp = Properties.Resources.hai_pinzu;
            }
            else if (_type == eType.Souzu)
            {
                _bmp = Properties.Resources.hai_souzu;
            }
            else if (num == eNumber.Ton ||
                num == eNumber.Nan ||
                num == eNumber.Sha ||
                num == eNumber.Pei)
            {
                _bmp = Properties.Resources.hai_sufon;
            }
            else
            {
                _bmp = Properties.Resources.hai_sangen;
            }

            {
                int top = 0;
                int div = 9;

                if (_type == eType.Zihai)
                {
                    if (_num == eNumber.Ton ||
                        _num == eNumber.Nan ||
                        _num == eNumber.Sha ||
                        _num == eNumber.Pei)
                    {
                        div = 4;
                    }
                    else
                    {
                        top = 4;
                        div = 3;
                    }

                }

                int w = _bmp.Width / div;
                int h = _bmp.Height;

                _bmpRect = new Rectangle(((int)num - top) * w, 0, w, h);
            }
        }

        public eName Name
        {
            get { return (eName)((int)_type * 9 + (int)_num); }
        }

        private static eName name(eType type, eNumber number)
        {
            return (eName)((int)type * 9 + (int)number);
        }

        public eType Type
        {
            get { return _type; }
        }

        public eNumber Number
        {
            get { return _num; }
        }

        static float DegToRad(Int32 deg)
        {
            return MathF.PI * deg / 180.0f;
        }

        public void SetPos(Int32 x, Int32 y)
        {
            var sc = MathF.SinCos(DegToRad(_rot));
            _points[0].X = x;
            _points[0].Y = y;
            _points[1].X = _points[0].X + s_width;
            _points[1].Y = _points[0].Y;
            _points[2].X = _points[0].X;
            _points[2].Y = _points[0].Y + s_height;

            sc = MathF.SinCos(DegToRad(_rot + 90));
            _points_yoko[0].X = x;
            _points_yoko[0].Y = y + (s_width + s_height) / 2;
            _points_yoko[1].X = _points_yoko[0].X;
            _points_yoko[1].Y = _points_yoko[0].Y - s_width;
            _points_yoko[2].X = _points_yoko[0].X + s_height;
            _points_yoko[2].Y = _points_yoko[0].Y;
        }

        private Point[] getOffsetPos(int ofs)
        {
            var sc = MathF.SinCos(DegToRad(_rot));
            Point[] tmp = (Point[])_points.Clone();
            tmp[0].X += (Int32)(ofs * sc.Sin);
            tmp[0].Y += (Int32)(ofs * sc.Cos);
            tmp[1].X += (Int32)(ofs * sc.Sin);
            tmp[1].Y += (Int32)(ofs * sc.Cos);
            tmp[2].X += (Int32)(ofs * sc.Sin);
            tmp[2].Y += (Int32)(ofs * sc.Cos);
            return tmp;
        }

        public void SetRot(Int32 rot)
        {
            this._rot = rot;
            SetPos(_points[0].X, _points[0].Y);
        }

        public int Draw(Graphics g, bool isYoko = false, bool isUra = false)
        {
            if (isUra)
            {
                g.DrawImage(s_bmpUra, _points, s_bmpUraRect, GraphicsUnit.Pixel);
                return s_width;
            }
            else if (_nakikouho)
            {
                int ofs = _nakichoice ? -s_height / 2 : -s_height / 4;
                Point[] tmp = getOffsetPos(ofs);
                g.DrawImage(_bmp, tmp, _bmpRect, GraphicsUnit.Pixel);
                return s_width;
            }
            else if (isYoko)
            {
                g.DrawImage(_bmp, _points_yoko, _bmpRect, GraphicsUnit.Pixel);
                return s_height;
            }
            else
            {
                g.DrawImage(_bmp, _points, _bmpRect, GraphicsUnit.Pixel);
                return s_width;
            }
        }

        public bool IsClick(int x, int y)
        {
            int xmin = int.MaxValue;
            int xmax = int.MinValue;
            int ymin = int.MaxValue;
            int ymax = int.MinValue;

            foreach (var point in _points)
            {
                xmin = Math.Min(xmin, point.X);
                xmax = Math.Max(xmax, point.X);
                ymin = Math.Min(ymin, point.Y);
                ymax = Math.Max(ymax, point.Y);
            }


            return (xmin < x && x < xmax) && (ymin < y && y < ymax);

            //            return false;
        }
    }
}
