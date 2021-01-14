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

        public BraillePart(List<BrailleMeasure> m)
        {
            Measures = m;
        }

        /*public void AppendMeasures(BrailleMeasure m)
        {
            Measures = Measures.Concat(m).ToArray();
        }*/

        public void Parse(BrailleText brailleText)
        {
            int i = 0;
            foreach (BrailleMeasure M in Measures)
            {
                M.Parse(brailleText);
                i++;
            }

        }

        internal void ParseBraille(List<char> content,BrailleText brailleText)
        {
            //Hacer bucle

            BrailleMeasure m = new BrailleMeasure();
            this.Measures.Add(m);
            Object o = Measures[0].ParseBraille(content, brailleText);
            //content.RemoveAt(0);

            for (int i=1; content.Count != 0; i++)
            {
                m = new BrailleMeasure(o);
                this.Measures.Add(m);
                Measures[i].ParseBraille(content, brailleText);
               // if(content.Count != 0) content.RemoveAt(0);
            }
        }
    }
}
