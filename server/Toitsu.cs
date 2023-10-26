using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Toitsu
    {
        private Hai[] hais = new Hai[2];
        public Toitsu(Hai hai0, Hai hai1)
        {
            hais[0] = hai0;
            hais[1] = hai1;
        }
    }
}
