﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Koutsu
    {
        private Hai[] hais =new Hai[3];
        public Koutsu(Hai hai0,Hai hai1,Hai hai2) { 
            hais[0] = hai0;
            hais[1] = hai1;
            hais[2] = hai2;
        }
    }
}
