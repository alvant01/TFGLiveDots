using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleClef : BrailleElement
    {
        public BrailleClef()
        {
            Sign = "G";
            Line = 2;
            Hand = "right";
            Ocho = null;
            //clef_octave_change = 0;

        }

        public BrailleClef(string s, int l, int c, string h)
        {
            Sign = s;
            Line = l;
            Hand = h;
            Ocho = null;
            //clef_octave_change = c;

        }
        public BrailleClef(string s, int l, int c, string h, string o)
        {
            Sign = s;
            Line = l;
            Hand = h;
            Ocho = o;
            //clef_octave_change = c;

        }

        public string Sign { get; set; }
        public int Line { get; set; }
        public string Hand { get; set; }
        //Indica que la clave de sol tiene un ocho por encima o por debajo. Valores:"up","down",null
        public string Ocho { get; set; }

        public int ParseBrailleInverse( BrailleText brailleText, char sign, char line, char hand, char ochoIni, char ocho)
        {
            int res = 3;
            List<string> L = new List<string>();
            switch (sign)
            {
                case 'í':
                    this.Sign = "G";
                    L.Add("34");
                    break;

                case '#':
                    this.Sign = "F";
                    L.Add("3456");
                    break;
                case 'ó':
                    this.Sign = "C";
                    L.Add("346");
                    break;
            }
            switch (line)
            {
                case '\'':
                    this.Line = 1;
                    L.Add("4");
                    break;
                case '%':
                    this.Line = 2;
                    L.Add("456");
                    break;
                case '`':
                    this.Line = 2;
                    L.Add("5");
                    break;
                case '{':
                    this.Line = 2;
                    L.Add("46");
                    break;
                default: // line = 2;
                    res = 2;
                    this.Line = 2;
                    break;
            }
            //Hand
            if (hand == 'l')
            {
                this.Hand = "left";
                L.Add("13");
            }
            else
            {
                this.Hand = "right";
                L.Add("123");
            }

            //ocho
            if (ochoIni == '#')
            {
                if (ocho == 'h')
                {
                    L.Add("125");
                    this.Ocho = "up";
                }
                else
                {
                    L.Add("236");
                    this.Ocho = "down";
                }
                res = 5;
            }
            else
            {
                this.Ocho = null;
            }



            brailleText.AddText(L);
            return res;
        }

        // public int clef_octave_change { get; set; }

        public void Parse(BrailleText brailleText)
        {
            ParseBraille(brailleText);
            ParseText(brailleText);
        }

        public void ParseBraille(BrailleText brailleText)
        {
            List<string> L = new List<string>();
            L.Add("345");
            //Revisar
            if (Sign == "G")
            {
                L.Add("34");
                //Si Line!=2
                if (Line == 1)
                    L.Add("4");
                else if (Line == 3)
                    L.Add("456");
                else if (Line == 4)
                    L.Add("5");
                else if (Line == 5)
                    L.Add("46");

                if (Hand == "left")
                    L.Add("13");
                else
                    L.Add("123");


                if (Ocho == "up")
                {
                    L.Add("3456");
                    L.Add("125");
                }
                else if (Ocho == "down")
                {
                    L.Add("3456");
                    L.Add("236");
                }
            }
            else if (Sign == "F")
            {
                L.Add("3456");
                //Si Line!=4
                if (Line == 1)
                    L.Add("4");
                else if (Line == 2)
                    L.Add("45");
                else if (Line == 3)
                    L.Add("456");
                else if (Line == 5)
                    L.Add("46");

                if (Hand == "right")
                    L.Add("13");
                else
                    L.Add("123");
            }
            else if (Sign == "C")
            {
                L.Add("346");
                //Si Line!=3
                if (Line == 1)
                    L.Add("4");
                else if (Line == 2)
                    L.Add("45");
                else if (Line == 4)
                    L.Add("5");
                else if (Line == 5)
                    L.Add("46");
                L.Add("123");
            }

            brailleText.AddText(L);
        }

        public void ParseText(BrailleText brailleText)
        {
            int tam = 0;
            string txt = "Clave de ";

            //L.Add("345");
            ++tam;

            if (Sign == "G")
            {
                txt += "Sol ";
                //L.Add("34");
                ++tam;
                //Si Line!=2
                if (Line == 1)
                {
                    ++tam;
                    txt += "en primera linea ";
                }
                else if (Line == 3)
                {
                    ++tam;
                    txt += "en tercera linea ";
                }
                else if (Line == 4)
                {
                    ++tam;
                    txt += "en cuarta linea ";
                }
                else if (Line == 5)
                {
                    ++tam;
                    txt += "en quinta linea ";
                }
                else txt += "en segunda linea ";

                if (Hand == "left")
                {
                    ++tam;
                    txt += "con mano izquierda ";
                }
                else
                {
                    ++tam;
                }

                if (Ocho == "up")
                {
                    tam += 2;
                    txt += "octava por encima ";
                }
                else if (Ocho == "down")
                {
                    tam += 2;
                    txt += "octava por debajo ";
                }
            }
            else if (Sign == "F")
            {
                txt += "Fa ";
                ++tam;
                //Si Line!=2
                if (Line == 1)
                {
                    ++tam;
                    txt += "en primera linea ";
                }
                else if (Line == 3)
                {
                    ++tam;
                    txt += "en tercera linea ";
                }
                else if (Line == 2)
                {
                    ++tam;
                    txt += "en segunda linea ";
                }
                else if (Line == 5)
                {
                    ++tam;
                    txt += "en quinta linea ";
                }
                else txt += "en cuarta linea ";

                if (Hand == "right")
                {
                    ++tam;
                    txt += "con mano derecha ";
                }
                else
                {
                    ++tam;
                }
                if (Ocho == "up")
                {
                    tam += 2;
                    txt += "octava por encima ";
                }
                else if (Ocho == "down")
                {
                    tam += 2;
                    txt += "octava por debajo ";
                }
            }
            else if (Sign == "C")
            {
                txt += "do";
                ++tam;
                //Si Line!=3
                if (Line == 1)
                {
                    ++tam;
                    txt += "en primera linea ";
                }
                else if (Line == 4)
                {
                    ++tam;
                    txt += "en cuarta linea ";
                }
                else if (Line == 2)
                {
                    ++tam;
                    txt += "en segunda linea ";
                }
                else if (Line == 5)
                {
                    ++tam;
                    txt += "en quinta linea ";
                }
                else txt += "en tercera linea ";

                if (Hand == "left")
                {
                    ++tam;
                    txt += "con mano izquierda ";
                }
                else
                {
                    ++tam;
                }

                if (Ocho == "up")
                {
                    tam += 2;
                    txt += "octava por encima ";
                }
                else if (Ocho == "down")
                {
                    tam += 2;
                    txt += "octava por debajo ";
                }
            }

            brailleText.AddViewer(txt, tam);

        }
    }
}
