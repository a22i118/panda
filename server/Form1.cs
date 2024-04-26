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

            //�l�l�ɔz��
            for(int  i=0; i<players; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    tehais[i].Add(yama.List[0]);
                    yama.List.RemoveAt(0);
                }
            }
            //��v�̃\�[�g
            for(int i = 0; i<players; i++)
            {
                tehais[i].Sort();
            }

            //�m�F�p��v

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

            // �����i�ǂ�ŗ���������������Ɓj

            // pictureBox�̕�size.Width�A����size.Height�Ŏ擾�ł���
            // �N���b�N�̈ʒu��(e.X, e.Y)
            gameManager.ClickCheck(e.X, e.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // �����i�ǂ�ŗ���������������Ɓj

            // �c�[���{�b�N�X����timer��form��Drag&Drop���Ēǉ�����
            // timer�̃v���p�e�B����Tick�C�x���g���N���b�N����Ƃ��̊֐��������
            // Interval���w�肷�邱�ƂŎw�肵���~���b���Ƃɂ��̃C�x���g���Ăяo�����
            // Interval=100(100/1000�b)

            // ������gameManager��Exec���Ăяo��
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