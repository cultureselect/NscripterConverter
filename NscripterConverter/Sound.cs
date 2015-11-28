using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Sound
    {
        public String Label;
        public String Title;
        public String Type;
        public String FileName;

        public override String ToString()
        {
            return Label + "\t" + Title + "\t" + Type + "\t" + FileName;
        }
    }
}
