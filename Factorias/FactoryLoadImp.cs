using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDots.Factories
{
    public class FactoryLoadImp : FactoryLoad
    {
        public override BrailleScore GetBrailleScore()
        {
            return new BrailleScore();
        }

        public override BXBrailleScore GetBXBrailleScore()
        {
            return new BXBrailleScore();
        }
    }
}
