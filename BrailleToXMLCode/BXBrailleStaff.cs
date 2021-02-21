using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDots
{
    enum BXHand
    {
        Left, Right
    }
    public class BXBrailleStaff : BXBrailleElement
    {
        public int Id { get; set; }

        public List<BXBrailleVoice> Voices { get; set; }

        public BXBrailleStaff()
        {
            Voices = new List<BXBrailleVoice>();
        }

        public BXBrailleStaff(List<BXBrailleVoice> v, int h)
        {
            Voices = v;
            Id = h;
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
