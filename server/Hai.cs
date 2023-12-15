using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        eType type_;
        eNumber num_;

        static Int32 width = 48;
        static Int32 height = 64;

        private Int32 rot = 0;

        private Bitmap bmp;
        Rectangle bmpRect;

        Point[] points = {
            new Point(0, 0),
            new Point(width, 0),
            new Point(0, height)
        };

        public Hai(eType type, eNumber num)
        {
            this.type_ = type;
            this.num_ = num;

            if (type_ == eType.Manzu)
            {
                bmp = Properties.Resources.hai_manzu;
            }
            else if (type_ == eType.Pinzu)
            {
                bmp = Properties.Resources.hai_pinzu;
            }
            else if (type_ == eType.Souzu)
            {
                bmp = Properties.Resources.hai_souzu;
            }
            else if (num == eNumber.Ton ||
                num == eNumber.Nan ||
                num == eNumber.Sha ||
                num == eNumber.Pei)
            {
                bmp = Properties.Resources.hai_sufon;
            }
            else
            {
                bmp = Properties.Resources.hai_sangen;
            }

            {
                int top = 0;
                int div = 9;

                if (type_ == eType.Zihai)
                {
                    if (num_ == eNumber.Ton ||
                        num_ == eNumber.Nan ||
                        num_ == eNumber.Sha ||
                        num_ == eNumber.Pei)
                    {
                        div = 4;
                    }
                    else
                    {
                        top = 4;
                        div = 3;
                    }

                }

                int w = bmp.Width / div;
                int h = bmp.Height;

                bmpRect = new Rectangle(((int)num - top) * w, 0, w, h);
            }
        }

        public eName Name
        {
            get { return (eName)((int)type_ * 9 + (int)num_); }
        }

        public eType Type
        {
            get { return type_; }
        }

        public eNumber Number
        {
            get { return num_; }
        }

        static float DegToRad(Int32 deg)
        {
            return MathF.PI * deg / 180.0f;
        }

        public void SetPos(Int32 x, Int32 y)
        {
            var sc = MathF.SinCos(DegToRad(rot));
            points[0].X = x;
            points[0].Y = y;
            points[1].X = x + (Int32)(width * sc.Cos);
            points[1].Y = y + (Int32)(width * sc.Sin);
            points[2].X = x + (Int32)(height * sc.Sin);
            points[2].Y = y + (Int32)(height * sc.Cos);
        }

        public void SetRot(Int32 rot)
        {
            this.rot = rot;
            SetPos(points[0].X, points[0].Y);
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(bmp, points, bmpRect, GraphicsUnit.Pixel);
        }

        public bool IsClick(int x, int y)
        {
            int xmin = int.MaxValue;
            int xmax = int.MinValue;
            int ymin = int.MaxValue;
            int ymax = int.MinValue;

            foreach (var point in points)
            {
                xmin = Math.Min(xmin, point.X);
                xmax = Math.Max(xmax, point.X);
                ymin = Math.Min(ymin, point.Y);
                ymax = Math.Max(ymax, point.Y);
            }


            return (xmin <= x && x <= xmax) && (ymin <= y && y <= ymax);

//            return false;
        }
    }
}
