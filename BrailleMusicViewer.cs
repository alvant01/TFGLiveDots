using System;
using System.Collections.Generic;

namespace LiveDots
{
    public class BrailleMusicViewer
    {
        MainWindow MyMainWindow;

        List<String> Viewer { get; set; }
        List<int> Forward;
        List<int> Backward;

        int Current;

        public BrailleMusicViewer()
        {
            Viewer = new List<String>();
            Forward = new List<int>();
            Backward = new List<int>();
            Backward.Add(0);
            Current = 0;
        }
        

        //devuelve el texto entero
        public string GetText()
        {
            string txt = "";
            foreach (string s in Viewer)
            {
                txt += s;
            }
            return txt;
        }

        public int GetCurrent()
        {
            return Current;
        }

        public string GetElement(int n)
        {
            return Viewer[n];
        }

        /*public int GetForward(int n)
        {
            return Forward[n];
        }
        public int GetBackward(int n)
        {
            return Backward[n];
        }*/

        public int GetCurrentForward()
        {
            return Forward[Current];
        }

        internal void AddEnd()
        {
            Forward.Add(0);
            Viewer.Add("Final de partitura");
        }

        public int GetCurrentBackward()
        {
            return Backward[Current];
        }

        public string GetElement()
        {          
                return Viewer[Current];       
        }

        public void UpdateIndex(int index)
        {
            Current = index;
        }

        public void AddElement(String txt, int tam)
        {
            for (int i = 0; i < tam; ++i)
            {
                Viewer.Add(txt);
                Forward.Add(tam - i);
                Backward.Add(i + 1);
            }
        }
        public void PopElement()
        {
            Viewer.RemoveAt(Viewer.Count - 1);
            Forward.RemoveAt(Forward.Count - 1);
            Backward.RemoveAt(Backward.Count - 1);
        }
    }
}
