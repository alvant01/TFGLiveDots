using System;

namespace LiveDots
{
    public class BraillePitch : BrailleElement
    {
        public string step { get; set; }
        public int octave { get; set; }

        public void Parse(BrailleText brailleText) { }
    }

}
