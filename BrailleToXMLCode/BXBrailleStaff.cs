using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDots
{
    public class BXBrailleStaff : BXBrailleElement
    {
        public int Id { get; set; }

        public List<BXBrailleVoice> Voices { get; set; }

        public BXBrailleStaff()
        {
            Voices = new List<BXBrailleVoice>();
        }

        public void Parse(List<char> content, BrailleText brailleText)
        {
            //Hacer bucle
            BXBrailleVoice voice = new BXBrailleVoice();

            voice.Parse(content, brailleText);
            this.Voices.Add(voice);
        }
    }
}
