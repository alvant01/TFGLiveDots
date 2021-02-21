using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleVoice : BrailleElement
    {
        public int StartingTime { get; set; }

        public int EndingTime { get; set; }

        public List<BrailleNote> Notes { get; set; }

        public List<BrailleChord> Chords { get; set; }

        public BrailleVoice()
        {
            Notes = new List<BrailleNote>();
            Chords = new List<BrailleChord>();
        }

        public BrailleVoice(int st, int et, List<BrailleNote> n, List<BrailleChord> c)
        {
            StartingTime = st;
            EndingTime = et;
            Notes = n;
            Chords = c;
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
