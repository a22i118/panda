using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Yama
    {
        List<Hai> _list = new List<Hai>();
        public List<Hai> List { get { return _list; } }

        public Yama() { }

        public void Init()
        {
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < 3)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            _list.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            _list.Add(new Hai((Hai.eType)i, (Hai.eNumber)j));
                        }
                    }
                }
            }

            // シャッフルする
            Shuffle();
        }

        // ツモ
        public Hai Tsumo()
        {
            if (_list.Count <= 0)
                return null;

            Hai tsumo = _list[0];
            _list.Remove(tsumo);
            return tsumo;
        }

        // 嶺上ツモ
        public Hai RinshanTsumo()
        {
            Hai tsumo = _list.Last();
            _list.Remove(tsumo);
            return tsumo;
        }

        /// <summary>
        /// シャッフルする関数
        /// </summary>
        public void Shuffle()
        {
            for (int i = _list.Count - 1; i > 0; i--)
            {
                var rand = new Random();
                var x = rand.Next(0, i + 1);
                var a = _list[i];
                _list[i] = _list[x];
                _list[x] = a;
            }
        }

        // 積み込み
        public void Tsumikomi(int player, Hai.eName[] array)
        {
            List<Hai> list = new List<Hai>();

            for (int i = 0; i < array.Length; i++)
            {
                Hai hai = _list.FindLast(e => e.Name == array[i]);

                if (hai != null)
                {
                    list.Add(hai);
                    _list.Remove(hai);
                }
            }

            _list.InsertRange(player * 13, list);
        }
    }
}
