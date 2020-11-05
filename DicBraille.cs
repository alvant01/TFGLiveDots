using System.Collections.Generic;

namespace LiveDots
{
    class DicBraille
    {

        public Dictionary<string, string> dic { get; set; }
        public DicBraille()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("1", "a");
            d.Add("12", "b");
            d.Add("14", "c");
            d.Add("145", "d");
            d.Add("15", "e");
            d.Add("124", "f");
            d.Add("1245", "g");
            d.Add("125", "h");
            d.Add("24", "i");
            d.Add("245", "j");
            d.Add("13", "k");
            d.Add("123", "l");
            d.Add("134", "m");
            d.Add("1345", "n");
            d.Add("135", "o");
            d.Add("1234", "p");
            d.Add("12345", "q");
            d.Add("1235", "r");
            d.Add("234", "s");
            d.Add("2345", "t");
            d.Add("136", "u");
            d.Add("1236", "v");
            d.Add("2456", "w");
            d.Add("1346", "x");
            d.Add("13456", "y");
            d.Add("1356", "z");
            d.Add("12356", "á");
            d.Add("2346", "é");
            d.Add("34", "í");
            d.Add("346", "ó");
            d.Add("23456", "ú");
            d.Add("1256", "ü");
            d.Add("12456", "ñ");
            d.Add("12346", "&");
            d.Add("3", ".");
            d.Add("2", ",");
            d.Add("26", "?");
            d.Add("23", ";"); //Fin signo texto
            d.Add("235", "!");
            d.Add("126", "(");
            d.Add("345", ")");
            d.Add("36", "-");
            d.Add("356", "]");
            d.Add("16", "1");
            d.Add("146", "3");
            d.Add("1456", "4");
            d.Add("156", "5");
            d.Add("1246", "6");
            d.Add("246", "9");
            d.Add("123456", "\\");
            d.Add("45", "^");
            d.Add("46", "{"); //Mayúscula braille
            d.Add("35", "}"); //Bastardilla
            d.Add("25", ":");
            d.Add("5", "`");
            d.Add("236", "[");
            d.Add("256", "*");
            d.Add("2356", "="); //Paréntesis en braille
            d.Add("456", "%");
            d.Add("4", "'");
            d.Add("56", "º"); //Comienzo signo texto
            d.Add("6", "_");
            d.Add("3456", "#");
            d.Add("", " ");
            d.Add(" ", " ");
            d.Add("\n", "\n");
            dic = d;
        }

        public string getTranslation(string s)
        {
            return this.dic[s];
        }
    }
}
