using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleText
    {
        public const int LineSize = 40;
        List<String> Text;
        BrailleMusicViewer Viewer;
        private int LineCount;

        public BrailleText()
        {
            Text = new List<string>();
            Viewer = new BrailleMusicViewer();
            LineCount = 0;
        }

        public BrailleText(BrailleScore score)
        {
            Text = new List<string>();
            Viewer = new BrailleMusicViewer();
            LineCount = 0;
            score.Parse(this);
        }
        public BrailleText(BrailleScore score, BrailleText bt)
        {
            Text = new List<string>();
            Viewer = new BrailleMusicViewer();
            LineCount = 0;
            
        }

        public void AddText(List<string> element)
        {
            if (LineCount + element.Count > LineSize)
            {
                JumpLine();
            }
            Text.AddRange(element);
            LineCount += element.Count;
        }

        public List<String> getText()
        {
            return this.Text;
        }
        public void AddViewer(string element, int tam)
        {
            Viewer.AddElement(element, tam);
        }

        internal void JumpLine()
        {
            Text.Add("\n");
            LineCount = 0;
            Viewer.AddElement("Salto de línea ", 1);
        }

        public void AddSpace(int n = 1)
        {
            for (int i = 0; i < n; ++i)
            {
                this.AddText(new List<string> { " " });
                this.AddViewer("Espacio ", 1);
            }
        }

        public void AddScoreEnd()
        {
            if (Text[Text.Count - 1] == " " || Text[Text.Count - 1] == "")
            {
                Text[Text.Count - 1] = "126";
                Viewer.PopElement();
            }
            else
            {
                Text.Add("126");
            }
            Text.Add("13");

            Viewer.AddElement("Barra final ", 2);
            Viewer.AddEnd();
        }

        public string GetBrailleString()
        {
            string braille = "";
            DicBraille dic = new DicBraille();
            foreach (string s in Text)
            {
                braille += dic.getTranslation(s);
            }
            return braille;
        }

        public BrailleMusicViewer GetViewer()
        {
            return Viewer;
        }

        public void concatenate(BrailleText text)
        {
            this.AddText(text.getText());
        }

    }
}
