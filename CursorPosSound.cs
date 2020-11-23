using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Linq;
using Manufaktura.Controls.Parser;
using Manufaktura.Model.MVVM;
using Manufaktura.Controls;
using Manufaktura.Controls.WPF;
using Manufaktura.Music;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model.MajorAndMinor;
using Manufaktura.Music.Model;

namespace LiveDots
{
    public class CursorPosSound
    {
        private bool played;
        ScorePlayer reproductor;
        Score nota;
        public CursorPosSound()
        {
            played = false;
            nota = Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
            //reproductor = new MyMidiTaskScorePlayer(nota);
        }
        Score creaScore()
        { //score.FirstStaff.Elements.Add(new Note(Pitch.C5, RhythmicDuration.Quarter));
            return Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
        }
        public void play(Pitch pitch, RhythmicDuration duration)
        {
            nota.FirstStaff.Elements.Add(new Note(pitch, duration));
            //if (reproductor != null) ((IDisposable)reproductor).Dispose();
          
            reproductor.Play();
            //nota.FirstStaff.Elements.Clear();
            played = true;

        }
        public void setPlay(bool p)
        {
            played = p;
        }
    }
}
