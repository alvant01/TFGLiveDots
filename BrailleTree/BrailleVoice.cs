using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleVoice : BrailleElement
    {
        public int EndingTime { get; set; }

        public List<BrailleNote> Notes { get; set; }


        public BrailleVoice()
        {
            Notes = new List<BrailleNote>();
        }

        /*public void AppendNotes(BrailleNote[] n)
        {
            Notes = Notes.Concat(n).ToArray();
        }
        public void AppendChords(Chord[] c)
        {
            Chords = Chords.Concat(c).ToArray();
        }*/

        public void Parse(BrailleText brailleText)
        {
            foreach (BrailleNote N in Notes)
            {
                N.Parse(brailleText);
            }
        }
    }
}
