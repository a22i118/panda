using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    abstract class INaki
    {
        protected int _from = 0;

        public abstract int Draw(Graphics g, int x, int y);
    }
}
