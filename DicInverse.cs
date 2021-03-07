using System.Collections.Generic;

namespace LiveDots
{
    class DicInverse
    {

        public Dictionary<string, string> dic { get; set; }
        public DicInverse()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("a", "1");
            d.Add("b", "12");
            d.Add("c", "14");
            d.Add("d", "145");
            d.Add("e", "15");
            d.Add("f", "124");
            d.Add("g", "1245");
            d.Add("h", "125");
            d.Add("i", "24");
            d.Add("j", "245");
            d.Add("k", "13");
            d.Add("l", "123");
            d.Add("m", "134");
            d.Add("n", "1345");
            d.Add("o", "135");
            d.Add("p", "1234");
            d.Add("q", "12345");
            d.Add("r", "1235");
            d.Add("s", "234");
            d.Add("t", "2345");
            d.Add("u", "136");
            d.Add("v", "1236");
            d.Add("w", "2456");
            d.Add("x", "1346");
            d.Add("y", "13456");
            d.Add("z", "1356");
            d.Add("�", "12356");
            d.Add("�", "2346");
            d.Add("�", "34");
            d.Add("�", "346");
            d.Add("�", "23456");
            d.Add("�", "1256");
            d.Add("�", "12456");
            d.Add("&", "12346");
            d.Add(".", "3");
            d.Add(",", "2");
            d.Add("?", "26");
            d.Add("", "");
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
            d.Add("46", "{"); //May�scula braille
            d.Add("35", "}"); //Bastardilla
            d.Add("25", ":");
            d.Add("5", "`");
            d.Add("236", "[");
            d.Add("256", "*");
            d.Add("2356", "="); //Par�ntesis en braille
            d.Add("456", "%");
            d.Add("4", "'");
            d.Add("56", "�"); //Comienzo signo texto
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