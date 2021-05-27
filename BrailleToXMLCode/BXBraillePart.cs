using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBraillePart : BXBrailleElement
    {
        public List<BXBrailleMeasure> Measures { get; set; }

        public BXBraillePart()
        {
            Measures = new List<BXBrailleMeasure>();
        }

        public void Parse(List<char> content, BrailleText brailleText)
        {
            //Hacer bucle

            BXBrailleMeasure m = new BXBrailleMeasure();
            this.Measures.Add(m);
            Object o = Measures[0].Parse(content, brailleText);
            //content.RemoveAt(0);

            for (int i=1; content.Count != 0; i++)
            {
                m = new BXBrailleMeasure(o);
                this.Measures.Add(m);
                Measures[i].Parse(content, brailleText);
            }
        }
    }
}
