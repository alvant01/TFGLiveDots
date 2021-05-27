using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDots
{
    public class BrailleStaff : BrailleElement
    {
        public int Id { get; set; }

        public List<BrailleVoice> Voices { get; set; }

        public BrailleStaff()
        {
            Voices = new List<BrailleVoice>();
        }

        public void Parse(BrailleText brailleText)
        {
            foreach (BrailleVoice V in Voices)
            {
                V.Parse(brailleText);
            }
        }
    }
}
