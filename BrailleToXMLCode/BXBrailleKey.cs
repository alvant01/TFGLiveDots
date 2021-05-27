using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBrailleKey : BXBrailleElement
    {

        public int Fifths { get; set; }
        public string Mode { get; set; }

        public void Parse(List<char> content, BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns>number of parameters needed for the Key</returns
        /// </summary>
        public int Parse(BrailleText brailleText, char v1, char v2, char v3)
        {
            List<string> L = new List<string>();
            if (v1 == ' ')
            {
                return 1;
            }

            int res = 0;
            if (v1 == '3')
            {
                L.Add("146");
                if (v2 == '3')
                {
                    L.Add("146");
                    if (v3 == '3')
                    {
                        L.Add("146");
                        this.Fifths = 3;
                        res = 4;
                    }
                    else
                    {
                        this.Fifths = 2;
                        res = 3;
                    }
                }
                else
                {
                    this.Fifths = 1;
                    res = 2;
                }
            }
            else if (v1 == '#')
            {
                L.Add("3456");
                if (v2 == 'd' && v3 == '3')
                {
                    L.Add("145");
                    L.Add("146");
                    this.Fifths = 4;
                }
                else if (v2 == 'e' && v3 == '3')
                {
                    L.Add("15");
                    L.Add("146");
                    this.Fifths = 5;
                }
                else if (v2 == 'f' && v3 == '3')
                {
                    L.Add("124");
                    L.Add("146");
                    this.Fifths = 6;
                }
                else if (v2 == 'd' && v3 == '(')
                {
                    L.Add("145");
                    L.Add("126");
                    this.Fifths = -4;
                }
                else if (v2 == 'e' && v3 == '(')
                {
                    L.Add("15");
                    L.Add("126");
                    this.Fifths = -5;
                }
                else if (v2 == 'f' && v3 == '(')
                {
                    L.Add("124");
                    L.Add("126");
                    this.Fifths = -6;
                }
                res = 4;
            }
            else if (v1 == '(')
            {
                L.Add("126");
                if (v2 == '(')
                {
                    L.Add("126");

                    if (v3 == '(')
                    {
                        L.Add("126");

                        this.Fifths = -3;
                        res = 4;
                    }
                    else
                    {
                        this.Fifths = -2;
                        res = 3;
                    }
                }
                else
                {
                    this.Fifths = -1;
                    res = 2;
                }
            }
            else { this.Fifths = 0; }
            brailleText.AddText(L);
            ParseText(brailleText);
            return res;
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
