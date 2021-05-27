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

        /* public void AppendParts(List<BraillePart> p)
         {
             Parts = Parts.Add(p);

         }*/

        public void Parse(BrailleText brailleText)
        {
            foreach (BraillePart P in Parts)
            {
                P.Parse(brailleText);
            }
            brailleText.AddScoreEnd();
        }
    }
}
