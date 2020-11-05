using System;

namespace LiveDots
{
    public class BraillePitch : BrailleElement
    {
        public string step { get; set; }
        public int octave { get; set; }

        public BraillePitch(string st, int oct)
        {
            step = st;
            octave = oct;
        }

        public void Parse(BrailleText brailleText) { }
        public void ParseBraille(BrailleText brailleText)
        {
            throw new NotImplementedException();
        }

        public void ParseText(BrailleMusicViewer viewer)
        {
            throw new NotImplementedException();
        }
    }

}
