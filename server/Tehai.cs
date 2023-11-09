using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Tehai
    {
        List<Hai> list_ = new List<Hai>();
        public List<Hai> List { get { return list_; } }
        public Tehai(){ }
            
        
        public void Add(Hai hai)
        {
            list_.Add(hai);
        }

        public void Sort() {
            list_.Sort((a,b) => (int)a.Name- (int)b.Name);
        }
    }
}
