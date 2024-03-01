using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    abstract class INaki : IMentsu
    {
        protected int _from = 0;

        public abstract int Draw(Graphics g, int x, int y);
    }
}
