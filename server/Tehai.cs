using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Tehai
    {
        List<Hai> list = new List<Hai>();

        public Tehai(){ }
            
        
        public void Add(Hai hai)
        {
            list.Add(hai);
        }

        public void Sort() {
            list.Sort((a,b) => (int)a.Name- (int)b.Name);
        }
    }
}
