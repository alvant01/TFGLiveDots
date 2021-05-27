using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBrailleClef : BXBrailleElement
    {
        public BXBrailleClef()
        {
            Sign = "G";
            Line = 2;
            Hand = "right";
            Ocho = null;
            //clef_octave_change = 0;

        }
        public string Sign { get; set; }
        public int Line { get; set; }
        public string Hand { get; set; }
        //Indica que la clave de sol tiene un ocho por encima o por debajo. Valores:"up","down",null
        public string Ocho { get; set; }

        public int ParseBrailleInverse(BrailleText brailleText, char sign, char line, char hand, char ochoIni, char ocho)
        {
            int res = 3;
            List<string> L = new List<string>();
            L.Add("345");
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
                    ochoIni = hand;
                    hand = line;
                    break;
            }
            //Hand
            if (hand == 'l')
            {
               this.Hand = "right";
               L.Add("123");
            }
            else
            {
                this.Hand = "left";
                 L.Add("13");
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
            ParseText(brailleText);
            return res;
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

        public void Parse(List<char> content, BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
    }
}
