using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDots.Factories
{
    public abstract class FactoryLoad
    {
        private static FactoryLoad instance = null;
        public static FactoryLoad GetInstance()
        {
            if (instance == null)
            {
                instance = new FactoryLoadImp();
            }

            return instance;
        }
        public abstract BrailleScore GetBrailleScore();

        public abstract BXBrailleScore GetBXBrailleScore();

        //public abstract 
    }
}
