using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LiveDots
{
    public enum Time_Symbol { Normal, Common, Cut, SingleNumber };
    public class BrailleTime : BrailleElement
    {
        public int Beats { get; set; }
        public int BeatType { get; set; }
        public Time_Symbol Symbol { get; set; }

        public BrailleTime() { }

        public BrailleTime(int b, int bt)
        {
            Beats = b;
            BeatType = bt;
        }

        public void Parse(BrailleText brailleText)
        {
            ParseBraille(brailleText);
            ParseText(brailleText);
        }

        public void ParseBraille(BrailleText brailleText)
        {
            List<string> L = new List<string>();
            switch (Symbol)
            {
                case Time_Symbol.Normal:
                    L.Add("3456");
                    L.AddRange(Number2Braille(Beats, true));
                    L.AddRange(Number2Braille(BeatType, false));
                    break;
                case Time_Symbol.Common: L.Add("46"); L.Add("14"); break;
                case Time_Symbol.Cut: L.Add("456"); L.Add("14"); break;
                case Time_Symbol.SingleNumber:
                    L.Add("3456");
                    L.AddRange(Number2Braille(Beats, true));
                    break;
            }
            brailleText.AddText(L);
        }


        public void ParseText(BrailleText brailleText)
        {
            switch (Symbol)
            {
                case Time_Symbol.Normal:
                    brailleText.AddViewer("Compás de " + Beats.ToString() + " por " + BeatType.ToString() + " ",
                        Beats.ToString().Length + BeatType.ToString().Length + 1);
                    break;
                case Time_Symbol.Common: brailleText.AddViewer("Compás de compasillo ", 2); break;
                case Time_Symbol.Cut: brailleText.AddViewer("Compás binario ", 2); break;
                case Time_Symbol.SingleNumber:
                    brailleText.AddViewer("Compás de " + Beats.ToString() + " ",
                        Beats.ToString().Length + 1);
                    break;
            }
        }

        internal void ParseBrailleInverse(char sign, char beat, char beatType)
        {
            switch (sign)
            {
                case '#':
                    this.Symbol = Time_Symbol.Normal;
                    break;
                case '{':
                    this.Symbol = Time_Symbol.Common;
                    break;
                case '%':
                    this.Symbol = Time_Symbol.Cut;
                    break;
                case '':
            }
        }

        public static List<string> Number2Braille(int N, bool pos = true)
        {
            List<string> L = new List<string>();
            foreach (char c in N.ToString())
            {
                L.Add(CharDigit2Braille(c, pos));
            }
            return L;
        }



        /*
         * Converts an integer to Braille in the upper or lower
         * cell, depending on the argument pos.
         * pos = true -> Upper Cell
         * pos = false -> Lower Cell
         */
        public static string CharDigit2Braille(char N, bool pos = true)
        {
            string s = "";
            if (pos)
            {
                switch (N)
                {
                    case '1': s = "1"; break;
                    case '2': s = "12"; break;
                    case '3': s = "14"; break;
                    case '4': s = "145"; break;
                    case '5': s = "15"; break;
                    case '6': s = "124"; break;
                    case '7': s = "1245"; break;
                    case '8': s = "125"; break;
                    case '9': s = "24"; break;
                    case '0': s = "245"; break;
                }
            }
            else
            {
                switch (N)
                {
                    case '1': s = "2"; break;
                    case '2': s = "23"; break;
                    case '3': s = "25"; break;
                    case '4': s = "256"; break;
                    case '5': s = "26"; break;
                    case '6': s = "235"; break;
                    case '7': s = "2356"; break;
                    case '8': s = "236"; break;
                    case '9': s = "35"; break;
                    case '0': s = "356"; break;
                }
            }
            return s;
        }

    }
}
