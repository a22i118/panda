using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Chi
    {
        private Hai[] _hais = new Hai[3];

        public Hai[] Hais { get { return _hais; } }

        public Chi(Hai hai0, Hai hai1, Hai hai2)
        {
            hai0.ResetChi();
            hai1.ResetChi();
            hai2.ResetChi();
            _hais[0] = hai0;
            _hais[1] = hai1;
            _hais[2] = hai2;
        }
    }
}
