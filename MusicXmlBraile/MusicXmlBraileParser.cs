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
        public string createXML(BXBrailleScore bs)
        {
            string xml = "";

            int countPart = 1;
            foreach (BXBraillePart part in bs.Parts)
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

        private string measureXML(string xml, List<BXBrailleMeasure> measures)
        {
            int countMeasure = 1;
            foreach (BXBrailleMeasure bm in measures)
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

        public string measureXML(string xml, BXBrailleMeasure bm, int count)
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
            foreach (BXBrailleStaff staff in bm.Staffs)
            {
                xml = staffXML(xml, staff.Voices);
            }
            return xml;
        }

        private string staffXML(string xml, List<BXBrailleVoice> voices)
        {
            foreach (BXBrailleVoice bv in voices)
            {
                xml = voiceXML(xml, bv);
            }
            return xml;
        }
        public string voiceXML(string xml, BXBrailleVoice bv)
        {
            foreach (BXBrailleNote note in bv.Notes)
            {
                xml = noteXML(xml, note);
            }
            return xml;
        }
        public string noteXML(string xml, BXBrailleNote bn)
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
                if(bn.getToctaveNum() <=4)
                    xml += "<stem>up</stem>\r\n";
                else
                    xml += "<stem>down</stem>\r\n";

            }
            xml += "</note>\r\n";

            return xml;

        }
    }
}
