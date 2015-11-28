using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Command
    {
        public String Comm;
        public String[] Arg = new String[6];
        public String Text;
        public String PageCtl;

        public Effect Effect;

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Comm).Append("\t");
            foreach (String a in Arg)
                sb.Append(a ?? "").Append("\t");
            sb.Append(Text).Append("\t");
            sb.Append(PageCtl).Append("\t");
            sb.Append("\t\t\t");
            sb.Append(Effect);

            return sb.ToString();
        }
    }
}
