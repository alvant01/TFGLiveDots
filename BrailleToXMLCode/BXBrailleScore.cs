using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBrailleScore : BXBrailleElement
    {
        public List<BXBraillePart> Parts { get; set; }

        public BXBrailleScore()
        {
            Parts = new List<BXBraillePart>();
        }

        public BXBrailleScore(List<BXBraillePart> p)
        {
            Parts = p;
        }

        BXBraillePart part;
        BXBrailleScore(BXBraillePart p)
        {
            part = p;
        }

        internal void Parse(List<char> content, BrailleText brailleText)
        {
            BXBraillePart p = new BXBraillePart();

            p.Parse(content, brailleText);
            brailleText.AddScoreEnd();
            this.Parts.Add(p);
        }

        void BXBrailleElement.Parse(List<char> content, BrailleText brailleText)
        {
        }
    }
}
