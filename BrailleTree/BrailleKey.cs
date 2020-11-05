using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleKey : BrailleElement
    {

        public int Fifths { get; set; }
        public string Mode { get; set; }

        public void Parse(BrailleText brailleText)
        {
            ParseBraille(brailleText);
            ParseText(brailleText);
        }

        public void ParseBraille(BrailleText brailleText)
        {
            List<string> L = new List<string>();

            switch (Fifths)
            {
                case 1: L.Add("146"); break;
                case 2: L.Add("146"); L.Add("146"); break;
                case 3: L.Add("146"); L.Add("146"); L.Add("146"); break;
                case 4: L.Add("3456"); L.Add("145"); L.Add("146"); break;
                case 5: L.Add("3456"); L.Add("15"); L.Add("146"); break;
                case 6: L.Add("3456"); L.Add("124"); L.Add("146"); break;
                case -1: L.Add("126"); break;
                case -2: L.Add("126"); L.Add("126"); break;
                case -3: L.Add("126"); L.Add("126"); L.Add("126"); break;
                case -4: L.Add("3456"); L.Add("145"); L.Add("126"); break;
                case -5: L.Add("3456"); L.Add("15"); L.Add("126"); break;
                case -6: L.Add("3456"); L.Add("124"); L.Add("126"); break;
            }
            brailleText.AddText(L);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns>number of parameters needed for the Key</returns>
        public int ParseBrailleInverse(char v1, char v2, char v3)
        {
            int res = 0;



            return res;
        }


        public List<int> getAlters()
        {
            List<int> L = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0 });
            switch (Mode)
            {
                case "Major":
                    switch (Fifths)
                    {
                        case 1: L[3] = 1; break;
                        case 2: L[3] = 1; L[0] = 1; break;
                        case 3: L[3] = 1; L[0] = 1; L[4] = 1; break;
                        case 4: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; break;
                        case 5: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; L[5] = 1; break;
                        case 6: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; L[5] = 1; L[2] = 1; break;
                        case -1: L[6] = -1; break;
                        case -2: L[6] = -1; L[2] = -1; break;
                        case -3: L[6] = -1; L[2] = -1; L[5] = -1; break;
                        case -4: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; break;
                        case -5: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; L[4] = -1; break;
                        case -6: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; L[4] = -1; L[0] = -1; break;

                    }
                    break;
                default:
                    switch (Fifths)
                    {
                        case 1: L[3] = 1; break;
                        case 2: L[3] = 1; L[0] = 1; break;
                        case 3: L[3] = 1; L[0] = 1; L[4] = 1; break;
                        case 4: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; break;
                        case 5: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; L[5] = 1; break;
                        case 6: L[3] = 1; L[0] = 1; L[4] = 1; L[1] = 1; L[5] = 1; L[2] = 1; break;
                        case -1: L[6] = -1; break;
                        case -2: L[6] = -1; L[2] = -1; break;
                        case -3: L[6] = -1; L[2] = -1; L[5] = -1; break;
                        case -4: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; break;
                        case -5: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; L[4] = -1; break;
                        case -6: L[6] = -1; L[2] = -1; L[5] = -1; L[1] = -1; L[4] = -1; L[0] = -1; break;

                    }

                    break;
            }
            return L;
        }

        public void ParseText(BrailleText brailleText)
        {
            int tam = 0;
            string txt = "Armadura de ";
            switch (Fifths)
            {
                case 1: ++tam; break;
                case 2: tam += 2; break;
                case 3: tam += 3; break;
                case 4: tam += 3; break;
                case 5: tam += 3; break;
                case 6: tam += 3; break;
                case -1: ++tam; break;
                case -2: tam += 2; break;
                case -3: tam += 3; break;
                case -4: tam += 3; break;
                case -5: tam += 3; break;
                case -6: tam += 3; break;
            }
            switch (Mode)
            {
                case "Minor":
                    switch (Fifths)
                    {
                        case 1: txt += "mi "; break;
                        case 2: txt += "si "; break;
                        case 3: txt += "fa sostenido "; break;
                        case 4: txt += "do sostenido "; break;
                        case 5: txt += "sol sostenido "; break;
                        case 6: txt += "re sostenido "; break;
                        case -1: txt += "re "; break;
                        case -2: txt += "sol "; break;
                        case -3: txt += "do "; break;
                        case -4: txt += "fa "; break;
                        case -5: txt += "si bemol "; break;
                        case -6: txt += "mi bemol "; break;

                    }
                    txt += "menor ";
                    break;
                default: //Major
                    switch (Fifths)
                    {
                        case 1: txt += "sol "; break;
                        case 2: txt += "re "; break;
                        case 3: txt += "la "; break;
                        case 4: txt += "mi "; break;
                        case 5: txt += "si "; break;
                        case 6: txt += "fa sostenido "; break;
                        case -1: txt += "fa "; break;
                        case -2: txt += "si bemol "; break;
                        case -3: txt += "mi bemol "; break;
                        case -4: txt += "la bemol "; break;
                        case -5: txt += "re bemol "; break;
                        case -6: txt += "sol bemol "; break;
                    }
                    txt += "mayor ";
                    break;
            }
            switch (Fifths) //Coincide para mayor y menor, hay que ver al añadir más modos si también coincide
            {
                case 1: txt += "con sostenido en fa "; break;
                case 2: txt += "con sostenidos en fa y do "; break;
                case 3: txt += "con sostenidos en fa, do y sol "; break;
                case 4: txt += "con sostenidos en fa, do, sol y re "; break;
                case 5: txt += "con sostenidos en fa, do, sol, re y la "; break;
                case 6: txt += "con sostenidos en fa, do, sol, re, la y mi "; break;
                case -1: txt += "con bemol en si "; break;
                case -2: txt += "con bemoles en si y mi "; break;
                case -3: txt += "con bemoles en si, mi y la "; break;
                case -4: txt += "con bemoles en si, mi, la y re "; break;
                case -5: txt += "con bemoles en si, mi, la, re y sol "; break;
                case -6: txt += "con bemoles en si, mi, la, re, sol y do "; break;
            }
            if (tam > 0)
                brailleText.AddViewer(txt, tam);
        }
    }
}
