using System;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using System.Xml;
using System.Threading.Tasks;


namespace LiveDots
{
    public class CursorPosSound
    {
        protected MainWindow mainWindow;
        readonly NoteDictionary noteDic;

        public CursorPosSound(MainWindow viewModel)
        {
            mainWindow = viewModel;

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
        }
        private void Play(Pitch pitch, RhythmicDuration duration)
        {                           
            mainWindow.player.PlayElement(new Note(pitch, duration));              
            //https://guitargearfinder.com/guides/convert-ms-milliseconds-bpm-beats-per-minute-vice-versa/             
            //https://rechneronline.de/musik/note-length.php            
        }
        public void testOfTransformStringToNote(ref string ViewerValue,ref string octava)
        {
            ViewerValue = StringToNote.GetResNote(ViewerValue);
            octava = StringToNote.GetNote(octava);
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
        public RhythmicDuration GetRhythmicDuration(string ViewerValue)
        {
            string nota = StringToNote.GetResNote(ViewerValue);
            return nota == null ? RhythmicDuration.Quarter : StringToNote.SetRitmo(nota) ; 
        }
    }
}
