using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BXBrailleVoice : BXBrailleElement
    {
        public int StartingTime { get; set; }

        public int EndingTime { get; set; }

        public List<BXBrailleNote> Notes { get; set; }

        public BXBrailleVoice()
        {
            Notes = new List<BXBrailleNote>();
        }

        public BXBrailleVoice(int st, int et, List<BXBrailleNote> n)
        {
            StartingTime = st;
            EndingTime = et;
            Notes = n;
        }

        internal void Parse(List<char> content,  BrailleText brailleText)
        {
           while (content.Count != 0 )
            {
                if (content[0] == ' ')
                {
                    content.RemoveAt(0);
                    break;
                }
                else if (content[1] == 'k' && content[0] == '(')
                {
                    content.RemoveRange(0, 2);
                }
                else if (content[0] == '\n')
                {
                    brailleText.JumpLine();
                    content.RemoveAt(0);
                }
                else if (content[0] == '\r')
                {
                   // brailleText.JumpLine();
                    content.RemoveAt(0);
                }
                else
                {
                    BXBrailleNote n = new BXBrailleNote();

                    n.Parse(brailleText, content);
                    this.Notes.Add(n);
                }

            }
        }

        void BXBrailleElement.Parse(List<char> content, BrailleText brailleText)
        {
            throw new NotImplementedException();
        }
    }
}
