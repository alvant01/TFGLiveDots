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
        /*
        public BrailleMusicViewer(BrailleScore score)
        {
            Viewer = new List<String>();
            Forward = new List<int>();
            Backward = new List<int>();
            Backward.Add(0);
            Current = 0;

            score.ParseText(this);
            Forward.Add(0); // Ultimo forward para que no avance al final.
        }
        */

        public void SetMainWindow(MainWindow window)
        {
            MyMainWindow = window;
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

        public int GetForward(int n)
        {
            return Forward[n];
        }

        public int GetCurrentForward()
        {
            return Forward[Current];
        }

        internal void AddEnd()
        {
            Forward.Add(0);
            Viewer.Add("Final de partitura");
        }

        // Función para borrar el espacio que se quita de la barra final
        /*
        internal void DeleteLastElement()
        {
            Viewer.RemoveAt(Viewer.Count()-1);
            Forward.RemoveAt(Viewer.Count() - 1);
            Backward.RemoveAt(Viewer.Count() - 1);
        }
        */

        public int GetBackward(int n)
        {
            return Backward[n];
        }

        public int GetCurrentBackward()
        {
            return Backward[Current];
        }

        public string GetElement()
        {
            return Viewer[Current];
        }

        /*
        public string GetNextElement()
        {
            string txt = Viewer[Current];
            Current += Forward[Current];
            this.MyMainWindow.Dispatcher.Invoke(() =>
            {
                MyMainWindow.text1.CaretIndex = Current;
            });
            
            return txt;
        }

        public string GetPreviousElement()
        {
            
            Current -= Backward[Current];
            string txt = Viewer[Current];
            this.MyMainWindow.Dispatcher.Invoke(() =>
            {
                MyMainWindow.text1.CaretIndex = Current;
            });
            return txt;
        }
        */
        public void UpdateIndex(int index)
        {
            Current = index;
        }

        /*
        public void UpdateCaret()
        {
            this.MyMainWindow.Dispatcher.Invoke(() =>
            {
                MyMainWindow.text1.CaretIndex = Current;
            });
        }
        
        public bool TextBoxFocused()
        {
            bool HasFocus = false;
            this.MyMainWindow.Dispatcher.Invoke(() =>
            {
                HasFocus = MyMainWindow.text1.IsFocused;
            });
            return HasFocus;
        }
        */

        public string PrintForward()
        {
            string txt = "";
            for (int i = 0; i < Forward.Count; ++i)
            {
                txt += " " + Forward[i].ToString();
            }
            return txt;
        }

        public bool IsInMiddle()
        {
            return Current > 0 && Current < Viewer.Count && (Viewer[Current] == Viewer[Current - 1]);
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
