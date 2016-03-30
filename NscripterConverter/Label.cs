using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Label
    {
        public string Name;

        public enum LabelTypes { Characters, Textures, Sounds, Commands, Layers }

        protected List<Character> Characters = new List<Character>();
        protected List<Texture> Textures = new List<Texture>();
        protected List<Sound> Sounds = new List<Sound>();
        protected List<Command> Commands = new List<Command>();
        protected List<Layer> Layers = new List<Layer>();

        protected List<String> Unknown = new List<String>();

        public bool Left = false, Center = false, Right = false;

        public void AddTo(LabelTypes type, Object obj)
        {
            if (type == LabelTypes.Characters)
            {
                Character n = (Character)obj;

                bool found = false;
                foreach (Character ch in Characters)
                {
                    if (ch.CharacterName == n.CharacterName)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Characters.Add(n);
            }
            else if (type == LabelTypes.Textures)
            {
                Texture t = (Texture)obj;

                bool found = false;
                foreach (Texture te in Textures)
                {
                    if (te.Filename == t.Filename)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Textures.Add(t);
            }
            else if (type == LabelTypes.Sounds)
            {
                Sound s = (Sound)obj;
                bool found = false;
                foreach (Sound so in Sounds)
                {
                    if (so.FileName == s.FileName)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Sounds.Add(s);
            }
            else if (type == LabelTypes.Layers)
            {
                Layer l = (Layer)obj;
                bool found = false;
                foreach (Layer lo in Layers)
                {
                    if (lo.getLayerName() == l.getLayerName())
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    Layers.Add(l);
            }
            else if (type == LabelTypes.Commands)
                Commands.Add((Command)obj);
            else
                Unknown.Add((String)obj);

        }

        public bool isEmptySide(string pos)
        {
            if (pos.IndexOf("r", StringComparison.OrdinalIgnoreCase) >= 0)
                return !Right;
            else if (pos.IndexOf("c", StringComparison.OrdinalIgnoreCase) >= 0)
                return !Center;
            else
                return !Left;
        }

        public void resetSides()
        {
            Right = Center = Left = false;
        }


        public String GetLastCharacterNameAtPos(string pos)
        {
            if (pos.IndexOf("r", StringComparison.OrdinalIgnoreCase) >= 0)
                pos = "right";
            else if (pos.IndexOf("c", StringComparison.OrdinalIgnoreCase) >= 0)
                pos = "center";
            else
                pos = "left";

            //we need to find the last Character at the indicated position
            foreach (Command c in Enumerable.Reverse(Commands))
            {
                if (c.Arg[2] == pos)
                {
                    return c.Arg[0]; //Character Name
                }

            }

            return null;
        }

        public bool writeFiles(String dir)
        {
            bool wrote = false;
            StringBuilder sb = new StringBuilder();

            Characters = Characters.OrderBy(Ch => Ch.FileName).ToList();
            Sounds = Sounds.OrderBy(So => So.FileName).ToList();
            Textures = Textures.OrderBy(Te => Te.Filename).ToList();
            Layers = Layers.OrderBy(Lo => Lo.getLayerName()).ToList();

            foreach (Character c in Characters)
                sb.Append(c.ToString()).Append("\n");
            if (sb.Length > 0)
            {
                wrote = true;
                File.WriteAllText(dir + "/Characters.tsv", sb.ToString(), Encoding.UTF8);
            }

            sb.Clear();
           
            foreach (Command c in Commands)
                sb.Append(c.ToString()).Append("\n");
            if (sb.Length > 0)
            {
                wrote = true;
                File.WriteAllText(dir + "/Commands.tsv", sb.ToString(), Encoding.UTF8);
            }

            sb.Clear();

            foreach (Sound s in Sounds)
                sb.Append(s.ToString()).Append("\n");
            if (sb.Length > 0)
            {
                wrote = true;
                File.WriteAllText(dir + "/Sounds.tsv", sb.ToString(), Encoding.UTF8);
            }

            sb.Clear();

            foreach(Texture t in Textures)
                sb.Append(t.ToString()).Append("\n");
            if (sb.Length > 0)
            {
                wrote = true;
                File.WriteAllText(dir + "/Textures.tsv", sb.ToString(), Encoding.UTF8);
            }

            sb.Clear();

            foreach (Layer l in Layers)
                sb.Append(l.ToString()).Append("\n");
            if (sb.Length > 0)
            {
                wrote = true;
                File.WriteAllText(dir + "/Layers.tsv", sb.ToString(), Encoding.UTF8);
            }

            return wrote;
        }
    }
}
