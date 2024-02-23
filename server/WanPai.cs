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
        List<Hai> _list = new List<Hai>();

        public WanPai() { }

        public void Init() { _list.Clear(); }

        public void Add(Hai hai) { _list.Add(hai); }
    }
}
