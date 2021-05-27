using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BraillePart : BrailleElement
    {
        public List<BrailleMeasure> Measures { get; set; }

        public BraillePart()
        {
            Measures = new List<BrailleMeasure>();
        }

        /*public void AppendMeasures(BrailleMeasure m)
        {
            Measures = Measures.Concat(m).ToArray();
        }*/

        public void Parse(BrailleText brailleText)
        {
            foreach (BrailleMeasure M in Measures)
            {
                M.Parse(brailleText);
            }

        }

        
    }
}
