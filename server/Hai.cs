using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Hai
    {
        public enum Type
        {
            Manzu,
            Souzu,
            Pinzu,
            Zihai,
        }

        public enum Number
        {
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,
            
            Ton=0,
            Nan,
            Sha,
            Pei,
            Hak,
            Hat,
            Thu,
        }

        Type type_;
        Number num_;

        public Hai(Type type,Number num)
        {
            this.type_ = type;
            this.num_ = num;
        }
    }
}
