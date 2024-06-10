using System;
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
        private int _id;
        private bool _isOya;
        private bool _huriten = false;
        private bool _isTempai = false;
        private bool _richiHuriten = false;
        private bool _isReach = false;
        private bool _isHaiteiHoutei = false;
        private bool _isDabReach = false;
        private Tehai _tehai = new Tehai();
        private Kawa _kawa = new Kawa();
        private TempaiCheck _tempaiCheck = null;
        private AtariList? _atariList = null;
        private List<Hai> _richiAtariHais;
        private List<Hai> _choiceAtariHais;
        private ActionCommand _actionCommand = new ActionCommand(0, 0, 0, 0);
        private Dictionary<Hai, List<Hai>> AtariHaisDic = new Dictionary<Hai, List<Hai>>();

        public const int Num = 4;

        public int Id { get { return _id; } }
        public int KansCount { get { return _tehai.Kans.Count; } }
        public bool IsOya { get { return _isOya; } set { _isOya = value; } }
        public bool Huriten { get { return _huriten; } set { _huriten = value; } }
        public bool IsTempai { get { return _isTempai; } set { _isTempai = value; } }
        public bool IsHaiteiHoutei { get { return _isHaiteiHoutei; } set { _isHaiteiHoutei = value; } }
        public bool IsReach { get { return _isReach; } set { _isReach = value; } }
        public bool IsDabReach { get { return _isDabReach; } set { _isDabReach = value; } }
        public TempaiCheck TempaiCheck { get { return _tempaiCheck; } }
        public List<Result> Results { get { return _atariList.Results; } }

        public List<Hai> ChoiceAtariHais { get { return _choiceAtariHais; } }
        public List<Hai> RichiAtariHais { get { return _richiAtariHais; } set { _richiAtariHais = value; } }
        public List<Hai> AtariHais { get { return _tempaiCheck == null ? null : _tempaiCheck.AtariHais; } }
        public ActionCommand ActionCommand { get { return _actionCommand; } }
        public Kawa Kawa { get { return _kawa; } }
        public Tehai Tehai { get { return _tehai; } }
        //public List<Hai> Hais { get { return _tehai.Hais; } }
        public Player(int id)
        {
            _id = id;
            _actionCommand = new ActionCommand(300, _id * 200 + 74 + 150, 64, 32);
        }
        public void Init()
        {
            _tehai.Init();
            _kawa.Init();
            if (_tempaiCheck != null) { _tempaiCheck.Init(); }
            if (_richiAtariHais != null) { _richiAtariHais.Clear(); }
            if (_choiceAtariHais != null) { _choiceAtariHais.Clear(); }
            if (AtariHaisDic != null) { AtariHaisDic.Clear(); }
            _isOya = false;
            _richiHuriten = false;
            _huriten = false;
            _isTempai = false;
            _isReach = false;
            _isDabReach = false;
            _isHaiteiHoutei = false;
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
        public void Tsumo(Hai hai, ulong yakuMask, int kansCount)
        {
            _huriten = false;
            _tehai.Add(hai);

            yakuMask |= Yaku.Tsumo.Mask;
            if (_isDabReach)
            {
                yakuMask |= Yaku.DabuRe.Mask;
            }
            else if (_isReach)
            {
                yakuMask |= Yaku.Reach.Mask;
            }
            if (_tehai.IsIppatsu)
            {
                yakuMask |= Yaku.Ippatsu.Mask;
            }
            if (_tehai.IsRinshan)
            {
                yakuMask |= Yaku.Rinshankaiho.Mask;
            }
            if (_isHaiteiHoutei)
            {
                yakuMask |= Yaku.Haiteiraoyue.Mask;
            }

            _atariList = new AtariList(_tehai, _isOya, yakuMask);

            if (_atariList.IsAtari())
            {
                _actionCommand.CanTsumo = true;
            }

            if (kansCount < 4 && (IsCanAnKan() || IsCanKaKan()))
            {
                _actionCommand.CanKan = true;
            }
            if (IsEnableReach(yakuMask) && _tehai.NowReach == false && _tehai.NakiCount() == 0)
            {
                _actionCommand.CanRichi = true;
            }

        }
        public void Tempai(Tehai tehai, ulong yakumask)
        {
            _tempaiCheck = new TempaiCheck(tehai, _isOya, yakumask);
        }

        public void ChoiceTempai()
        {
            foreach (Hai choiceHai in _tehai.Hais)
            {
                if (AtariHaisDic != null && choiceHai.ThrowChoice && AtariHaisDic.ContainsKey(choiceHai))
                {
                    _choiceAtariHais = AtariHaisDic[choiceHai];
                }
            }
        }


        public bool HuritenCheck(Hai suteHai)
        {
            if (_richiHuriten)
            {
                _huriten = _richiHuriten;
            }

            if (_huriten)
            {
                return true;
            }
            if (_richiAtariHais != null)
            {
                foreach (var atariHai in _richiAtariHais)
                {
                    if (suteHai != null)
                    {
                        if (suteHai.Name == atariHai.Name)
                        {
                            _richiHuriten = true;
                            _huriten = true;
                            return true;
                        }
                    }
                    if (_kawa.Hais.Find(hai => hai.Name == atariHai.Name) != null)
                    {
                        _richiHuriten = true;
                        _huriten = true;
                        return true;
                    }
                }
            }
            if (AtariHais != null)
            {
                foreach (var atariHai in AtariHais)
                {
                    if (suteHai != null)
                    {
                        if (suteHai.Name == atariHai.Name)
                        {
                            _huriten = true;
                            return true;
                        }
                    }
                    if (_kawa.Hais.Find(hai => hai.Name == atariHai.Name) != null)
                    {
                        _huriten = true;

                        return true;
                    }
                }
            }
            return false;
        }

        public void CommandValid(Hai hai, Tehai tehai, int kansCount, ulong yakuMask, bool isCanChi)
        {
            if (_isDabReach)
            {
                yakuMask |= Yaku.DabuRe.Mask;
            }
            else if (_isReach)
            {
                yakuMask |= Yaku.Reach.Mask;
            }
            if (_tehai.IsIppatsu)
            {
                yakuMask |= Yaku.Ippatsu.Mask;
            }
            if (_isHaiteiHoutei)
            {
                yakuMask |= Yaku.Hoteiraoyui.Mask;
            }

            _atariList = new AtariList(_tehai, _isOya, yakuMask, hai);

            //ロンのコマンドを有効にする
            if (_huriten == false && _atariList.IsAtari())
            {
                _actionCommand.CanRon = true;
            }
            // チーのコマンドを有効にする
            if (isCanChi && IsCanChi(hai) && tehai.NowReach == false)
            {
                _actionCommand.CanChi = true;
            }

            // ポンのコマンドを有効にする
            if (IsCanPon(hai) && tehai.NowReach == false)
            {
                _actionCommand.CanPon = true;
            }

            // カンのコマンドを有効にする
            if (kansCount < 4 && IsCanMinKan(hai) && tehai.NowReach == false)
            {
                _actionCommand.CanKan = true;
            }
        }
        public void Reach()
        {
            for (int i = 0; i < _tehai.Hais.Count; i++)
            {
                _tehai.Hais[i].ThrowChoice = false;
                _tehai.Hais[i].ReachThrowChoice = true;
            }

            _tehai.DeclareReach = true;
            _isReach = true;
            _tehai.IsIppatsu = true;
        }
        //リーチできるかどうか
        public bool IsEnableReach(ulong yakumask)
        {
            bool isRichi = false;
            AtariHaisDic.Clear();

            Hai hai;
            for (int i = 0; i < _tehai.Hais.Count; i++)
            {
                Tehai tmp = new Tehai(_tehai);
                if (i == 0 || i! > 0 && !(tmp.Hais[i - 1].Name == tmp.Hais[i].Name))
                {
                    hai = tmp.Hais[i];
                    tmp.Hais.Remove(tmp.Hais[i]);
                    Tempai(tmp, yakumask);
                    List<Hai> list = new List<Hai>(AtariHais);
                    AtariHaisDic.Add(hai, list);
                }
                else
                {
                    hai = tmp.Hais[i];
                    List<Hai> list = new List<Hai>(AtariHais);
                    AtariHaisDic.Add(hai, list);
                }

                if (AtariHais.Count != 0)
                {
                    _tehai.Hais[i].IsRichi = true;
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
        public bool IsCanAny()
        {
            return _actionCommand.IsCanAny();
        }
        public void Draw(Graphics g, bool teban)
        {
            _tehai.Draw(g, _id);
            _kawa.Draw(g, _id);
            _actionCommand.Draw(g, teban);
            //if (_tempaiCheck != null)
            //{
            //    _tempaiCheck.Draw(g, _id);
            //}
            if (_richiAtariHais != null)
            {
                int x = 300 - 48;

                for (int i = 0; i < _richiAtariHais.Count; i++)
                {
                    _richiAtariHais[i].SetPos(x += 48, _id * 200 + 110 + 150);
                    _richiAtariHais[i].Draw(g);
                }
            }
            if (_choiceAtariHais != null)
            {
                int x = 300 - 48;

                for (int i = 0; i < _choiceAtariHais.Count; i++)
                {
                    _choiceAtariHais[i].SetPos(x += 48, _id * 200 + 110 + 150);
                    _choiceAtariHais[i].Draw(g);
                }
            }
            Font font = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);
            if (_huriten)
            {
                g.DrawString("振聴", font, Brushes.Red, new PointF(150, Id * 200 + 150));
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
        public int NakiCount() { return _tehai.NakiCount(); }
    }
}
