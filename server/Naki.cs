using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static server.Hai;

namespace server
{
    abstract class INaki
    {
        protected int _from = 0;

        protected eState _state_and = 0;
        protected eState _state_or = 0;

        public eState StateAnd { get { return _state_and; } }
        public eState StateOr { get { return _state_or; } }

        public abstract int Draw(Graphics g, int x, int y);
    }
}
