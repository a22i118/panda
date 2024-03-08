using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Player
    {
        public const int Num = 4;

        // 表示の起点
        private int _x;
        private int _y;

        private int _id;

        private Tehai _tehai = new Tehai();
        private Kawa _kawa = new Kawa();
        private ActionCommand[]? _actionCommand = null;
        private AtariList? _atariList = null;

        public Player(int id)
        {
            _id = id;
        }

        public void Restart()
        {

        }

        public void Draw()
        {

        }
    }
}
