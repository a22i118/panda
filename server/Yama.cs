using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Yama
    {
        private List<Hai> _hais = new List<Hai>();
        public List<Hai> Hais { get { return _hais; } }

        // 牌の種類 * 4枚 - 配牌 * 4人 - 王牌 = 70
        private const int c_yamaMax = (9 * 3 + 7) * 4 - 13 * 4 - 14;

        public int TsumoCount { get { return c_yamaMax - _hais.Count; } }

        public Yama() { }

        public void Init()
        {
            _hais.Clear();

            for (int i = 0; i < 4; i++)
            {
                foreach (var haiState in Hai.sHaiStates)
                {
                    _hais.Add(new Hai(haiState));
                }
            }

            //for (int k = 0; k < 4; k++)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        if (i < 3)
            //        {
            //            for (int j = 0; j < 9; j++)
            //            {
            //                _hais.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));
            //            }
            //        }
            //        else
            //        {
            //            for (int j = 0; j < 7; j++)
            //            {
            //                _hais.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));
            //            }
            //        }
            //    }
            //}

            // シャッフルする
            Shuffle();
        }

        // ツモ
        public Hai Tsumo()
        {
            if (_hais.Count <= 0)
                return null;

            Hai tsumo = _hais[0];
            _hais.Remove(tsumo);
            return tsumo;
        }

        // 嶺上ツモ
        public Hai RinshanTsumo()
        {
            Hai tsumo = _hais.Last();
            _hais.Remove(tsumo);
            return tsumo;
        }

        /// <summary>
        /// シャッフルする関数
        /// </summary>
        public void Shuffle()
        {
            for (int i = _hais.Count - 1; i > 0; i--)
            {
                var rand = new Random();
                var x = rand.Next(0, i + 1);
                var a = _hais[i];
                _hais[i] = _hais[x];
                _hais[x] = a;
            }
        }

        // 積み込み
        public void Tsumikomi(int player, Hai.eName[] array)
        {
            List<Hai> list = new List<Hai>();

            for (int i = 0; i < array.Length; i++)
            {
                Hai hai = _hais.FindLast(e => e.Name == array[i]);

                if (hai != null)
                {
                    list.Add(hai);
                    _hais.Remove(hai);
                }
            }

            _hais.InsertRange(player * 13, list);
        }
    }
}
