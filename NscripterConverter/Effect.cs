using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    /*
     * 
     * 1   Instantaneous display. No runtime variable needed.
     * 2   Left-sided shutter
     * 3   Right-sided shutter
     * 4   Upwards shutter
     * 5   Downwards shutter
     * 6   Left-sided curtain
     * 7   Right-sided curtain
     * 8   Upwards curtain
     * 9   Downwards curtain
     * 10  Pixelwise crossfade
     * 11  Left-sided scroll
     * 12  Right-sided scroll
     * 13  Upwards scroll
     * 14  Downwards scroll
     * 15  Fade via mask pattern. You must set a filename with this, pointing to a mask bmp (of either 256 colors or full color). In this mask bmp, the white areas fade slowly, and the black areas fade quickly.
     * 16  Mosaic out. After this effect is called, the state of the screen will be as uncertain as if you used Effect Number 0, so please call a display command, like print, immediately afterwards.
     * 17  Mosaic in
     * 18  Crossfade via mask. This works similarly to 15, except it is far more processor intensive, so do use this effect with care.
     * 
     */


    class Effect
    {
        protected int Number; //only used if 'effect' is called raw. Effects in other commands do not seem to use this
        protected int Index;
        protected int Runtime; //in milliseconds
        protected String PatternFileName;

        protected static int[] TOTAL_CALLS = new int[17];

        public Effect(int num, int ind, int runt = 0, String pfn = null)
        {
            Number = num;
            Index = ind;
            Runtime = runt;
            PatternFileName = pfn;

            TOTAL_CALLS[Index]++;
        }
               
        public Effect(params String[] data)
        {
            //parse data to determine correctness
            int ei = Int32.Parse(data[0]);

            TOTAL_CALLS[ei]++;

            if (ei == 1)
            {
                Number = -1;
                Index = ei;
                Runtime = 0;
                PatternFileName = null;
                return;
            }

            int runt = Int32.Parse(data[1]);

            String pfn = null;
            try
            {
                pfn = data[2];
            }
            catch (Exception e)
            { }; //don't care

            Number = -1;
            Index = ei;
            Runtime = runt;
            PatternFileName = null;

        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Number).Append("\t").Append(Runtime);

            return sb.ToString();
        }

        public static String getTotalCalls()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Total Effect Calls:");
            for (int i = 0; i < TOTAL_CALLS.Length; i++)
                sb.Append(i + ": " + TOTAL_CALLS[i] + "\n");

            return sb.ToString();
        }
    }
}
