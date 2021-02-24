using System;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using System.Xml;
using System.Threading.Tasks;


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
        private async void Play(Pitch pitch, RhythmicDuration duration)
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
                mainWindow.player.PlayElement(new Note(pitch, duration));

                // https://physics.stackexchange.com/questions/15900/converting-notes-to-milliseconds https://guitargearfinder.com/guides/convert-ms-milliseconds-bpm-beats-per-minute-vice-versa/
                //https://rechneronline.de/musik/note-length.php
                //varia con el tempo

                //temporal con el tempo de 100
                int tempo = mainWindow.player.Tempo.BeatsPerMinute;
                int timespan = 0;
                var aux = duration.ToTimeSpan(mainWindow.player.Tempo);

                if (duration == RhythmicDuration.Eighth)
                {
                    timespan = 300;
                }
                else if (duration == RhythmicDuration.Quarter)
                {
                    timespan = 600;
                }

                else if (duration == RhythmicDuration.Eighth)
                {
                    timespan = 1200;
                }
                else if (duration == RhythmicDuration.Whole)
                {
                    timespan = 2400;
                }
                //esperar a la duracion de la nota
                await Task.Delay(aux);
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
            string nota = StringToNote.GetResNote(ViewerValue);
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