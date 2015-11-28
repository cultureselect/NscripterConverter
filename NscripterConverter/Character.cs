using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Character
    {
        public String CharacterName;
        //1 tab
        public String Pattern;
        //3 tabs
        public String FileName;

        public Effect Effect;


        public override String ToString()
        {
            return CharacterName + "\t" + "\t" + Pattern + "\t" + "\t\t\t" + FileName + "\t" + "\t\t" + Effect;
        }


    }

    
}
