using System.Collections.Generic;
using System.Linq;

namespace LiveDots
{
    enum Hand
    {
        Left, Right
    }
    public class BrailleStaff : BrailleElement
    {
        public int Id { get; set; }

        public List<BrailleVoice> Voices { get; set; }

        public BrailleStaff()
        {
            Voices = new List<BrailleVoice>();
        }

        public BrailleStaff(List<BrailleVoice> v, int h)
        {
            Voices = v;
            Id = h;
        }

        public void AppendVoices(List<BrailleVoice> v)
        {
            Voices = Voices.Concat(v).ToList();
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
