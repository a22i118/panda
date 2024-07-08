using server;
using System.Drawing.Drawing2D;
using static server.Hai;

namespace server
{
    public partial class Form1 : Form
    {
        private GameManager _gameManager = new GameManager();

        // Socket通信のテスト
        //private Socket _socket = new Socket();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            _gameManager.Draw(g);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            pb.Invalidate();
            _gameManager.ClickCheck(e.X, e.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _gameManager.Exec();
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