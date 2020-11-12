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

        public Attribute ParseBraille(List<char> content, BrailleText brailleText)
        {
            Attribute res = null;
            //Quito los 4 espacios

            //Quito )

            //asigno clef
            int num = Clef.ParseBrailleInverse(brailleText, content[0], content[1], content[2], content[3], content[4]);

            content.RemoveRange(0, num);

            //asigno Key
            if (content[0] != ' ')
            {
                int numDelete = Key.ParseBrailleInverse(brailleText, content[0], content[1], content[2]);
            }
            else
                content.RemoveAt(0);
            //asigno Time
            Time.ParseBrailleInverse(brailleText, content[0], content[1], content[2]);

            return res;
        }
    }
}
