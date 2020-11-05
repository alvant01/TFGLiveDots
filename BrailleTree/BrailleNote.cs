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
    }
}


