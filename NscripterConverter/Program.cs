using System;
using System.IO;
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
            String TextColor = null;


            foreach (String line in lines)
            {
                if (line.StartsWith("*"))
                {
                    if (curr != null) //If the current label is not null make a pointer to the next label and add the current label
                    {
                        //make a jump command to the next label
                        Command jump = new Command();
                        jump.Comm = "Jump";
                        jump.Arg[0] = "*" + line.Substring(1);

                        curr.AddTo(Label.LabelTypes.Commands, jump);

                        //push the current label into the Labels list
                        Labels.Add(curr);

                    }

                    //otherwise we have our first label 
                    curr = new Label();
                    curr.Name = line.Substring(1);
                    continue;
                }


                if (curr == null)
                    throw new Exception("Trying to process commands without a label? That's a spanking");

                if (line.Trim().Length == 0)
                    continue; 

                //okay let's go down and check everything!

                if (line.StartsWith("waveloop", StringComparison.OrdinalIgnoreCase))
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
                else if (line.StartsWith("wavestop", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a command to stop the looping wave command
                    Command ws = new Command();

                    ws.Comm = "StopAmbience";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, ws);
                }
                else if (line.StartsWith("bgmloop", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO?
                    throw new Exception("Not implemented!");
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
                else if (line.StartsWith("bg", StringComparison.OrdinalIgnoreCase) && !line.StartsWith("bgm", StringComparison.OrdinalIgnoreCase))
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

                    //Background commands also clear any existing character sprites
                    Command coff = new Command();
                    coff.Comm = "CharacterOff";
                    coff.Arg[0] = ""; //Blank is all characters
                    coff.PageCtl = "Next";

                    curr.AddTo(Label.LabelTypes.Commands, coff);

                    curr.resetSides();

                    //They also remove any text, so page break
                    Command br = new Command();
                    br.Comm = "";
                    br.PageCtl = "Next";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, br);

                    //Now do the command for it
                    Command bgc = new Command();
                    bgc.Comm = "Bg";
                    bgc.Arg[0] = fname.Split('.')[0];
                    bgc.PageCtl = "Next";

                    if (data.Length > 2)
                        bgc.Effect = new Effect(data[1], data[2]);
                    else if (data.Length > 1)
                        bgc.Effect = new Effect(data[1]);

                    curr.AddTo(Label.LabelTypes.Commands, bgc);

                    
                }
                else if (line.StartsWith("stop", StringComparison.OrdinalIgnoreCase))
                { 
                    //Handle a command to stop all sound
                    Command ws = new Command();

                    ws.Comm = "StopSound";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, ws);

                }
                else if (line.StartsWith("ld"))
                {
                    //Handle a character load
                    Character cl = new Character();
                    String[] data = line.Split(',').Select(d => d.Trim(new Char[] { ' ', '"' })).ToArray();

                    String fname = data[1].Substring(data[1].LastIndexOf("/") + 1);
                    String cname = fname.Split('.')[0];

                    cl.FileName = fname;
                    cl.CharacterName = cname;
                    cl.Pattern = ""; //TODO?: Split Cname into multiple patterns

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Characters, cl);

                    //Get the Layer Ready
                    Layer CLayer = new Layer();
                    CLayer.CharacterName = cname;

                    //Set this up BEFORE we add the command so we don't blow away the character right after showing them!
                    Command coff = new Command();
                    coff.Comm = "CharacterOff";
                    coff.Arg[0] = curr.GetLastCharacterNameAtPos(data[0]);

                    //Character Off previous CHARACTER BEFORE adding the layer later on...
                    if (!curr.isEmptySide(data[0])) 
                        curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, coff);

                    //Now do the command for it
                    Command spr = new Command();

                    spr.Comm = "";
                    spr.Arg[0] = cname;
                    if (data[0].IndexOf("r", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        curr.Right = true;

                        CLayer.LayerType = Layer.LayerTypes.Right;
                        CLayer.x = "500";

                    }
                    else if (data[0].IndexOf("c", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        curr.Center = true;
                        
                        CLayer.LayerType = Layer.LayerTypes.Center;
                        CLayer.x = "0";
                    }
                    else
                    {
                        curr.Left = true;

                        CLayer.LayerType = Layer.LayerTypes.Left;
                        CLayer.x = "-500";
                    }

                    if (data.Length > 3)
                        spr.Effect = new Effect(data[2], data[3]);
                    else if (data.Length > 2)
                        spr.Effect = new Effect(data[2]);

                    spr.Arg[2] = CLayer.getLayerName();

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, spr);
                    
                    curr.AddTo(NscripterConverter.Label.LabelTypes.Layers, CLayer);


                }
                else if(line.StartsWith("click"))
                {
                    Command wait = new Command();

                    wait.Comm = "";
                    wait.PageCtl = "Input";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, wait);
                }
                else if (line.StartsWith("cl"))
                {
                    //Handles a clear at a certain position
                    Command cl = new Command();
                    String[] data = line.Split(',').Select(d => d.Trim()).ToArray();

                    cl.Comm = "CharacterOff";
                    //Utage wants a character name so we need to get the last character name

                    String pos = data[0].Substring(3);

                    if (pos.StartsWith("a", StringComparison.OrdinalIgnoreCase))
                        cl.Arg[0] = "";
                    else
                        cl.Arg[0] = curr.GetLastCharacterNameAtPos(pos);

                    //Effect
                    if (data.Length > 2)
                        cl.Effect = new Effect(data[1], data[2]);
                    else if (data.Length > 1)
                        cl.Effect = new Effect(data[1]);

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, cl);

                    curr.resetSides(); 
                   

                }
                else if (line.StartsWith("`"))
                {
                    //Handle a text command(s)
                    String[] data = line.Trim('`').Split('@').ToArray();

                    bool forcenext = false;
                    foreach (String text in data)
                    {
                       if(text.Trim().Length == 0)
                           continue; //ignore whitespace junk

                        Command nu = new Command();
                        nu.Comm = "";
                        if (TextColor != null)
                        {
                            nu.Text = "<color=" + TextColor + "ff>" + text + "</color>";
                            //Console.WriteLine("Coloring line " + text);
                        }
                        else
                            nu.Text = text;

                        if (text.IndexOf("\\") >= 0)
                        {
                            nu.Text = nu.Text.Trim('\\');
                            nu.PageCtl = "";
                        }
                        else if (text.IndexOf("/") >= 0)
                        {
                            nu.Text = nu.Text.Trim('/');
                            nu.PageCtl = "Next";
                            forcenext = true;
                        }
                        else
                            nu.PageCtl = "Input";

                        curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, nu);
                    }

                    TextColor = null; //FTFM - "Note that this only changes the next text display, and not any of the ones before or after, so be careful."

                    if (!forcenext)
                    {
                        //at the VERY END we want to add a BR since that's how Nscripter does it
                        Command br = new Command();
                        br.Comm = "";
                        br.PageCtl = "InputBr";

                        curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, br);
                    }
                }
                else if (line.StartsWith("monocro", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO: REVISIT
                    Command mono = new Command();
                    String[] data = line.Split(' ').Select(d => d.Trim()).ToArray();

                    mono.Comm = "Monocro";
                    mono.Arg[0] = data[1];

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, mono);

                }
                else if (line.StartsWith("wait", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a wait command
                    Command wait = new Command();
                    String[] data = line.Split(' ').Select(d => d.Trim()).ToArray();

                    wait.Comm = "Wait";
                    wait.Arg[5] = (Single.Parse(data[1]) / 1000f).ToString(); //converts milliseconds to seconds

                    wait.PageCtl = "Next";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, wait);
                }
                else if (line.StartsWith("quake", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle a quake command. TODO: Revisit this after testing
                    Command quake = new Command();
                    String[] data = line.Split(' ').Select(d => d.Trim()).ToArray();
                    String[] exargs = data[1].Split(',').Select(d => d.Trim()).ToArray();

                    quake.Comm = "Shake";
                    quake.Arg[0] = "Graphics"; //ARG1

                    if (data.Length == 3) //annoying space threw off the parser
                    {
                        exargs[0] = data[1].Trim(',');
                        exargs[1] = data[2];
                    }

                    String param = "time=" + (Single.Parse(exargs[1]) / 1000f).ToString() + " "; //Convert milliseconds to seconds
                    if (data[0].IndexOf("x", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        param += "x=" + (10 * Int32.Parse(exargs[0])).ToString() + " ";
                    }
                    else if (data[0].IndexOf("y", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        param += "y=" + (10 * Int32.Parse(exargs[0])).ToString() + " ";
                    }
                    else
                    {
                        param += "x=" + (10 * Int32.Parse(exargs[0])).ToString() + " ";
                        param += "y=" + (10 * Int32.Parse(exargs[0])).ToString() + " ";
                    }

                    quake.Arg[2] = param; //ARG3
                    quake.Arg[5] = ""; //blank on purpose

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, quake);
                }
                else if (line.StartsWith("br", StringComparison.OrdinalIgnoreCase))
                {
                    //Handle linefeed command
                    Command br = new Command();
                    br.Comm = "";
                    br.PageCtl = "InputBr";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, br);
                }
                else if (line.StartsWith("#"))
                {
                    //Change the color of the next text command
                    TextColor = line;
                    //Console.WriteLine("Next line is colored " + line);
                }
                else if (line.StartsWith("@"))
                {
                    //Handle enter click wait state
                    Command com = new Command();
                    com.Comm = "";
                    com.PageCtl = "Input";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, com);
                }
                else if (line.StartsWith(@"\"))
                {
                    //Handle end-of-page wait
                    Command com = new Command();
                    com.Comm = "";
                    com.Text = " "; //Appears to be required as the whole "row" can't be blank
                    com.PageCtl = "";

                    curr.AddTo(NscripterConverter.Label.LabelTypes.Commands, com);
                }
                else
                {
                    //Handle something else that is probably not needed but probably will end up being useful
                    Console.WriteLine(line);
                }
            }

            if (curr != null) //Add in our last label you IMBECILE
                Labels.Add(curr);

            //Now that our labels are full of information, write a folder for each one
            Console.WriteLine(Effect.getTotalCalls());
            Console.Beep();
            Console.WriteLine("All Done!");
            Console.ReadKey();
            Console.WriteLine("Preparing to dump...");
            Console.Beep();

            String dir = @"C:\Users\Rob\Desktop\Wish Conversion\Test1_es\";

            foreach (Label lab in Labels)
            {
                String dirpath = dir + lab.Name;
                Directory.CreateDirectory(dirpath);
                bool wrote = lab.writeFiles(dirpath);
                if (wrote)
                    Console.WriteLine(dirpath + "\t has data!");
                else
                    Directory.Delete(dirpath);
            }

            Console.Beep();
            Console.WriteLine("...Done");
            Console.ReadKey();


        }
    }
}
