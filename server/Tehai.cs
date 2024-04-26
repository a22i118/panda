﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    internal class Tehai
    {
        private List<Hai> _hais = new List<Hai>();
        private List<Chi> _chis = new List<Chi>();
        private List<Pon> _pons = new List<Pon>();
        private List<Kan> _kans = new List<Kan>();
        private List<INaki> _naki = new List<INaki>();

        public List<Hai> Hais { get { return _hais; } }
        public List<Chi> Chis { get { return _chis; } }
        public List<Pon> Pons { get { return _pons; } }
        public List<Kan> Kans { get { return _kans; } }

        public int SarashiCount() { return _chis.Count + _pons.Count + _kans.Count; }

        public Tehai() { }

        public void Init()
        {
            _hais.Clear();
            _pons.Clear();
            _chis.Clear();
            _kans.Clear();
            _naki.Clear();
        }

        public void Add(Hai hai)
        {
            _hais.Add(hai);
        }

        public void Sort()
        {
            _hais.Sort((a, b) => (int)a.Name - (int)b.Name);
        }

        public void Draw(Graphics g, int players)
        {
            int x = 300 - 48;

            for (int i = 0; i < _hais.Count; i++)
            {
                _hais[i].SetPos(x += 48, players * 200);
                _hais[i].Draw(g);
            }

            if (IsCanTsumo())
            {
                x += 48;
            }

            x += 48;

            for (int i = _naki.Count - 1; i >= 0; i--)
            {
                x += 6;
                x = _naki[i].Draw(g, x, players * 200);
            }
        }
        public void AgariDraw(Graphics g, Hai hai)
        {
            int x = 300 - 48;
            for (int i = 0; i < _hais.Count; i++)
            {
                _hais[i].SetPos(x += 48, 800);
                _hais[i].Draw(g);
            }
            x += 72;
            for (int i = _naki.Count - 1; i >= 0; i--)
            {
                x += 6;
                x = _naki[i].Draw(g, x, 800);
            }
            if (hai != null)
            {

                hai.SetPos(x += 24, 800);
                hai.Draw(g);
            }

        }

        public Hai Click(int x, int y)
        {
            for (int i = 0; i < _hais.Count; i++)
            {
                if (_hais[i].IsClick(x, y))
                {
                    return _hais[i];
                }
            }
            return null;
        }

        public Hai Throw(int x, int y, Kawa kawas)
        {
            Hai del = null;

            for (int i = 0; i < _hais.Count; i++)
            {
                if (_hais[i].IsClick(x, y))
                {

                    kawas.Add(_hais[i]);

                    del = _hais[i];
                    break;

                }
            }

            if (del != null)
            {
                _hais.Remove(del);

            }

            return del;
        }

        public void Tsumo(Yama yama)
        {
            if (IsCanTsumo())
            {
                _hais.Add(yama.Tsumo());
            }
        }

        public bool IsCanTsumo()
        {
            return _hais.Count + 3 * (_chis.Count + _pons.Count + _kans.Count) < 14;
        }

        private bool find(eName hai1, eName hai2)
        {
            int i = 0;
            for (; i < _hais.Count; i++) { if (_hais[i].Name == hai1) { ++i; break; } }
            for (; i < _hais.Count; i++) { if (_hais[i].Name == hai2) { return true; } }
            return false;
        }

        public void ResetNakikouho()
        {
            foreach (var item in _hais)
            {
                item.ResetNakikouho();
            }
        }

        public bool IsCanChiRight(Hai hai) { return find(hai.Next(-2), hai.Next(-1)); }

        public bool IsCanChiCenter(Hai hai) { return find(hai.Next(-1), hai.Next(1)); }

        public bool IsCanChiLeft(Hai hai) { return find(hai.Next(1), hai.Next(2)); }

        public bool IsCanChi(Hai hai)
        {
            return IsCanChiRight(hai) || IsCanChiCenter(hai) || IsCanChiLeft(hai);
        }

        public bool Chi(Hai hai)
        {
            Hai[] choice =
            {
                _hais.Find(value => value.Name == hai.Next(-2) && value.Nakichoice),
                _hais.Find(value => value.Name == hai.Next(-1) && value.Nakichoice),
                _hais.Find(value => value.Name == hai.Next(1) && value.Nakichoice),
                _hais.Find(value => value.Name == hai.Next(2) && value.Nakichoice)
            };

            Hai[] kouho =
            {
                choice[0] != null ? choice[0] : _hais.Find(value => value.Name == hai.Next(-2)),
                choice[1] != null ? choice[1] : _hais.Find(value => value.Name == hai.Next(-1)),
                choice[2] != null ? choice[2] : _hais.Find(value => value.Name == hai.Next(1)),
                choice[3] != null ? choice[3] : _hais.Find(value => value.Name == hai.Next(2))
            };

            if (kouho[1] == null) { kouho[0] = null; }
            if (kouho[2] == null) { kouho[3] = null; }

            int kouho_num = kouho.Count(value => value != null);

            bool[] is_kouho = new bool[4]{
                kouho[0] != null,
                kouho[1] != null,
                kouho[2] != null,
                kouho[3] != null,
            };

            int pattern = -1;

            if (kouho_num == 2)
            {
                if (is_kouho[0] && is_kouho[1]) { pattern = 0; }
                else if (is_kouho[1] && is_kouho[2]) { pattern = 1; }
                else if (is_kouho[2] && is_kouho[3]) { pattern = 2; }
            }
            else if (kouho_num == 3)
            {
                if (is_kouho[0] && is_kouho[1] && is_kouho[2])
                {
                    if (choice[0] != null) { pattern = 0; }
                    else if (choice[2] != null) { pattern = 1; }
                    kouho[1].Nakichoice = true;
                }
                else if (is_kouho[1] && is_kouho[2] && is_kouho[3])
                {
                    if (choice[1] != null) { pattern = 1; }
                    else if (choice[3] != null) { pattern = 2; }
                    kouho[2].Nakichoice = true;
                }
            }
            else if (kouho_num == 4)
            {
                if (choice[0] != null) { pattern = 0; }
                else if (choice[3] != null) { pattern = 2; }
                else if (choice[1] != null && choice[2] != null) { pattern = 1; }
            }

            if (pattern == 0)
            {
                Chi chi = new Chi(kouho[0], kouho[1], hai, 1);
                _chis.Add(chi);
                _naki.Add(chi);
                _hais.Remove(kouho[0]);
                _hais.Remove(kouho[1]);
                return true;
            }
            else if (pattern == 1)
            {
                Chi chi = new Chi(kouho[1], hai, kouho[2], 2);
                _chis.Add(chi);
                _naki.Add(chi);
                _hais.Remove(kouho[1]);
                _hais.Remove(kouho[2]);
                return true;
            }
            else if (pattern == 2)
            {
                Chi chi = new Chi(hai, kouho[2], kouho[3], 3);
                _chis.Add(chi);
                _naki.Add(chi);
                _hais.Remove(kouho[2]);
                _hais.Remove(kouho[3]);
                return true;
            }

            foreach (var item in _hais)
            {
                if (item.Name == hai.Next(-2) ||
                    item.Name == hai.Next(-1) ||
                    item.Name == hai.Next(1) ||
                    item.Name == hai.Next(2))
                {
                    item.Nakikouho = true;
                }
            }

            int nakichoice = 0;
            foreach (var item in _hais)
            {
                if (item.Nakichoice)
                {
                    nakichoice++;
                }
            }

            if (nakichoice >= 2)
            {
                foreach (var item in _hais)
                {
                    if (item.Nakichoice)
                    {
                        item.Nakichoice = false;
                    }
                }
            }

            return false;
        }

        public bool IsCanPon(Hai hai) { return find(hai.Name, hai.Name); }

        public void Pon(Hai sutehai, int from)
        {
            Hai sutehai1 = null;
            Hai sutehai2 = null;
            int i;
            for (i = 0; i < _hais.Count; i++)
            {
                if (sutehai.Name == _hais[i].Name)
                {
                    sutehai1 = _hais[i];
                    i++;
                    break;
                }
            }

            for (; i < _hais.Count; i++)
            {
                if (sutehai.Name == _hais[i].Name)
                {
                    sutehai2 = _hais[i]; break;
                }
            }

            if (sutehai1 != null && sutehai2 != null)
            {
                Pon pon = new Pon(sutehai, sutehai1, sutehai2, from);
                _pons.Add(pon);
                _naki.Add(pon);
                _hais.Remove(sutehai1);
                _hais.Remove(sutehai2);
            }
        }

        public bool IsCanMinKan(Hai hai) { return _hais.Count(e => e.Name == hai.Name) >= 3; }

        public void MinKan(Hai hai, int from)
        {
            Hai hai1 = null;
            Hai hai2 = null;
            Hai hai3 = null;

            int i = 0;
            for (; i < _hais.Count; i++) { if (hai.Name == _hais[i].Name) { hai1 = _hais[i]; i++; break; } }
            for (; i < _hais.Count; i++) { if (hai.Name == _hais[i].Name) { hai2 = _hais[i]; i++; break; } }
            for (; i < _hais.Count; i++) { if (hai.Name == _hais[i].Name) { hai3 = _hais[i]; i++; break; } }

            if (hai1 != null && hai2 != null && hai3 != null)
            {
                Kan kan = new Kan(hai, hai1, hai2, hai3, from);
                _kans.Add(kan);
                _naki.Add(kan);
                _hais.Remove(hai1);
                _hais.Remove(hai2);
                _hais.Remove(hai3);
            }
        }

        private bool isCanAnKan(Hai hai) { return _hais.Count(e => e.Name == hai.Name) >= 4; }

        private bool isCanKaKan(Hai hai)
        {
            foreach (var pon in _pons) { if (pon.IsCanKaKan(hai)) { return true; } }
            return false;
        }

        public bool IsCanAnKan()
        {
            foreach (var hai in _hais) { if (isCanAnKan(hai)) { return true; } }
            return false;
        }

        public bool IsCanKaKan()
        {
            foreach (var hai in _hais) { if (isCanKaKan(hai)) { return true; } }
            return false;
        }

        public bool AnKan()
        {
            List<Hai> kouhos = new List<Hai>();
            Hai choice = null;

            foreach (var hai in _hais)
            {
                if ((isCanAnKan(hai) || isCanKaKan(hai)) && kouhos.Find(e => e.Name == hai.Name) == null)
                {
                    kouhos.Add(hai);
                    Hai tmp = _hais.Find(e => e.Name == hai.Name && e.Nakichoice);
                    if (tmp != null) { choice = tmp; }
                }
            }

            if (kouhos.Count == 1)
            {
                choice = kouhos[0];
            }

            if (choice != null)
            {
                Hai hai1 = null;
                Hai hai2 = null;
                Hai hai3 = null;
                Hai hai4 = null;

                int i = 0;
                for (; i < _hais.Count; i++) { if (choice.Name == _hais[i].Name) { hai1 = _hais[i]; i++; break; } }
                for (; i < _hais.Count; i++) { if (choice.Name == _hais[i].Name) { hai2 = _hais[i]; i++; break; } }
                for (; i < _hais.Count; i++) { if (choice.Name == _hais[i].Name) { hai3 = _hais[i]; i++; break; } }
                for (; i < _hais.Count; i++) { if (choice.Name == _hais[i].Name) { hai4 = _hais[i]; i++; break; } }

                if (hai1 != null && hai2 != null && hai3 != null && hai4 != null)
                {
                    Kan kan = new Kan(hai1, hai2, hai3, hai4, 0);
                    _kans.Add(kan);
                    _naki.Add(kan);
                    _hais.Remove(hai1);
                    _hais.Remove(hai2);
                    _hais.Remove(hai3);
                    _hais.Remove(hai4);
                    return true;
                }

                foreach (var pon in _pons)
                {
                    if (pon.IsCanKaKan(choice))
                    {
                        Kan kan = pon.KaKan(choice);
                        _kans.Add(kan);
                        int idx = _naki.FindIndex(e => e == pon);
                        _naki.Insert(idx, kan);
                        _hais.Remove(choice);
                        _pons.Remove(pon);
                        _naki.Remove(pon);
                        return true;
                    }
                }
            }

            foreach (var kouho in kouhos)
            {
                foreach (var hai in _hais)
                {
                    if (kouho.Name == hai.Name)
                    {
                        hai.Nakikouho = true;
                    }
                }
            }

            return false;
        }
    }
}
