using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NscripterConverter
{
    class Label
    {
        public string Name;

        public enum LabelTypes { Characters, Textures, Sounds, Commands }

        protected List<Character> Characters = new List<Character>();
        protected List<Texture> Textures = new List<Texture>();
        protected List<Sound> Sounds = new List<Sound>();
        protected List<Command> Commands = new List<Command>();

        protected List<String> Unknown = new List<String>();

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
            else if (type == LabelTypes.Commands)
                Commands.Add((Command)obj);
            else
                Unknown.Add((String)obj);

        }
  
    }
}
