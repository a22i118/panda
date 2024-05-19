﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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
        private bool _isTempai = false;
        public bool IsTempai { get { return _isTempai; } set { _isTempai = value; } }
        //private bool _nowRichi = false;
        //public bool NowRichi { get { return _nowRichi; } set { _nowRichi = value; } }
        //private bool _declareRichi = false;
        //public bool DeclarRichi { get { return _declareRichi; } set { _declareRichi = value; } }

        private Tehai _tehai = new Tehai();
        private Kawa _kawa = new Kawa();
        //private CheckTehai _checkTehai = new CheckTehai(_tehai,_isOya,);
        private TempaiCheck _tempaiCheck = null;
        private ActionCommand _actionCommand = new ActionCommand(0, 0, 0, 0);
        private AtariList? _atariList = null;
        public TempaiCheck TempaiCheck { get { return _tempaiCheck; } }
        public List<Result> Results { get { return _atariList.Results; } }
        private List<Hai> _richiAtariHais;
        public List<Hai> RichiAtariHais { get { return _richiAtariHais; } set { _richiAtariHais = value; } }
        public List<Hai> AtariHais { get { return _tempaiCheck == null ? null : _tempaiCheck.AtariHais; } }
        public List<List<Hai>> _atariHaisList = new List<List<Hai>>();
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
            if (_richiAtariHais != null) { _richiAtariHais.Clear(); }
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
        public void Tsumo(Hai hai, Tehai tehai, ulong yakuMask)
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
            if (IsRichi(tehai, yakuMask) && tehai.NowRichi == false)
            {
                _actionCommand.CanRichi = true;
            }

        }
        public void Tempai(Tehai tehai, ulong yakumask)
        {
            _tempaiCheck = new TempaiCheck(tehai, _isOya, yakumask);
        }
        public bool HuritenCheck()
        {
            Huriten = false;

            if (AtariHais != null)
            {
                foreach (var atariHai in AtariHais)
                {
                    if (Kawa.Hais.Find(hai => hai.Name == atariHai.Name) != null)
                    {
                        Huriten = true;

                        return true;
                    }
                }
            }
            return false;
        }

        public void CommandValid(Hai hai, Tehai tehai, ulong yakuMask, bool isCanChi)
        {
            _atariList = new AtariList(_tehai, _isOya, yakuMask, hai);

            if (_huriten == false && _atariList.IsAtari())
            {
                _actionCommand.CanRon = true;
            }
            // チーのコマンドを有効にする
            if (isCanChi && IsCanChi(hai) && tehai.NowRichi == false)
            {
                _actionCommand.CanChi = true;
            }

            // ポンのコマンドを有効にする
            if (IsCanPon(hai) && tehai.NowRichi == false)
            {
                _actionCommand.CanPon = true;
            }

            // カンのコマンドを有効にする
            if (IsCanMinKan(hai) && tehai.NowRichi == false)
            {
                _actionCommand.CanKan = true;
            }
        }
        public void Richi(Tehai tehai)
        {
            for (int i = 0; i < tehai.Hais.Count; i++)
            {
                tehai.Hais[i].ThrowChoice = false;
                tehai.Hais[i].RichiThrowChoice = true;
            }

            tehai.DeclareRichi = true;

        }
        //リーチできるかどうか
        public bool IsRichi(Tehai tehai, ulong yakumask)
        {
            bool isRichi = false;
            _atariHaisList.Clear();

            for (int i = 0; i < tehai.Hais.Count; i++)
            {
                Tehai tmp = new Tehai(tehai);
                tmp.Hais.Remove(tmp.Hais[i]);
                Tempai(tmp, yakumask);
                if (AtariHais.Count != 0)
                {
                    _atariHaisList.Add(AtariHais);
                    tehai.Hais[i].IsRichi = true;
                    isRichi = true;
                }

            }
            AtariHais.Clear();

            if (isRichi)
            {
                return true;
            }

            return false;
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
            if (_tempaiCheck != null)
            {
                _tempaiCheck.Draw(g, _id);
            }
            if (_richiAtariHais != null)
            {
                int x = 300 - 48;

                for (int i = 0; i < _richiAtariHais.Count; i++)
                {
                    _richiAtariHais[i].SetPos(x += 48, _id * 200 + 110 + 50);
                    _richiAtariHais[i].Draw(g);
                }
            }
            Font font2 = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);
            if (_huriten)
            {
                g.DrawString("振聴", font2, Brushes.Red, new PointF(150, Id * 200 + 50));
            }
        }
        public void AgariDraw(Graphics g, Hai hai)
        {
            _tehai.AgariDraw(g, hai);
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
        public bool IsCallTsumo() { return _actionCommand.IsCallTsumo(); }
        public bool IsCallChi() { return _actionCommand.IsCallChi(); }
        public bool IsCallPon() { return _actionCommand.IsCallPon(); }
        public bool IsCallRichi() { return _actionCommand.IsCallRichi(); }

        public int SarashiCount() { return _tehai.SarashiCount(); }
    }
}
