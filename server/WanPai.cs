using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class WanPai
    {
        List<Hai> list_ = new List<Hai>();
        public List<Hai> List { get { return list_; } }
        public WanPai() { }


        public void Add(Hai hai)
        {
            list_.Add(hai);
        }


    }
}
