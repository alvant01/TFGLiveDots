using Manufaktura.Music.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LiveDots
{
    
    class NoteDictionary
    {
        public Dictionary<string, NoteForSound> _NoteDic;
        readonly string[] octavas;
        public NoteDictionary(XmlDocument notasXML)
        {
            _NoteDic = new Dictionary<string, NoteForSound>();
            rellenarString(out octavas);
            NoteForSound noteForSound = new NoteForSound();
            //XmlNodeList octaves = notasXML.GetElementsByTagName("Octavas");
            /*foreach (XmlNode node in notasXML.DocumentElement.ChildNodes)
            {
                string text = node.InnerText; //or loop through its children as well
            }*/
            XmlNodeList tonos = notasXML.GetElementsByTagName("Nota");
            for (int j = 0; j < tonos.Count; j++)
            {
                noteForSound = NoteForSound.TransformStringToNote(tonos[j].InnerText.ToString());
                //noteForSound.rhythmicDuration_ = RhythmicDuration.;
                _NoteDic.Add(tonos[j].InnerText.ToString(), noteForSound);
            }
            NoteForSound aux = _NoteDic["C1"];
        }

        private void rellenarString(out string[] o)
        {
            o = new string[6];
            o[0] = "Primera";
            o[0] = "Segunda";
            o[0] = "Tercera";
            o[0] = "Cuarta";
            o[0] = "Quinta";
            o[0] = "Sexta";
        }
    }
}
