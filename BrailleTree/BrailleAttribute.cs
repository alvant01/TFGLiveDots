using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleAttribute : BrailleElement
    {
        public bool First;
        public BrailleKey Key { get; set; }
        public BrailleTime Time { get; set; }
        public BrailleClef Clef { get; set; }

        public BrailleAttribute() { }

        public void Parse(BrailleText brailleText)
        {
            if (!First) brailleText.JumpLine();

            brailleText.AddSpace(4);

            if (Clef != null)
                Clef.Parse(brailleText);
            if (Key != null)
            {
                Key.Parse(brailleText);
                brailleText.AddSpace();
            }
            if (Time != null)
            {
                Time.Parse(brailleText);
                brailleText.AddSpace();
            }

            brailleText.JumpLine();
        }

        public Attribute ParseBraille(List<char> content)
        {
            Attribute res = null;
            //Quito los 4 espacios

            //Quito )

            //asigno clef
            Clef.ParseBrailleInverse(content[0], content[1]);
            content.RemoveRange(0,2);
            if (content[0] == '#')
            {
                content.RemoveAt(0);
                Clef.ParseBrailleInverseOcho(content[0]);
                content.RemoveAt(0);
            }
            //asigno Key
            if (content[0] != ' ')
            {
                Key.ParseBrailleInverse(content[0], content[1]);
            }
            else
                content.RemoveAt(0);
            //asigno Time
            Time.ParseBrailleInverse(content[0], content[1], content[2]);

            return res;
        }
    }
}
