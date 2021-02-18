using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Manufaktura.Music.Model; 
namespace LiveDots
{

    public class NoteForSound
    {
        //public Pitch pitch_ { get; set; }
        public Pitch pitch_;


        // public RhythmicDuration rhythmicDuration_;
        /* public NoteForSound(Pitch p, RhythmicDuration r)
         {
             pitch_ = p;
             rhythmicDuration_ = r;
         }*/

        /*public bool Equals(NoteForSound noteForSound)
        {
            return noteForSound.pitch_.Equals(pitch_) &&
                noteForSound.rhythmicDuration_.Equals(rhythmicDuration_);
        }
        public override bool Equals(object o) {
            return this.Equals(o as NoteForSound);
        }

        public override int GetHashCode() => pitch_.GetHashCode() ^ rhythmicDuration_.GetHashCode();*/

        public static NoteForSound TransformStringToNote(string note)
        {
            NoteForSound n = new NoteForSound();

            switch (note[0])
            {
                case 'A':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.A1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.A2;
                            break;                        
                        case '3':
                            n.pitch_ = Pitch.A3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.A4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.A5;
                            break; 
                        case '6':
                            n.pitch_ = Pitch.A6;
                            break;
                    }
                    break;
                case 'B':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.B1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.B2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.B3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.B4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.B5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.B6;
                            break;
                    }
                    break;
                case 'C':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.C1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.C2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.C3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.C4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.C5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.C6;
                            break;
                    }
                    break;
                case 'D':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.D1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.D2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.D3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.D4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.D5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.D6;
                            break;
                    }
                    break;
                case 'E':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.E1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.E2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.E3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.E4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.E5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.E6;
                            break;
                    }
                    break;
                case 'F':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.F1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.F2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.F3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.F4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.F5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.F6;
                            break;
                    }
                    break;
                case 'G':
                    switch (note[1])
                    {
                        case '1':
                            n.pitch_ = Pitch.G1;
                            break;
                        case '2':
                            n.pitch_ = Pitch.G2;
                            break;
                        case '3':
                            n.pitch_ = Pitch.G3;
                            break;
                        case '4':
                            n.pitch_ = Pitch.G4;
                            break;
                        case '5':
                            n.pitch_ = Pitch.G5;
                            break;
                        case '6':
                            n.pitch_ = Pitch.G6;
                            break;
                    }
                    break;
            }
            return n;
        }
    }
}
