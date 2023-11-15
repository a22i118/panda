using server;
using System.Drawing.Drawing2D;

namespace reversi
{
    public partial class Form1 : Form
    {
        const int rows = 4;
        const int columns = 4;
        const int players = 4;

        const int YNum = rows;
        const int XNum = columns;

        Reversi scene = new Reversi(XNum, YNum);

        public Form1()
        {
            InitializeComponent();
            Yama yama = new Yama();
            //Tehai tehai = new Tehai();
            Tehai[] tehais = new Tehai[players];
            Kawa[] kawas = new Kawa[players];

            for(int i=0; i<players; i++)
            {
                tehais[i]= new Tehai();
                kawas[i]= new Kawa();
            }

            //四人に配る
            for(int  i=0; i<players; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
            }
            //手牌のソート
            for(int i = 0; i<players; i++)
            {
                tehais[i].Sort();
            }

            //確認用手牌をつくるぜ

            //tehai.Add(yama.List[0]);
            //yama.List.RemoveAt(0);
            Tehai tehai = new Tehai();
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));

            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));

            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));
            tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));

            tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num2));
            tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num3));

            tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num1));


            AtariList atariList = new AtariList(tehai);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            scene = new Reversi(XNum, YNum);

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            PictureBox pb = (PictureBox)sender;

            Size size = pb.Size;

            int wh;
            {
                int w = size.Width / XNum;
                int h = size.Height / YNum;

                wh = w < h ? w : h;
            }

            int margine = wh / 8;
            Font font = new Font(new FontFamily("Arial"), wh / 8, FontStyle.Bold);

            for (int y = 0; y < YNum; y++)
            {
                int tmp_y = y * wh;

                for (int x = 0; x < XNum; x++)
                {
                    int tmp_x = x * wh;

                    g.DrawRectangle(Pens.Black, tmp_x, tmp_y, wh, wh);

                    if (scene.IsExist(x, y, Reversi.ePLAYER.P0))
                    {
                        g.FillEllipse(Brushes.Black, tmp_x + margine, tmp_y + margine, wh - 2 * margine, wh - 2 * margine);
                    }
                    if (scene.IsExist(x, y, Reversi.ePLAYER.P1))
                    {
                        g.FillEllipse(Brushes.White, tmp_x + margine, tmp_y + margine, wh - 2 * margine, wh - 2 * margine);
                    }
                    if (scene.IsPlaceable(x, y, Reversi.ePLAYER.P0))
                    {
                        g.FillEllipse(Brushes.Black, tmp_x + wh / 2 - wh / 8, tmp_y + wh / 2 - wh / 8, wh / 4, wh / 4);
                        g.FillEllipse(Brushes.Black, tmp_x + wh / 2 - wh / 8, tmp_y + wh / 2 - wh / 8, wh / 4, wh / 4);
                        g.DrawString(String.Format("{0, 2}", scene.result[x, y].winRate((int)Reversi.ePLAYER.P0)), font, Brushes.White, new PointF(tmp_x, tmp_y));
                    }
                    if (scene.IsPlaceable(x, y, Reversi.ePLAYER.P1))
                    {
                        g.FillEllipse(Brushes.White, tmp_x + wh / 2 - wh / 8, tmp_y + wh / 2 - wh / 8, wh / 4, wh / 4);
                        g.DrawString(String.Format("{0, 2}", scene.result[x, y].winRate((int)Reversi.ePLAYER.P1)), font, Brushes.White, new PointF(tmp_x, tmp_y));
                    }
                }
            }

            if (scene.Player == Reversi.ePLAYER.P0)
            {
                g.FillEllipse(Brushes.Black, (XNum + 1) * wh + wh / 6, 1 * wh + wh / 5, wh / 2, wh / 2);
            }
            else
            {
                g.FillEllipse(Brushes.White, (XNum + 1) * wh + wh / 6, 2 * wh + wh / 5, wh / 2, wh / 2);
            }

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.FillMode = FillMode.Winding;

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Center;

            Font fnt_seat = new Font(new FontFamily("Arial"), wh / 2, FontStyle.Bold);

            path.AddString(String.Format("{0, 2}", scene.Places(Reversi.ePLAYER.P0)), fnt_seat.FontFamily, (int)fnt_seat.Style, wh / 2, new Point((XNum + 1) * wh, 1 * wh + wh / 2), stringFormat);

            //g.DrawPath(new Pen(Color.Black, 8), path);
            g.FillPath(new SolidBrush(Color.Black), path);

            path.Reset();
            path.AddString(String.Format("{0, 2}", scene.Places(Reversi.ePLAYER.P1)), fnt_seat.FontFamily, (int)fnt_seat.Style, wh / 2, new Point((XNum + 1) * wh, 2 * wh + wh / 2), stringFormat);

            //g.DrawPath(new Pen(Color.White, 8), path);
            g.FillPath(new SolidBrush(Color.White), path);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            Size size = pb.Size;

            int wh;
            {
                int w = size.Width / XNum;
                int h = size.Height / YNum;

                wh = w < h ? w : h;
            }

            int x = e.X / wh;
            int y = e.Y / wh;

            if (scene.IsPlaceable(x, y))
            {
                scene.Place(x, y);
            }

            pb.Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}