using System.Collections.Generic;

namespace LiveDots
{
    interface BXBrailleElement
    {
        public void Parse(List<char> content, BrailleText brailleText);
    }
}