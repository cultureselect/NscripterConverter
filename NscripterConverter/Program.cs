using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //EN
            String[] lines = System.IO.File.ReadAllLines(@"C:\Users\Rob\Desktop\Wish Conversion\DoesTheThing\0.txt", Encoding.Default);

            //ES
            //String[] lines = System.IO.File.ReadAllLines(@"C:\Users\Rob\Desktop\Wish Conversion\DoesTheThing\Wish_1P_Trans_20151012_Spanish Translation_UTF8.txt", Encoding.UTF8);

            List<Label> Labels = new List<Label>();

            Label curr = null;

            foreach (String line in lines)
            {
                if (line.StartsWith("*"))
                {
                    //Create a new Label and push the current label into the Labels list
                    if (curr != null) //no label no add
                        Labels.Add(curr);
                    curr = new Label();
                    curr.Name = line.Substring(1);
                    continue;
                }

                if (curr == null)
                    throw new Exception("wtf?");

                //okay let's go down and check everything!

                if (line.StartsWith("bg", StringComparison.OrdinalIgnoreCase) && !line.StartsWith("bgm", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a background
                    Texture bg = new Texture();
                    String[] data = line.Split(',').Select(d => d.Trim(new Char[] { ' ', '"' } )).ToArray();

                    String fname;
                    data[0] = data[0].Substring(2).Trim();
                    if (data[0].Equals("Black", StringComparison.OrdinalIgnoreCase) || data[0].Equals("White", StringComparison.OrdinalIgnoreCase))
                        fname = data[0];
                    else if(data[0].Contains("$") || data[0].Contains("#")) //annoying things
                        fname = data[0];
                    else
                        fname = data[0].Substring(data[0].LastIndexOf("/"));

                    String type;
                    if (data[0].IndexOf("event", StringComparison.OrdinalIgnoreCase) >= 0)
                        type = "Event";
                    else
                        type = "Bg";


                    bg.Filename = fname;
                    bg.Type = type;
                    bg.Label = fname.Split('.')[0];

                    curr.AddTo(Label.LabelTypes.Textures, bg);
                }
                else if (line.StartsWith("wave", StringComparison.OrdinalIgnoreCase) || line.StartsWith("bgm", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a new Sound

                    //TODO

                }
                else if(line.StartsWith("`"))
                {
                    //Handle a text command
                    Command text = new Command();

                    //TODO

                }
                else if (line.StartsWith("wait", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a wait command
                    Command wait = new Command();
                    String[] data = line.Split(' ').Select(d => d.Trim()).ToArray();

                    wait.Comm = "Wait";
                    wait.Arg[5] = (Int32.Parse(data[1]) / 1000).ToString(); //converts milliseconds to seconds

                    curr.AddTo(Label.LabelTypes.Commands, wait);
                }
                else
                {
                    //Handle something else that is probably not needed
                }
            }

            //Now that our labels are full of information, write a folder for each one
            //TODO


            int DEBUG = 0;
        }
    }
}
