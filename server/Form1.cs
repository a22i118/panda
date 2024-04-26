using server;
using System.Drawing.Drawing2D;
using static server.Hai;

namespace reversi
{
    public partial class Form1 : Form
    {
        const int rows = 4;
        const int columns = 4;
        const int players = 4;

        const int YNum = rows;
        const int XNum = columns;

        GameManager gameManager = new GameManager();
        public Form1()
        {

            InitializeComponent();



            //Tehai tehai = new Tehai();
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num1));

            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num2));

            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));
            //tehai.Add(new Hai(Hai.eType.Manzu, Hai.eNumber.Num3));

            //tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num1));
            //tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num2));
            //tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num3));

            //tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num1));
            //tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num1));

            //AtariList atariList = new AtariList(tehai);

#if false
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

            //確認用手牌

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

            tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num2));
            tehai.Add(new Hai(Hai.eType.Pinzu, Hai.eNumber.Num3));

            tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num1));
            tehai.Add(new Hai(Hai.eType.Souzu, Hai.eNumber.Num1));

#endif


        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            gameManager.Draw(g);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            pb.Invalidate();

            // 説明（読んで理解したら消すこと）

            // pictureBoxの幅size.Width、高さsize.Heightで取得できる
            // クリックの位置は(e.X, e.Y)
            gameManager.ClickCheck(e.X, e.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 説明（読んで理解したら消すこと）

            // ツールボックスからtimerをformにDrag&Dropして追加する
            // timerのプロパティからTickイベントをクリックするとこの関数が作られる
            // Intervalを指定することで指定したミリ秒ごとにこのイベントが呼び出される
            // Interval=100(100/1000秒)

            // ここでgameManagerのExecを呼び出す
            gameManager.Exec();

            pictureBox1.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}