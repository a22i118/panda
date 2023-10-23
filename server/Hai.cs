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
            Pinzu,
            Souzu,
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
            Haku,
            Hatu,
            Thun,
        }

        public enum eName
        {
            Manzu1,
            Manzu2, 
            Manzu3,
            Manzu4,
            Manzu5, 
            Manzu6,
            Manzu7,
            Manzu8,
            Manzu9,

            Pinzu1,
            Pinzu2,
            Pinzu3,
            Pinzu4,
            Pinzu5,
            Pinzu6,
            Pinzu7,
            Pinzu8,
            Pinzu9,

            Souzu1,
            Souzu2,
            Souzu3,
            Souzu4,
            Souzu5,
            Souzu6,
            Souzu7,
            Souzu8,
            Souzu9,

            Ton,
            Nan,
            Sha,
            Pei,
            Haku,
            Hatu,
            Thun,
        }

        Type type_;
        Number num_;

        public Hai(Type type,Number num)
        {
            this.type_ = type;
            this.num_ = num;
        }

        public eName Name
        {
            get { return (eName)((int)type_ * 9 + (int)num_); }
        }
    }
}
