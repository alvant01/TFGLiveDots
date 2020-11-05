using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleScore : BrailleElement
    {
        public List<BraillePart> Parts { get; set; }

        public BrailleScore()
        {
            Parts = new List<BraillePart>();
        }

        public BrailleScore(List<BraillePart> p)
        {
            Parts = p;
        }

        /* public void AppendParts(List<BraillePart> p)
         {
             Parts = Parts.Add(p);

         }*/
        BraillePart part;
        BrailleScore(BraillePart p)
        {
            part = p;
        }

        public void Parse(BrailleText brailleText)
        {
            foreach (BraillePart P in Parts)
            {
                P.Parse(brailleText);
            }
            brailleText.AddScoreEnd();
        }

        internal void ParseBraille(List<char> content)
        {
            BraillePart p = new BraillePart();

            p.ParseBraille(content);
        }
    }
}
