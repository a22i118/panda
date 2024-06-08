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
        List<Hai> _rinshams = new List<Hai>();
        List<Hai> _doras = new List<Hai>();

        public WanPai() { }
        public void Init(Yama yama)
        {
            _rinshams.Clear();
            _doras.Clear();

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
        }

        public void Add(Hai hai) { _doras.Add(hai); }

        public Hai Tsumo()
        {
            if (_rinshams.Count <= 0)
                return null;

            Hai tsumo = _rinshams[0];
            _rinshams.Remove(tsumo);
            return tsumo;
        }
    }
}
