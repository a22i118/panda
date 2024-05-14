using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static server.GameManager;

namespace server
{
    internal class Player
    {
        public const int Num = 4;

        // 表示の起点
        private int _x;
        private int _y;

        private int _id;
        private bool _isOya;
        public bool IsOya { get { return _isOya; } set { _isOya = value; } }
        private bool _huriten = false;
        public bool Huriten { get { return _huriten; } set { _huriten = value; } }

        private Tehai _tehai = new Tehai();
        private Kawa _kawa = new Kawa();
        private TempaiCheck _tempaiCheck = null;
        private ActionCommand _actionCommand = new ActionCommand(0, 0, 0, 0);
        private AtariList? _atariList = null;
        public TempaiCheck TempaiCheck { get { return _tempaiCheck; } }
        public List<Result> Results { get { return _atariList.Results; } }
        public List<Hai> AtariHais { get { return _tempaiCheck.AtariHais; } }
        public ActionCommand ActionCommand { get { return _actionCommand; } }
        public Kawa Kawa { get { return _kawa; } }
        public int Id { get { return _id; } }
        public Tehai Tehai { get { return _tehai; } }
        public List<Hai> Hais { get { return _tehai.Hais; } }
        public Player(int id)
        {
            _id = id;
            _actionCommand = new ActionCommand(300, _id * 200 + 74 + 50, 64, 32);
        }
        public void Init()
        {
            _tehai.Init();
            _kawa.Init();
            if (_tempaiCheck != null) { _tempaiCheck.Init(); }
            _isOya = false;
        }
        public void Sort()
        {
            _actionCommand.Init();
            _tehai.Sort();
        }
        public void Deal(Hai hai)
        {
            _tehai.Add(hai);
        }
        public void Tsumo(Hai hai, ulong yakuMask)
        {
            _tehai.Add(hai);

            _atariList = new AtariList(_tehai, _isOya, yakuMask);

            if (_atariList.IsAtari())
            {
                _actionCommand.CanTsumo = true;
                //Console.WriteLine("アタリ");
            }

            if (IsCanAnKan() || IsCanKaKan())
            {
                _actionCommand.CanKan = true;
            }

        }
        public void Tempai(Tehai tehai, ulong yakumask)
        {
            _tempaiCheck = new TempaiCheck(tehai, _isOya, yakumask);
        }
        public void Ron(Hai hai, ulong yakuMask, bool isCanChi)
        {
            _atariList = new AtariList(_tehai, _isOya, yakuMask, hai);

            if (_atariList.IsAtari())
            {
                _actionCommand.CanRon = true;
            }
            // チーのコマンドを有効にする
            if (isCanChi && IsCanChi(hai))
            {
                _actionCommand.CanChi = true;
            }

            // ポンのコマンドを有効にする
            if (IsCanPon(hai))
            {
                _actionCommand.CanPon = true;
            }

            // カンのコマンドを有効にする
            if (IsCanMinKan(hai))
            {
                _actionCommand.CanKan = true;
            }
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
        public Hai Throw(int x, int y)
        {
            return _tehai.Throw(x, y, _kawa);
        }
        public void ResetNakikouho()
        {
            _actionCommand.Init();
            _tehai.ResetNakikouho();
        }
        public void Restart()
        {

        }
        //public ActionCommand(int x, int y,int w, int h)
        //{

        //}
        public bool IsCanAny()
        {
            return _actionCommand.IsCanAny();
        }
        public void Draw(Graphics g, bool teban)
        {
            _tehai.Draw(g, _id);
            _kawa.Draw(g, _id);
            _actionCommand.Draw(g, teban);
        }
        public void AgariDraw(Graphics g, Hai hai)
        {
            _tehai.AgariDraw(g, hai);
        }
        public void TempaiDraw(Graphics g)
        {
            if (_tempaiCheck != null)
            {
                _tempaiCheck.Draw(g, _id);
            }

        }

        public string[] YakuString()
        {
            return _atariList.YakuString();
        }
        public string[] FuString()
        {
            return _atariList.FuString();
        }
        public string[] HanString()
        {
            return _atariList.HanString();
        }
        public string[] TenString()
        {
            return _atariList.TenString();
        }
        public bool CommandUpdate(int x, int y)
        {
            
            return _actionCommand.Click(x, y);
        }
        public bool IsCallKan() { return _actionCommand.IsCallKan(); }
        public bool IsCallRon() { return _actionCommand.IsCallRon(); }
        public bool IsCallTsumo() { return (_actionCommand.IsCallTsumo()); }
        public bool IsCallChi() { return _actionCommand.IsCallChi(); }
        public bool IsCallPon() { return _actionCommand.IsCallPon(); }

        public int SarashiCount() { return _tehai.SarashiCount(); }
    }
}
