using MusicXml;
using MusicXml.Domain;
using System;
using System.Collections.Generic;

namespace LiveDots
{
    /* En esta clase se incluiran los metodos necesarios para convertir entre
     * un arbol XML y un arbol de Braille.
     */
    public static class Converter
    {

        static BrailleNote Xml2Note(Note note)
        {
            BrailleNote n;
            if (!note.IsRest)
            {
                n = new BrailleNote
                {
                    IsRest = note.IsRest,
                    Type = note.Type,
                    Octave = (Octave)(note.Pitch.Octave - 1), //-1 por conversion al enumerado.
                    Step = (Step)(note.Pitch.Step - 'A'),
                    Alter = note.Pitch.Alter,
                    Staff = note.Staff,
                    Accidental = note.Accidental
                };
            }
            else
            {
                n = new BrailleNote
                {
                    IsRest = note.IsRest,
                    Type = note.Type,
                    Staff = note.Staff
                };
            }

            return n;
        }

        public static BrailleScore Xml2Braille(string filename)
        {
            return treeXml2Braille(MusicXmlParser.GetScore(filename));
        }


        public static List<List<int>> GenerateAlters(BrailleAttribute attr)
        {
            List<List<int>> L = new List<List<int>>(8);
            for (int i = 0; i < 8; ++i)
            {
                L.Add(new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0 }));
                if (attr.Key.getAlters() != null || attr.Key.getAlters().Count != 0)
                    L[i] = attr.Key.getAlters();

            }
            return L;
        }


        static BrailleScore treeXml2Braille(Score score)
        {
            BrailleScore s = new BrailleScore();

            for (int i = 0; i < score.Parts.Count; ++i)
            {
                BraillePart p = new BraillePart();
                BrailleNote nLast = null; // Needed to compare and see if the octave is needed.
                List<List<int>> AlterAux;

                int lastKey = 0;
                for (int j = 0; j < score.Parts[i].Measures.Count; ++j)
                {
                    int temp = 0;

                    BrailleMeasure m = new BrailleMeasure();
                    if (score.Parts[i].Measures[j].Attributes != null)
                    {
                        m.Attribute = Xml2Attribute(score.Parts[i].Measures[j].Attributes);
                        m.Attribute.First = j == 0;
                        if (m.Attribute.Key != null) lastKey = j;
                    }

                    AlterAux = GenerateAlters(Xml2Attribute(score.Parts[i].Measures[lastKey].Attributes));

                    m.Staffs.Add(new BrailleStaff());
                    for (int k = 0; k < score.Parts[i].Measures[j].MeasureElements.Count; ++k)
                    {
                        switch (score.Parts[i].Measures[j].MeasureElements[k].Type)
                        {
                            case MeasureElementType.Note:
                                Note noteAux = (Note)score.Parts[i].Measures[j].MeasureElements[k].Element;
                                BrailleNote n = Xml2Note(noteAux);

                                n.GiveAccidental(AlterAux);

                                while (n.Staff >= m.Staffs.Count) // Creemos que los Staffs se numeran desde el 0.
                                {
                                    m.Staffs.Add(new BrailleStaff());
                                }
                                BrailleStaff st;
                                if (n.Staff >= 0)
                                    st = m.Staffs[n.Staff - 1];
                                else st = m.Staffs[m.Staffs.Count - 1];

                                while (noteAux.Voice > st.Voices.Count)
                                {
                                    st.Voices.Add(new BrailleVoice());
                                }
                                BrailleVoice v = st.Voices[noteAux.Voice - 1];

                                BrailleNote.CheckLast(nLast, n);

                                v.Notes.Add(n);

                                nLast = n;
                                temp += noteAux.Duration;
                                break;
                            case MeasureElementType.Backup:
                                Backup backupAux = (Backup)score.Parts[i].Measures[j].MeasureElements[k].Element;
                                temp -= backupAux.Duration;
                                break;
                            case MeasureElementType.Forward:
                                Forward forwardAux = (Forward)score.Parts[i].Measures[j].MeasureElements[k].Element;
                                temp -= forwardAux.Duration;
                                break;
                        }
                    }

                    p.Measures.Add(m);
                }

                s.Parts.Add(p);
            }

            return s;
        }

        static BrailleAttribute Xml2Attribute(MeasureAttributes attribute)
        {

            BrailleAttribute attr = new BrailleAttribute();
            if (attribute.Key != null)
                attr.Key = new BrailleKey { Fifths = attribute.Key.Fifths, Mode = attribute.Key.Mode };
            if (attribute.Clef != null)
                attr.Clef = new BrailleClef { Line = attribute.Clef.Line, Sign = attribute.Clef.Sign };
            if (attribute.Time != null)
            {
                attr.Time = new BrailleTime { Beats = attribute.Time.Beats };
                if (attribute.Time.Mode != "")
                    attr.Time.BeatType = Convert.ToInt32(attribute.Time.Mode);
                attr.Time.Symbol = SymbolConverter(attribute.Time.Symbol);
            }

            return attr;
        }

        static Time_Symbol SymbolConverter(TimeSymbol symbol)
        {
            Time_Symbol sym = new Time_Symbol();
            switch (symbol)
            {
                case TimeSymbol.Normal: sym = Time_Symbol.Normal; break;
                case TimeSymbol.Common: sym = Time_Symbol.Common; break;
                case TimeSymbol.Cut: sym = Time_Symbol.Cut; break;
                case TimeSymbol.SingleNumber: sym = Time_Symbol.SingleNumber; break;
            }
            return sym;
        }
    }
}
