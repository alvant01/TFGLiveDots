using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDots
{
    /*  enum Duration
     {
         Maxima, Long, Breve, Whole, Half, Quarter, Eighth, _16th, _32nd, _64th, _128th, _256th, _512th, _1024th 
     }
     */

    public enum Step
    {
        A, B, C, D, E, F, G
    }

    public enum Octave
    {
        First, Second, Third, Fourth, Fifth, Sixth, Seventh
    }

    public enum Type2
    {
        Maxima, Long, Breve, Whole, Half, Quarter, Eighth, _16th, _32nd, _64th, _128th, _256th, _512th, _1024th
    }


    public class BrailleNote : BrailleElement
    {
        //public Pitch pitch { get; set; }
        //public Duration duration { get; set; } //Usaremos la variable type de music xml que nos da directamente la figura a representar, en lugar de duration, que es relativo al compás en que se esté mediendo

        public Step Step { get; set; }
        public Octave Octave { get; set; }

        //El agrupamiento se representa usando la primera nota del agrupamiento
        //No estoy seguro de que se pueda poner un atributo del mismo tipo que la clase
        //public Tuplet Tuplet { get; set; }

        public string Type { get; set; }

        public int Staff { get; set; }

        public bool IsRest { get; set; }

        public bool IsDotted { get; set; }
        public int Alter { get; set; }

        public string Accidental { get; set; }

        // Indicators for printing attributes
        public bool PrintOctave { get; set; }
        public bool PrintValueSeparation { get; set; }
        public bool PrintAlter { get; set; }

        public bool PreviousBreve { get; set; }
        public bool Previous256th { get; set; }

        public BrailleNote()
        {
            Type = null;
        }

        public BrailleNote(Step s, Octave o, string t, int st)
        {
            Step = s;
            Octave = o;
            //Tuplet = tup;
            Type = t;
            Staff = st;
        }

        private string Octave2Braille()
        {
            string s = "";
            switch (Octave)
            {
                case (Octave.First):
                    s = "4";
                    break;
                case (Octave.Second):
                    s = "45";
                    break;
                case (Octave.Third):
                    s = "456";
                    break;
                case (Octave.Fourth):
                    s = "5";
                    break;
                case (Octave.Fifth):
                    s = "46";
                    break;
                case (Octave.Sixth):
                    s = "56";
                    break;
                case (Octave.Seventh):
                    s = "6";
                    break;
            }
            return s;
        }
        private string Octave2Text()
        {
            string txt = "";
            switch (Octave)
            {
                case (Octave.First):
                    txt = "Primera octava";
                    break;
                case (Octave.Second):
                    txt = "Segunda octava";
                    break;
                case (Octave.Third):
                    txt = "Tercera octava";
                    break;
                case (Octave.Fourth):
                    txt = "Cuarta octava";
                    break;
                case (Octave.Fifth):
                    txt = "Quinta octava";
                    break;
                case (Octave.Sixth):
                    txt = "Sexta octava";
                    break;
                case (Octave.Seventh):
                    txt = "Séptima octava";
                    break;
            }
            txt += " ";
            return txt;
        }

        public static int Octave2Int(Octave o)
        {
            switch (o)
            {
                case (Octave.First):
                    return 1;
                case (Octave.Second):
                    return 2;
                case (Octave.Third):
                    return 3;
                case (Octave.Fourth):
                    return 4;
                case (Octave.Fifth):
                    return 5;
                case (Octave.Sixth):
                    return 6;
                case (Octave.Seventh):
                    return 7;
            }
            return 0;
        }


        internal void GiveAccidental(List<List<int>> alterAux)
        {
            PrintAlter = alterAux[Octave2Int(Octave) - 1][Step2Int(Step) - 1] != Alter;
            alterAux[Octave2Int(Octave) - 1][Step2Int(Step) - 1] = Alter;
        }


        public static int Step2Int(Step s)
        {
            switch (s)
            {
                case Step.A:
                    return 6;
                case Step.B:
                    return 7;
                case Step.C: //Las octavas empiezan en do
                    return 1;
                case Step.D:
                    return 2;
                case Step.E:
                    return 3;
                case Step.F:
                    return 4;
                case Step.G:
                    return 5;
            }
            return -1;
        }
        //Devuelve el intervalo entre dos notas dadas: segunda, tercera...
        public static int Interval(BrailleNote n1, BrailleNote n2)
        {
            if (n1.IsRest || n2.IsRest)
            {
                return -1;
            }
            int s1 = Step2Int(n1.Step);
            int s2 = Step2Int(n2.Step);
            int o1 = Octave2Int(n1.Octave);
            int o2 = Octave2Int(n2.Octave);
            if (o1 == o2)
                return Math.Abs(s1 - s2) + 1;
            else if (o1 < o2)
                return 8 * (o2 - o1) - s1 + s2;
            else
                return 8 * (o1 - o2) - s2 + s1;
        }

        public static bool OctaveNeeded(BrailleNote n1, BrailleNote n2)
        {
            bool needed = false;
            int interval = Interval(n1, n2);
            if (n1 == null ||
                (interval == 4 && n1.Octave != n2.Octave) ||
                (interval == 5 && n1.Octave != n2.Octave) ||
                interval >= 6)
                needed = true;

            return needed;
        }

        public static void CheckLast(BrailleNote nLast, BrailleNote n)
        {
            // If its needed to print the octave before the note, set the attribute to true.
            if (nLast == null || BrailleNote.OctaveNeeded(nLast, n))
                n.PrintOctave = true;

            if (nLast != null && n.Type == "256th" && nLast.Type == "256th")
                n.Previous256th = true;

            if (nLast != null && n.Type == "breve" && nLast.Type == "breve")
                n.PreviousBreve = true;

            if (nLast != null && BrailleNote.ValueSeparationNeeded(nLast, n))
                n.PrintValueSeparation = true;
        }

        public List<string> PrintAccidental()
        {
            List<string> L = new List<string>();
            switch (Alter)
            {
                case -2:
                    L.Add("126"); L.Add("126"); break;
                case -1:
                    L.Add("126"); break;
                case 1:
                    L.Add("146"); break;
                case 0:
                    L.Add("16"); break;
                case 2:
                    L.Add("146"); L.Add("146"); break;
            }
            return L;
        }


        public string TextAccidental(ref int tam)
        {
            string txt = "";
            switch (Alter)
            {
                case -2:
                    tam = 2;
                    txt = "doble bemol";
                    break;
                case -1:
                    tam = 1;
                    txt = "bemol";
                    break;
                case 1:
                    tam = 1;
                    txt = "sostenido";
                    break;
                case 0:
                    tam = 1;
                    txt = "becuadro";
                    break;
                case 2:
                    tam = 2;
                    txt = "doble sostenido";
                    break;
            }
            return txt;
        }

        public void Parse(BrailleText brailleText)
        {
            ParseBraille(brailleText);
            ParseText(brailleText);
        }

        public void ParseBraille(BrailleText brailleText)
        {
            List<string> L = new List<string>();

            if (PrintValueSeparation)
            {
                L.Add("126");
                L.Add("2");
            }


            string s = "";

            if (IsRest)  //silencio
            {
                if (Type == "whole" || Type == "16th")
                    s = "134";
                else if (Type == "half" || Type == "32nd")
                    s = "136";
                else if (Type == "quarter" || Type == "64th")
                    s = "1236";
                else if (Type == "eighth" || Type == "126th")
                    s = "1346";

            }
            else
            {
                if (PrintAlter) L.AddRange(PrintAccidental());
                if (PrintOctave)
                    L.Add(Octave2Braille());
                switch (Step)
                {
                    case Step.A: //la
                        s += "24";
                        break;
                    case Step.B: //si
                        s += "245";
                        break;
                    case Step.C: //do
                        s += "145";
                        break;
                    case Step.D: //re
                        s += "15";
                        break;
                    case Step.E: //mi
                        s += "124";
                        break;
                    case Step.F: //fa
                        s += "1245";
                        break;
                    case Step.G: //sol
                        s += "125";
                        break;
                }


                switch (Type)
                {
                    /*case "maxima": //maxima (32)
                        s += "";
                        break;
                    case "long": //longa (16)
                        s += "";
                        break;*/
                    case "breve": //cuadrada (8)
                        s += "36";
                        s = new String(s.OrderBy(x => x).ToArray());
                        L.Add(s);
                        if (!PreviousBreve)
                        {
                            L.Add("45");
                            L.Add("14");
                        }
                        break;
                    case "whole": //redonda
                        s += "36";
                        break;
                    case "half": //blanca
                        s += "3";
                        break;
                    case "quarter": //negra (1)
                        s += "6";
                        break;
                    case "eighth": //corchea
                        s += "";
                        break;
                    case "16th": //semicorchea
                        s += "36";
                        break;
                    case "32nd": //fusa
                        s += "3";
                        break;
                    case "64th": //semifusa
                        s += "6";
                        break;
                    case "128th": //garrapatea
                        s += "";
                        break;
                    case "256th": //semigarrapatea 
                        s += "36";
                        if (!Previous256th)
                        {
                            L.Add("56");
                            L.Add("126");
                            L.Add("2");
                        }
                        break;
                        /*case _512th:
                            s += "";
                            break;
                        case _1024th:
                            s += "";
                            break;*/



                }
            }
            s = new String(s.OrderBy(x => x).ToArray());
            L.Add(s);
            brailleText.AddText(L);
        }

        public static bool ValueSeparationNeeded(BrailleNote n1, BrailleNote n2)
        {
            switch (n2.Type)
            {
                case "whole": //redonda
                    if (n1.Type == "16th") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "half": //blanca
                    if (n1.Type == "32nd") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "quarter": //negra (1)
                    if (n1.Type == "64th") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "eighth": //corchea
                    if (n1.Type == "128th") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "16th": //semicorchea
                    if (n1.Type == "whole") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "32nd": //fusa
                    if (n1.Type == "half") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "64th": //semifusa
                    if (n1.Type == "quarter") //separacion de valores
                    {
                        return true;
                    }
                    break;
                case "128th": //garrapatea
                    if (n1.Type == "eighth") //separacion de valores
                    {
                        return true;
                    }
                    break;
                default: return false;
            }

            return false;
        }

        public void ParseText(BrailleText brailleText)
        {
            int tam = 0;
            string txt = "";

            if (PrintValueSeparation)
            {
                txt += "Separación de valores ";
                tam += 2;
                brailleText.AddViewer(txt, tam);
                return;
            }



            if (IsRest)  //silencio
            {

                if (Type == "whole")
                {
                    ++tam;
                    txt += "Silencio de redonda";
                }
                else if (Type == "half")
                {
                    ++tam;
                    txt += "Silencio de blanca";
                }
                else if (Type == "quarter")
                {
                    ++tam;
                    txt += "Silencio de negra";
                }
                else if (Type == "eighth")
                {
                    ++tam;
                    txt += "Silencio de corchea";
                }
                else if (Type == "16th")
                {
                    ++tam;
                    txt += "Silencio de semicorchea";
                }
                else if (Type == "32nd")
                {
                    ++tam;
                    txt += "Silencio de fusa";
                }
                else if (Type == "64th")
                {
                    ++tam;
                    txt += "Silencio de semifusa";
                }
                else if (Type == "126th")
                {
                    ++tam;
                    txt += "Silencio de garrapatea";
                }
                txt += " ";
            }
            else
            {

                if (PrintOctave)
                {
                    txt += Octave2Text();
                    ++tam;
                }

                switch (Step)
                {
                    case Step.A: //la
                        ++tam;
                        txt = "La ";
                        break;
                    case Step.B: //si
                        ++tam;
                        txt += "Si ";
                        break;
                    case Step.C: //do
                        ++tam;
                        txt += "Do ";
                        break;
                    case Step.D: //re
                        ++tam;
                        txt += "Re ";
                        break;
                    case Step.E: //mi
                        ++tam;
                        txt += "Mi ";
                        break;
                    case Step.F: //fa
                        ++tam;
                        txt += "Fa ";
                        break;
                    case Step.G: //sol
                        ++tam;
                        txt += "Sol ";
                        break;
                }

                if (PrintAlter)
                {
                    int aux = 0;
                    txt += TextAccidental(ref aux);
                    tam += aux;
                }


                switch (Type)
                {
                    /*case "maxima": //maxima (32)
                        s += "";
                        break;
                    case "long": //longa (16)
                        s += "";
                        break;*/
                    case "breve": //cuadrada (8)
                        txt += "cuadrada ";
                        if (!PreviousBreve)
                        {
                            tam += 2;
                        }
                        break;
                    case "whole": //redonda
                        txt += "redonda ";
                        break;
                    case "half": //blanca
                        txt += "blanca ";
                        break;
                    case "quarter": //negra (1)
                        txt += "negra ";
                        break;
                    case "eighth": //corchea
                        txt += "corchea ";
                        break;
                    case "16th": //semicorchea
                        txt += "semicorchea ";
                        break;
                    case "32nd": //fusa
                        txt += "fusa ";
                        break;
                    case "64th": //semifusa
                        txt += "semifusa ";
                        break;
                    case "128th": //garrapatea
                        txt += "garrapatea ";
                        break;
                    case "256th": //semigarrapatea 
                        txt += "semigarrapatea ";
                        if (!Previous256th)
                        {
                            tam += 3;
                        }
                        break;
                        /*case _512th:
                            s += "";
                            break;
                        case _1024th:
                            s += "";
                            break;*/
                }
            }
            brailleText.AddViewer(txt, tam);
        }
        public void ParseBrailleInverse(BrailleText brailleText, List<char> content)
        {
            List<string> L = new List<string>();
            string s = "";
            int i;
            if (content[1] == 'k' && content[0] == '(')
            {
                content.RemoveRange(0, 2);
                return;
            }
                i =0;
            if (content[0] == '\n')
            {
                brailleText.JumpLine();
                content.RemoveAt(0);
                return;
            }
            if (content[0] == ' ')
            {
                brailleText.AddSpace();
                content.RemoveAt(0);            
                return;
            }
            switch (content[0]) //silencios
            {
                case 'm':
                    this.Type = "whole";
                    this.IsRest = true;
                    L.Add("134");
                    break;
                case 'u':
                    this.Type = "half";
                    this.IsRest = true;
                    L.Add("136");
                    break;
                case 'v':
                    this.Type = "quarter";
                    this.IsRest = true;
                    L.Add("1236");
                    break;
                case 'x':
                    this.Type = "eight";
                    this.IsRest = true;
                    L.Add("1346");
                    break;
            }
            if (IsRest)
            {
                brailleText.AddText(L);
                content.RemoveAt(0);
                ParseText(brailleText);
                return;
            }

            //Alter
            if (content[0] == '(' && content[1] == '(')
            {
                L.Add("126");
                L.Add("126");
                Alter = -2;
                PrintAlter = true;
                content.RemoveRange(0, 2);
            }
            else if(content[0] == '(')
            {
                L.Add("126");
                Alter = -1;
                PrintAlter = true;
                content.RemoveAt(0);
            }
            else if (content[0] == '1')
            {
                L.Add("16");
                Alter = 0;
                PrintAlter = true;
                content.RemoveAt(0);
            }
            else if (content[0] == '3')
            {
                L.Add("146");
                Alter = 1;
                PrintAlter = true;
                content.RemoveAt(0);
            }
            else if (content[0] == '3' && content[1] == '3')
            {
                L.Add("146");
                L.Add("146");
                Alter = 2;
                PrintAlter = true;
                content.RemoveRange(0, 2);
            }
            //Octava
            switch (content[0])
            {
                case '\'':
                    L.Add("4");
                    Octave = Octave.First;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case '^':
                    L.Add("45");
                    Octave = Octave.Second;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case '%':
                    L.Add("456");
                    Octave = Octave.Third;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case '`':
                    L.Add("5");
                    Octave = Octave.Fourth;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case '{':
                    L.Add("46");
                    Octave = Octave.Fifth;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case 'º':
                    L.Add("56");
                    Octave = Octave.Sixth;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
                case '_':
                    L.Add("6");
                    Octave = Octave.Seventh;
                    this.PrintOctave = true;
                    content.RemoveAt(0);
                    break;
            }
            //Steps
            switch (content[0])
            {
                case 'é': //La Redondo
                    L.Add("2346");
                    this.Step = Step.A;
                    this.Type = "whole";
                    break;
                case 's': //La blanca
                    L.Add("234");
                    this.Step = Step.A;
                    this.Type = "half";
                    break;
                case '9': //La negra
                    L.Add("246");
                    this.Step = Step.A;
                    this.Type = "quarter";
                    break;
                case 'i':
                    L.Add("24");//La corchea
                    this.Step = Step.A;
                    this.Type = "eighth";
                    break;

                case 'ú': //Si redondo
                    L.Add("23456");
                    this.Step = Step.B;
                    this.Type = "whole";
                    break;
                case 't': //Si blanca
                    L.Add("2345");
                    this.Step = Step.B;
                    this.Type = "half";
                    break;
                case 'w': //Si negra
                    L.Add("2456");
                    this.Step = Step.B;
                    this.Type = "quarter";
                    break;
                case 'j': //Si corchea
                    L.Add("245");
                    this.Step = Step.B;
                    this.Type = "eighth";
                    break;

                case 'y': //Do redondo
                    L.Add("13456");
                    this.Step = Step.C;
                    this.Type = "whole";
                    break;
                case 'n': //Do blanca
                    L.Add("1345");
                    this.Step = Step.C;
                    this.Type = "half";
                    break;
                case '4': //Do negra
                    L.Add("1456");
                    this.Step = Step.C;
                    this.Type = "quarter";
                    break;
                case 'd': //Do corchea
                    L.Add("145");
                    this.Step = Step.C;
                    this.Type = "eighth";
                    break;

                case 'z': //Re redondo
                    L.Add("1356");
                    this.Step = Step.D;
                    this.Type = "whole";
                    break;
                case 'o': //Re blanca
                    L.Add("135");
                    this.Step = Step.D;
                    this.Type = "half";
                    break;
                case '5': //Re negra
                    L.Add("156");
                    this.Step = Step.D;
                    this.Type = "quarter";
                    break;
                case 'e'://Re corchea
                    L.Add("15");
                    this.Step = Step.D;
                    this.Type = "eighth";
                    break;

                case '&': //Mi redondo
                    L.Add("12346");
                    this.Step = Step.E;
                    this.Type = "whole";
                    break;
                case 'p': //Mi blanca
                    L.Add("1234");
                    this.Step = Step.E;
                    this.Type = "half";
                    break;
                case '6': //Mi negra
                    L.Add("1246");
                    this.Step = Step.E;
                    this.Type = "quarter";
                    break;
                case 'f'://Mi corchea
                    L.Add("124");
                    this.Step = Step.E;
                    this.Type = "eighth";
                    break;

                case '\\': //Fa redondo
                    L.Add("123456");
                    this.Step = Step.F;
                    this.Type = "whole";
                    break;
                case 'q': //Fa blanca
                    L.Add("12345");
                    this.Step = Step.F;
                    this.Type = "half";
                    break;
                case 'ñ': //Fa negra
                    L.Add("12456");
                    this.Step = Step.F;
                    this.Type = "quarter";
                    break;
                case 'g'://Fa corchea
                    L.Add("1245");
                    this.Step = Step.F;
                    this.Type = "eighth";
                    break;

                case 'á': //Sol redondo
                    L.Add("12356");
                    this.Step = Step.G;
                    this.Type = "whole";
                    break;
                case 'r': //Sol blanca
                    L.Add("1235");
                    this.Step = Step.G;
                    this.Type = "half";
                    break;
                case 'ü': //Sol negra
                    L.Add("1256");
                    this.Step = Step.G;
                    this.Type = "quarter";
                    break;
                case 'h'://Sol corchea
                    L.Add("125");
                    this.Step = Step.G;
                    this.Type = "eighth";
                    break;
            } ///Notas
            content.RemoveAt(0);
            brailleText.AddText(L);
            ParseText(brailleText);

        }
    }
}


