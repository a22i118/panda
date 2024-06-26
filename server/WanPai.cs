using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class WanPai
    {
        private List<Hai> _rinshams = new List<Hai>();
        private List<Hai> _doras = new List<Hai>();
        private List<Hai.eName> _doraNames = new List<Hai.eName>();
        private List<Hai.eName> _doraUraNames = new List<Hai.eName>();
        public List<Hai> Rinshams { get { return _rinshams; } set { _rinshams = value; } }
        public List<Hai.eName> DoraNames { get { return _doraNames; } }
        public List<Hai.eName> DoraUraNames { get { return _doraUraNames; } }
        public WanPai() { }
        public void Init(Yama yama)
        {
            _rinshams.Clear();
            _doras.Clear();
            _doraNames.Clear();
            _doraUraNames.Clear();
            if (yama != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    _rinshams.Add(yama.RinshanTsumo());
                }
                for (int i = 0; i < 10; i++)
                {
                    _doras.Add(yama.RinshanTsumo());
                }
            }
            _doras[0] = new Hai(Hai.Manzu9);
            _doraNames.Add(_doras[0].DoraNext());
            _doraUraNames.Add(_doras[5].DoraNext());
        }

        /// <summary>
        /// ツモ牌を返す
        /// </summary>
        /// <returns></returns>
        public Hai Tsumo()
        {
            if (_rinshams.Count <= 0)
                return null;

            Hai tsumo = _rinshams[0];
            _rinshams.Remove(tsumo);
            return tsumo;
        }
        public void Dora(int kansCount)
        {
            _doraNames.Add(_doras[kansCount].DoraNext());
            _doraUraNames.Add(_doras[kansCount + 5].DoraNext());
        }
        public void Draw(Graphics g, int kansCount)
        {
            Font font = new Font(new FontFamily("HGS行書体"), 24, FontStyle.Bold);
            g.DrawString("ドラ", font, Brushes.Black, new PointF(280, 50));
            int x = 400 - 48;
            for (int i = 0; i < 5; i++)
            {
                _doras[i].SetPos(x += 48, 50);
                if (i <= kansCount)
                {
                    _doras[i].Draw(g);
                }
                else
                {
                    _doras[i].Draw(g, false, true);
                }
            }
        }
        /// <summary>
        /// 上がり時にドラ、裏ドラを表示する
        /// </summary>
        /// <param name="g"></param>
        /// <param name="isReach"></param>
        /// <param name="kansCount"></param>
        public void AgariDraw(Graphics g, bool isReach, int kansCount)
        {
            Font font = new Font(new FontFamily("HGS行書体"), 24, FontStyle.Bold);
            g.DrawString("ドラ", font, Brushes.Red, new PointF(300, 670));
            g.DrawString("裏ドラ", font, Brushes.Red, new PointF(750, 670));
            int x = 430 - 48;
            for (int i = 0; i < 5; i++)
            {
                _doras[i].SetPos(x += 48, 650);
                if (i <= kansCount)
                {
                    _doras[i].Draw(g);

                }
                else
                {
                    _doras[i].Draw(g, false, true);
                }
            }
            x += 250;
            for (int i = 5; i < 10; i++)
            {
                _doras[i].SetPos(x += 48, 650);
                if (isReach && i <= kansCount + 5)
                {
                    _doras[i].Draw(g);
                }
                else
                {
                    _doras[i].Draw(g, false, true);
                }
            }
        }
    }
}
