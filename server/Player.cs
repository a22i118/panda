using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public Tehai _tehai = new Tehai();
        private Kawa _kawa = new Kawa();
        private ActionCommand[]? _actionCommand = null;
        private AtariList? _atariList = null;

        public Player(int id)
        {
            _id = id;
        }
        public void Init()
        {
            _tehai.Init();
            _kawa.Init();
        }
        public void Sort()
        {
            _tehai.Sort();
        }
        public void Tsumo(Hai hai)
        {
            _tehai.Add(hai);
        }
        public bool Chi(Hai _sutehai)
        {
            return _tehai.Chi(_sutehai);
        }
        public void Pon(Hai _sutehai, int turn)
        {
            _tehai.Pon(_sutehai, (turn - _id + Player.Num) % Player.Num);
        }
        public void MinKan(Hai _sutehai, int turn)
        {
            _tehai.MinKan(_sutehai, (turn - _id + Player.Num) % Player.Num);
        }
        public bool AnKan()
        {
            return _tehai.AnKan();
        }
        public bool IsCanTsumo()
        {
            return _tehai.IsCanTsumo();
        }
        public bool IsCanChi(Hai hai)
        {
            return _tehai.IsCanChi(hai);
        }
        public bool IsCanPon(Hai hai)
        {
            return _tehai.IsCanPon(hai);
        }
        public bool IsCanAnKan()
        {
            return _tehai.IsCanAnKan();
        }
        public bool IsCanKaKan()
        {
            return _tehai.IsCanKaKan();
        }
        public bool IsCanMinKan(Hai hai)
        {
            return _tehai.IsCanMinKan(hai);
        }
        public Hai Click(int x, int y)
        {
            return _tehai.Click(x, y);
        }
        public Hai Click(int x, int y, Kawa _kawa)
        {
            return _tehai.Click(x, y, _kawa);
        }
        public void ResetNakikouho()
        {
            _tehai.ResetNakikouho();
        }
        public void Restart()
        {

        }

        public void Draw(Graphics g)
        {
            _tehai.Draw(g, _id);
        }
    }
}
