using System.Collections.Generic;

namespace LiveDots
{
    static class Translator
    {
        static public string Num2Braille(List<string> ls)
        {
            string braille = "";
            DicBraille dic = new DicBraille();
            foreach (string s in ls)
            {
                braille += dic.getTranslation(s);
            }
            return braille;
        }
    }
}
