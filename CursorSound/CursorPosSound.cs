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
using System.Xml;

namespace LiveDots
{
    public class CursorPosSound
    {
        private bool played;
        protected MainWindow mainWindow;
        NoteDictionary noteDic;

        //StaffForNotes myMusical = new StaffForNotes();
        //string clave_;
        public CursorPosSound(MainWindow viewModel)
        {
            mainWindow = viewModel;
            played = true;

            //if not found create xml
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load("Notas.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " -XML not found");
                CreateXML.createDocument();
            }
            noteDic = new NoteDictionary(xDoc);

            //nota = Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
            //reproductor = new MyMidiTaskScorePlayer(nota, new MidiDevice(2, "Cursor")); // cannot insert different devices, no idea why
        }
        /*public void setClave(string clave){
            clave_ = clave;
        }*/
        private void Play(Pitch pitch, RhythmicDuration duration)
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
        public bool GetPlay()
        {
            return played;
        }
        public void SetPlay(bool p)
        {
            played = p;
        }

        public void TransformStringToNoteAndPlay(string ViewerValue, string octava)
        {
            string nota = StringToNote.getNote(ViewerValue);
            octava = StringToNote.GetNote(octava);
            if (nota != null)
            {
                Pitch pitch = noteDic._NoteDic[nota[0] + octava].pitch_;
                RhythmicDuration rhythmicDuration = StringToNote.SetRitmo(nota);
                Play(pitch, rhythmicDuration);
            }
        }
    }
}

/*Score creaScore()
{ //score.FirstStaff.Elements.Add(new Note(Pitch.C5, RhythmicDuration.Quarter));
return Score.CreateOneStaffScore(Clef.Treble, new MajorScale(Manufaktura.Music.Model.Step.C, false));
}*/