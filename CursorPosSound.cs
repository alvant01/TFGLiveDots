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
        protected MainWindow mainWindow;
        StaffForNotes myMusical = new StaffForNotes();
        //string clave_;
        public CursorPosSound(MainWindow viewModel)
        {
            mainWindow = viewModel;
            played = true;

            //nota = Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
            //reproductor = new MyMidiTaskScorePlayer(nota, new MidiDevice(2, "Cursor")); // cannot insert different devices, no idea why
        }
        /*public void setClave(string clave){
            clave_ = clave;
        }*/
        public void play(Pitch pitch, RhythmicDuration duration)
        {
            if (!played)
            {
                /*
                nota.FirstStaff.Elements.Clear(); // no borra los elementos introducidos
                */
                Note note = new Note();
                //cambiando esto no funciona
                note.Pitch.StepName = Manufaktura.Music.Model.Step.B;

                //Intento de hacer staff para que no pete el error del playElement, no funciona
                /*Staff staff = new Staff();
                note.Staff = staff;*/
               //note.Staff =
                mainWindow.player.PlayElement(new Note(pitch,duration));
                played = true;
            }

        }
        public bool getPlay()
        {
            return played;
        }
        public void setPlay(bool p)
        {
            played = p;
        }
    }
}

/*Score creaScore()
{ //score.FirstStaff.Elements.Add(new Note(Pitch.C5, RhythmicDuration.Quarter));
return Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
}*/