using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Layer
    {
        public enum LayerTypes { Left, Center, Right}

        public LayerTypes LayerType;
        public String CharacterName; 
        public String Type = "Character";
        public String x = "0";
        public String y = "0";
        public String order = "20";

        public String getLayerName()
        {
            return getLayerTypeName() + "_" + CharacterName;
        }

        private String getLayerTypeName()
        {
            if (LayerType == LayerTypes.Left)
                return "Left";
            else if (LayerType == LayerTypes.Center)
                return "Center";
            else
                return "Right";
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.getLayerName());
            sb.Append(",");
            sb.Append(Type);
            sb.Append(",");
            sb.Append(x);
            sb.Append(",");
            sb.Append(y);
            sb.Append(",");
            sb.Append(order);

            return sb.ToString();
        }


    }
}
