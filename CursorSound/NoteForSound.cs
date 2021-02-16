using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufaktura.Music.Model; 
namespace LiveDots
{
    public class NoteForSound
    {
        Pitch pitch_;
        RhythmicDuration rhythmicDuration_;
        public NoteForSound(Pitch p, RhythmicDuration r)
        {
            pitch_ = p;
            rhythmicDuration_ = r;
        }
    }
}
