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

            //if (Clef != null)
            //    Clef.Parse(brailleText);
        }
    }
}
