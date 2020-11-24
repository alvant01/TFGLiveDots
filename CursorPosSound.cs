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
using Manufaktura.Controls.Desktop.Audio.Midi;

namespace LiveDots
{
    public class CursorPosSound
    {
        private bool played;
        ScorePlayer reproductor;
        Score nota;// partitura
        public CursorPosSound(ScorePlayer r) // le paso scoreplayer aunque en realidad no es muy necesario si se pudiera usar un mismo midi
        {
            played = true;
            nota = Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
            /*no creo que  sea la mejor manera de hacerlo, prefiero crear un musicalSymbol y usar eso para tocar la nota, en vez de crear una partitura todo el rato
             * MusicalSymbol no tiene nada de documentacion en internet
             */
            if (reproductor != null) ((IDisposable)reproductor).Dispose();
            reproductor = r;
            reproductor = new MyMidiTaskScorePlayer(nota);
            //reproductor = new MyMidiTaskScorePlayer(nota, new MidiDevice(2, "Cursor")); // cannot insert different devices, no idea why
        }

        public void play(Pitch pitch, RhythmicDuration duration)
        {
            if (!played)
            {
                // no consigo hacer que se reproduzca una nota, se van añadiendo
                nota.FirstStaff.Elements.Add(new Note(pitch, duration));
                reproductor.Play();
                nota.FirstStaff.Elements.Clear(); // no borra los elementos introducidos
                reproductor.PlayElement(new Note(pitch,duration));// ? No se como pasarle un MusicalSymbol
                played = true;
            }

        }
        public void setPlay(bool p)
        {
            played = p;
        }
        /*Score creaScore()
{ //score.FirstStaff.Elements.Add(new Note(Pitch.C5, RhythmicDuration.Quarter));
    return Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
}*/
    }
}
