﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Texture
    {
        public String Label;
        public String Type;
        //Three tabs inbetween
        public String Filename;

        public override String ToString()
        {
            return Label + "\t" + Type + "\t" + "\t\t\t" + Filename;
        }
    }
}
