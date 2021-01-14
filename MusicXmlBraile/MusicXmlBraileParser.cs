using MusicXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LiveDots.MusicXmlBraile
{
    class MusicXmlBraileParser
    {
        public string createXML(BrailleScore bs)
        {
            string xml = "";

            int countPart = 1;
            foreach (BraillePart part in bs.Parts)
            {
                xml += "<score-partwise>\r\n";

                xml += "<part-list>\r\n";
                xml += " <score-part id=\"P" + countPart + "\">\r\n";
                
                xml += "</score-part>\r\n";
                xml += "</part-list>\r\n";

                xml += "<part id=\"P" + countPart + "\">\r\n";

                xml = measureXML(xml, part.Measures);

                xml += "</part>\r\n";
                xml += "</score-partwise>\r\n";

                countPart++;
            }

            return xml;
        }

        private string measureXML(string xml, List<BrailleMeasure> measures)
        {
            int countMeasure = 1;
            foreach (BrailleMeasure bm in measures)
            {
                
                xml += "<measure number=\"" + countMeasure + "\">\r\n";
                xml = measureXML(xml, bm, countMeasure);
                if (measures.Count == countMeasure)
                {
                    xml += "<barline location = \"right\" > <bar-style> light-heavy </bar-style> </barline>\r\n";
                }
                xml += "</measure>\r\n";
                countMeasure++;
            }
            return xml;
        }

        public string measureXML(string xml, BrailleMeasure bm, int count)
        {
            if (count == 1)
            {
                xml += "<attributes>\r\n";

                xml += "<clef>\r\n";
                xml += "<sign>" + bm.Attribute.Clef.Sign + "</sign>\r\n";
                xml += "<line>" + bm.Attribute.Clef.Line + "</line>\r\n";
                xml += "</clef>\r\n";

                xml += "<key>\r\n";
                xml += "<fifths>" + bm.Attribute.Key.Fifths + "</fifths>\r\n";
                xml += "</key>\r\n";

                xml += "<time>\r\n";
                xml += "<beats>" + bm.Attribute.Time.Beats + "</beats>\r\n";
                xml += "<beat-type>" + bm.Attribute.Time.BeatType + "</beat-type>\r\n";
                xml += "</time>\r\n";

                xml += "</attributes>\r\n";
            }
            foreach (BrailleStaff staff in bm.Staffs)
            {
                xml = staffXML(xml, staff.Voices);
            }
            return xml;
        }

        private string staffXML(string xml, List<BrailleVoice> voices)
        {
            foreach (BrailleVoice bv in voices)
            {
                xml = voiceXML(xml, bv);
            }
            return xml;
        }
        public string voiceXML(string xml, BrailleVoice bv)
        {
            foreach (BrailleNote note in bv.Notes)
            {
                xml = noteXML(xml, note);
            }
            return xml;
        }
        public string noteXML(string xml, BrailleNote bn)
        {
            xml += "<note>\r\n";
            if (bn.IsRest)
            {
                

                xml += "<rest/>\r\n";
                xml += "<duration>" + bn.duration + "</duration>\r\n"; //mirar
                xml += "<voice>" + 1 + "</voice>\r\n"; //mirar
                xml += "<type>" + bn.Type + "</type>\r\n";
            }
            else 
            {
                xml += "<pitch>\r\n";
                xml += "<step>" + bn.Step + "</step>\r\n";
                xml += "<octave>" + bn.getToctaveNum() + "</octave>\r\n";
                xml += "</pitch>\r\n";
                xml += "<duration>" + bn.duration + "</duration>\r\n"; //mirar
                xml += "<voice>" + 1 + "</voice>\r\n"; //mirar
                xml += "<type>" + bn.Type + "</type>\r\n";
                xml += "<stem>up</stem>\r\n";

            }
            xml += "</note>\r\n";

            return xml;

        }
    }
}
