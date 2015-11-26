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
                        fname = data[0].Substring(data[0].LastIndexOf("/") + 1);

                    String type;
                    if (data[0].IndexOf("event", StringComparison.OrdinalIgnoreCase) >= 0)
                        type = "Event";
                    else
                        type = "Bg";


                    bg.Filename = fname;
                    bg.Type = type;
                    bg.Label = fname.Split('.')[0];

                    curr.AddTo(Label.LabelTypes.Textures, bg);


                    //Now do the command for it
                    Command bgc = new Command();
                    bgc.Comm = "Bg";
                    bgc.Arg[0] = bg.Label;

                    curr.AddTo(Label.LabelTypes.Commands, bgc);

                }
                else if (line.StartsWith("wave", StringComparison.OrdinalIgnoreCase) && !line.StartsWith("waveloop", StringComparison.OrdinalIgnoreCase) && !line.StartsWith("wavestop", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a new Sound
                    Sound sd = new Sound();
                    String tline = line;
                    if (line.StartsWith("wave\"", StringComparison.OrdinalIgnoreCase))
                    {
                        tline = line.Substring(0, 4) + " \"" + line.Substring(5);
                    }

                    String[] data = tline.Split(' ').Select(d => d.Trim(new Char[] { ' ', '"' })).ToArray();

                    String fname = data[1].Substring(data[1].LastIndexOf("/") + 1);
                    String Label = fname.Split('.')[0];

                    sd.FileName = fname;
                    sd.Label = Label;
                    sd.Type = "Se";
                    sd.Title = Label;

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Sounds, sd);

                    //Now do the command for it
                    Command sdc = new Command();
                    sdc.Comm = "Se";
                    sdc.Arg[0] = sd.Label;

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, sdc);
                }
                else if (line.StartsWith("waveloop", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a looping wave (We'll use ambience for this!)
                    Sound sd = new Sound();
                    String[] data = line.Split(' ').Select(d => d.Trim(new Char[] { ' ', '"' })).ToArray();

                    String fname = data[1].Substring(data[1].LastIndexOf("/") + 1);
                    String Label = fname.Split('.')[0];

                    sd.FileName = fname;
                    sd.Label = Label;
                    sd.Type = "Ambience";
                    sd.Title = Label;

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Sounds, sd);

                    //Now do the command for it
                    Command sdc = new Command();
                    sdc.Comm = "Ambience";
                    sdc.Arg[0] = Label;
                    sdc.Arg[1] = "TRUE"; //loop

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, sdc);
                }
                else if (line.StartsWith("wavestop", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a command to stop the looping wave command
                    Command ws = new Command();

                    ws.Comm = "StopAmbience";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, ws);
                }
                else if (line.StartsWith("bgm", StringComparison.OrdinalIgnoreCase) && !line.StartsWith("bgmloop", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a BGM
                    Sound sd = new Sound();
                    String tline = line;
                    if (line.StartsWith("bgm\"", StringComparison.OrdinalIgnoreCase))
                    {
                        tline = line.Substring(0, 3) + " \"" + line.Substring(4);
                    }
                    String[] data = tline.Split(' ').Select(d => d.Trim(new Char[] { ' ', '"' })).ToArray();

                    String fname = data[1].Substring(data[1].LastIndexOf("/") + 1);
                    String Label = fname.Split('.')[0];

                    sd.FileName = fname;
                    sd.Label = Label;
                    sd.Type = "Bgm";
                    sd.Title = Label;

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Sounds, sd);

                    //Now do the command for it
                    Command sdc = new Command();
                    sdc.Comm = "Bgm";
                    sdc.Arg[0] = sd.Label;

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, sdc);
                }
                else if (line.StartsWith("bgmloop", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO

                }
                else if (line.StartsWith("stop", StringComparison.OrdinalIgnoreCase))
                { 
                    //Handle a command to stop all sound
                    Command ws = new Command();

                    ws.Comm = "StopSound";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, ws);

                }
                else if (line.StartsWith("`"))
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

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, wait);
                }
                else if (line.StartsWith("quake", StringComparison.OrdinalIgnoreCase))
                {

                }
                else
                {
                    //Handle something else that is probably not needed but probably will end up being useful
                }
            }

            //Now that our labels are full of information, write a folder for each one
            //TODO


            int DEBUG = 0;
        }
    }
}
