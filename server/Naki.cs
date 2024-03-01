using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;
using static server.Yaku;

namespace server
{
    abstract class INaki : IMentsu
    {
        protected int _from = 0;

        public override eMachi Machi(Hai hai) { return eMachi.None; }

        public abstract int Draw(Graphics g, int x, int y);
    }
}
