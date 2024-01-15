using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Kan
    {
        private Hai[] hais = new Hai[4];
        public Kan(Hai hai0, Hai hai1, Hai hai2, Hai hai3)
        {
            hais[0] = hai0;
            hais[1] = hai1;
            hais[2] = hai2;
            hais[3] = hai3;
        }
    }
}
