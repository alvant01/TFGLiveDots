using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LiveDots
{
    public class BXBrailleTime : BXBrailleElement
    {
        public int Beats { get; set; }
        public int BeatType { get; set; }
        public Time_Symbol Symbol { get; set; }

        public BXBrailleTime() { }

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

        public int Parse(BrailleText brailleText, List<char> content)
        {
            List<string> L = new List<string>();
            bool pos = false;
            int i;
            switch (content[0])
            {
                case '#':
                    L.Add("3456");
                    this.Symbol = Time_Symbol.Normal;
                    break;
                case '{':
                    L.Add("46");
                    this.Symbol = Time_Symbol.Common;
                    break;
                case '%':
                    L.Add("456");
                    this.Symbol = Time_Symbol.Cut;
                    break;
            }
            for ( i = 1; content[i] != '\n'; i++)
            {
                if (!pos)
                {
                    switch (content[i])
                    {
                        case 'a':
                            this.Beats = 1;
                            L.Add("1");
                            break;
                        case 'b':
                            this.Beats = 2;
                            L.Add("12");
                            break;
                        case 'c':
                            this.Beats = 3;
                            L.Add("14");
                            break;
                        case 'd':
                            this.Beats = 4;
                            L.Add("145");
                            break;
                        case 'e':
                            this.Beats = 5;
                            L.Add("15");
                            break;
                        case 'f':
                            this.Beats = 6;
                            L.Add("124");
                            break;
                        case 'g':
                            this.Beats = 7;
                            L.Add("1245");
                            break;
                        case 'h':
                            this.Beats = 8;
                            L.Add("125");
                            break;
                        case 'i':
                            this.Beats = 9;
                            L.Add("24");
                            break;
                        case 'j':
                            this.Beats = 0;
                            L.Add("245");
                            break;
                        default:
                            pos = true;
                            i--;
                            break;
                    }
                }
                else 
                {
                    switch (content[i])
                    {
                        case ',':
                            this.BeatType = 1;
                            L.Add("2");
                            break;
                        case ';':
                            this.BeatType = 2;
                            L.Add("23");
                            break;
                        case ':':
                            this.BeatType = 3;
                            L.Add("25");
                            break;
                        case '*':
                            this.BeatType = 4;
                            L.Add("256");
                            break;
                        case '?':
                            this.BeatType = 5;
                            L.Add("26");
                            break;
                        case '!':
                            this.BeatType = 6;
                            L.Add("235");
                            break;
                        case '=':
                            this.BeatType = 7;
                            L.Add("2356");
                            break;
                        case '[':
                            this.BeatType = 8;
                            L.Add("236");
                            break;
                        case '}':
                            this.BeatType = 9;
                            L.Add("35");
                            break;
                        case ']':
                            this.BeatType = 0;
                            L.Add("356");
                            break;
                    }
                }
            }
            brailleText.AddText(L);
            ParseText(brailleText);
            return i;
        }

        /*
         * Converts an integer to Braille in the upper or lower
         * cell, depending on the argument pos.
         * pos = true -> Upper Cell
         * pos = false -> Lower Cell
         */

        public void Parse(List<char> content, BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
    }
}
