using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDots.Factories
{
    class FactoryLoadBraille : FactoryLoad
    {
        public FactoryLoadBraille()
        {
            BrailleText bt = new BrailleText();
            BrailleScore bs = new BrailleScore();


        }
        public override void load(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
