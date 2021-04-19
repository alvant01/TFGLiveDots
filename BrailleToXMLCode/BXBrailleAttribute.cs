using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBrailleAttribute : BrailleElement
    {
        public bool First;
        public BXBrailleKey Key { get; set; }
        public BXBrailleTime Time { get; set; }
        public BXBrailleClef Clef { get; set; }

        public BXBrailleAttribute() { }

        public BXBrailleAttribute Parse(List<char> content, BrailleText brailleText)
        {

            content.RemoveRange(0, 5);
            brailleText.AddSpace(4);
            BrailleText brailleTextAux  = new BrailleText();
            //asigno clef
            Clef = new BXBrailleClef();
            int num = Clef.ParseBrailleInverse(brailleTextAux, content[0], content[1], content[2], content[3], content[4]);
            content.RemoveRange(0, num);

            //asigno Key
            Key = new BXBrailleKey();
           
            int numDelete = Key.Parse(brailleText, content[0], content[1], content[2]);
            content.RemoveRange(0, numDelete);
            brailleText.AddSpace();

            //asigno Time
            Time = new BXBrailleTime();
            numDelete = Time.Parse(brailleText, content);
            content.RemoveRange(0, numDelete);
            brailleText.AddSpace();
            brailleText.JumpLine();

            brailleText.concatenate(brailleTextAux);

            return this;
        }

        public void Parse(BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
    }
}
