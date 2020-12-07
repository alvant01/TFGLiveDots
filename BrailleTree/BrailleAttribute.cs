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

        public BrailleAttribute ParseBraille(List<char> content, BrailleText brailleText)
        {

            content.RemoveRange(0, 5);
            brailleText.AddSpace(4);

            //asigno clef
            Clef = new BrailleClef();
            int num = Clef.ParseBrailleInverse(brailleText, content[0], content[1], content[2], content[3], content[4]);
            content.RemoveRange(0, num);

            //asigno Key
            Key = new BrailleKey();
            int numDelete = Key.ParseBrailleInverse(brailleText, content[0], content[1], content[2]);
            content.RemoveRange(0, numDelete);
            brailleText.AddSpace();

            //asigno Time
            Time = new BrailleTime();
            numDelete = Time.ParseBrailleInverse(brailleText, content);
            content.RemoveRange(0, numDelete);
            brailleText.AddSpace();
            brailleText.JumpLine();

            return this;
        }
    }
}
