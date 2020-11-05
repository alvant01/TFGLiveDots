using System;
using System.Runtime.InteropServices;

namespace LiveDots
{
    [Guid("3651AB8D-D999-423E-806B-29B4B1D6994F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface LiveDotsCOMObj_Interface
    {
        [DispId(1)]
        string GetLine();
        [DispId(2)]
        string GetChar();
        [DispId(3)]
        string GetBackSpace();
        [DispId(4)]
        string GetDeleted();
        [DispId(5)]
        string GetHightLightedText();
        [DispId(6)]
        string GetCharacter(int c, string b);
        [DispId(7)]
        string GetObjectTypeAndText(int x);
        [DispId(8)]
        string GetAll();
        [DispId(9)]
        string GetVerbText(int c, string b);
        [DispId(10)]
        string IsHandled(int handle);
        [DispId(11)]
        string getErrorCode();
        [DispId(12)]
        int isBrailleLine();
        [DispId(13)]
        string SayFromCursor();
        [DispId(14)]
        string SayToCursor();
        [DispId(15)]
        string SayWord();
        [DispId(16)]
        string SayCharacterPhonetic();
        [DispId(17)]
        string SpellWord();
        [DispId(18)]
        string GetStatusBar();
        [DispId(19)]
        bool HasObject();
        [DispId(20)]
        string GetCharJawsCursor(int handle, int x, int y);
        [DispId(21)]
        string GetLineJawsCursor(int handle, int x, int y);
        [DispId(22)]
        string GetParagraph();
        [DispId(23)]
        string GetElement();
        [DispId(24)]
        bool IsTextBoxFocused();
    }

    [Guid("0B1BDE4B-20D8-4C73-8062-6680F04F8F98")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface LiveDotsCOMObj_Events
    {
    }

    [ProgId("LiveDots.LiveDotsCOMObj")]
    [ComVisible(true)]
    [Guid("64550ee5-5255-4c18-98f0-71ec898fe97c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(LiveDotsCOMObj_Interface))]
    [ComSourceInterfaces(typeof(LiveDotsCOMObj_Events))]
    public class LiveDotsCOMObj : LiveDotsCOMObj_Interface
    {
        [DllImport("oleaut32.dll")]
        private static extern int RegisterActiveObject([MarshalAs(UnmanagedType.IUnknown)] object punk, ref Guid rclsid, uint dwFlags, out int pdwRegister);
        [DllImport("oleaut32.dll")]
        private static extern int RevokeActiveObject(int register, IntPtr reserved);

        static LiveDotsCOMObj livedotsCOMObj = null;
        static RegistrationServices regServices;
        static int cookie, cookieType;
        static string error_code = "wwwxwz54";
        // Para pruebas
        static BrailleMusicViewer viewer;
        public static MainWindow MainWindow;
        bool CheckFocused = false;

        public static void Register()
        {
            if (livedotsCOMObj == null)
                livedotsCOMObj = new LiveDotsCOMObj();
            Guid clsidApp = new Guid("64550ee5-5255-4c18-98f0-71ec898fe97c");
            RegisterActiveObject(livedotsCOMObj, ref clsidApp, 0, out cookie);
            regServices = new RegistrationServices();
            cookieType = regServices.RegisterTypeForComClients(typeof(LiveDotsCOMObj), RegistrationClassContext.LocalServer, RegistrationConnectionType.SingleUse);

            //quitar esto de aquí
        }

        public static void Unregister()
        {
            RevokeActiveObject(cookie, IntPtr.Zero);
            regServices.UnregisterTypeForComClients(cookieType);
        }

        public string GetElement()
        {
            if (viewer != null)
                return viewer.GetElement();
            return null;
        }

        public bool IsTextBoxFocused()
        {
            bool selected = false, aux;
            MainWindow.Dispatcher.Invoke(() =>
            {
                selected = MainWindow.text1.IsFocused;
            });
            aux = selected && !CheckFocused;
            CheckFocused = selected;
            return aux;
        }

        public static void SetMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public string GetLine()
        {
            return "He getteado la linea";
        }

        public string GetLineJawsCursor(int handle, int y, int x)
        {
            return "He getteado la linea jaws cursor";

        }

        public string GetChar()
        {
            return "He getteado el char";
        }

        public string GetCharJawsCursor(int handle, int y, int x)
        {

            return "He getteado el char jaws cursor";
        }

        public string GetBackSpace()
        {
            return "He getteado el back space";
        }

        public string GetDeleted()
        {
            return "He getteado deleted";
        }

        public string GetHightLightedText()
        {
            return "He getteado highlighted text";
        }
        public string GetCharacter(int c, string b)
        {

            return "He getteado el character";

        }
        public string GetVerbText(int c, string b)
        {
            return "He getteado verbtext";
        }
        public string IsHandled(int handle)
        {
            return "is handled";
        }


        public string GetObjectTypeAndText(int x)
        {
            return "He getteado object and type text";
        }

        public string GetAll()
        {
            return "He getteado allllll";
        }

        public string getErrorCode()
        {
            return error_code;
        }

        public int isBrailleLine()
        {
            /*if (rtb != null)
            {
                try
                {
                    //Debug.WriteLine("BRAILLLLLLLLLE: " + rtb.braille);
                    int braille = rtb.braille;
                    rtb.braille = 0;
                    return braille;
                }
                catch { }
            }*/
            return 0;
        }

        public string SayFromCursor()
        {
            return "toma from cursor";
        }

        public string SayToCursor()
        {
            return "Toma to cursor";
        }

        public string SayWord()
        {
            return "word";
        }

        public string SayCharacterPhonetic()
        {
            return "character phonetic";
        }

        public string SpellWord()
        {
            return "spellword";
        }

        public string GetStatusBar()
        {
            return "status bar";
        }

        public bool HasObject()
        {
            //Debug.WriteLine(viewer != null ? true : false); 
            return viewer != null ? true : false;
            //return false;
        }

        public string GetParagraph()
        {
            return "He getteado el parrafo";
        }


        /*
        public static void SetMainView(MainView main_view)
        {
            mainView = main_view;
        }
        */
        public static void SetCurrent(BrailleMusicViewer view)
        {
            viewer = view;
        }

        public static BrailleMusicViewer GetCurrent()
        {
            return viewer;
        }

        public static void UnSetCurrent()
        {
            viewer = null;
        }
        /*
            public bool TextBoxFocused()
            {
                if(viewer != null)
                {
                    return viewer.TextBoxFocused();
                }
                return false;
            }

        public string GetLine()
           {
               try
               {
                   //if (rtb != null) return rtb.verbalizationLine();
               }
               catch { }
               return "Hola caracola";
               return null;
           }

           public string GetLineJawsCursor(int handle, int y, int x)
           {
               try
               {
                   //Debug.WriteLine(rtb.Handle.ToString() + " GetLine " + handle + ", " + x + ", " + y);
                   if (rtb != null && rtb.Handle.ToString() == handle.ToString())
                   {
                       int index = rtb.GetCharIndexFromPosition(new System.Drawing.Point(x, y));
                       int line_destination = rtb.getRealLine(index);
                       return rtb.verbalizationLine(line_destination);
                   }
               }
               catch { }
               //if (rtb != null) return rtb.VerbalizationCharacter;
               return error_code;

           }

           public string GetChar()
           {
               //Debug.WriteLine("GetChar");
               try
               {
                   if (rtb != null) return rtb.verbalizationCharacter();
               }
               catch { }
               return null;
           }

           public string GetCharJawsCursor(int handle, int y, int x)
           {
               //Debug.WriteLine(rtb.Handle.ToString() + " GetChar " + handle + ", " + x + ", " + y);
               try
               {
                   if (rtb != null && rtb.Handle.ToString() == handle.ToString())
                   {
                       return rtb.verbalizationPosition(x, y);
                   }
               }
               catch { }
               //if (rtb != null) return rtb.VerbalizationCharacter;
               return error_code;
           }

           public string GetBackSpace()
           {
               if (rtb != null)
               {
                   try
                   {
                       //Debug.WriteLine("GetBackSpace ------------------    " + rtb.VerbalizationBackSpace);
                       string txt = rtb.verbalizationBackspace();
                       //Debug.WriteLine("GetBackSpace ------------------    " + txt);
                       return txt;
                   }
                   catch { }
               }
               return null;
           }

           public string GetDeleted()
           {
               if (rtb != null)
               {
                   try
                   {
                       //Debug.WriteLine("GetDeleted ------------------    " + rtb.VerbalizationDeleted);
                       string txt = rtb.verbalizationDeleted();
                       //Debug.WriteLine("GetDeleted ------------------    " + txt);
                       return txt;
                   }
                   catch { }
               }
               return null;
           }

           public string GetHightLightedText()
           {
               if (rtb != null)
               {
                   try
                   {
                       //Debug.WriteLine("GetHightLightedText");
                       string text = rtb.verbalizationHightlighted();
                       if (text != "")
                           return text;//Resources.Resources.selected;
                       else
                           return "";
                   }
                   catch { return ""; }
               }
               else return error_code;
           }
           public string GetCharacter(int c, string b)
           {

               if (rtb != null)
               {
                   //Debug.WriteLine("GetCharacter    " + c + " --------" + rtb.Handle.ToString());
                   //Debug.WriteLine(b);
                   try
                   {
                       string txt = rtb.verbalizationCharacter(b);
                       return txt;
                   }
                   catch { }
               }
               return null;

           }
           public string GetVerbText(int c, string b)
           {
               if (rtb != null)
               {
                   string txt = "";
                   try
                   {
                       //Debug.WriteLine("GetVerbText    " + c + " --------" + rtb.Handle.ToString());
                       //Debug.WriteLine(c + " " + b);
                       if (rtb.Handle.ToString() == c.ToString())
                       {
                           txt = rtb.verbalizationMathText(b);
                           //Debug.WriteLine("GetCharacter    verb " + txt);
                       }//else return rtb.verbalizationMathText(b);
                        //txt = rtb.verbalizationMathText(b);
                        //Debug.WriteLine("GetCharacter    verb " + txt);
                   }
                   catch { }
                   return txt;
               }
               else
                   return null;
           }
           public string IsHandled(int handle)
           {
               if (rtb != null)
               {
                   try
                   {
                       if (rtb.Handle.ToString() == handle.ToString())
                       {
                           //Debug.WriteLine("Handled    TRUEEE");
                           return handle.ToString();
                       }
                       //else
                       //Debug.WriteLine("Handled    FALSE" + rtb.Handle.ToString() + "     " + handle.ToString());
                   }
                   catch { }
               }
               return "";
           }


           public string GetObjectTypeAndText(int x)
           {
               if (rtb != null)
               {
                   try
                   {
                       //Debug.WriteLine("GetObjectTypeAndTextasdsadasdasdasdsa " + x + " -- " + rtb.Handle.ToString() + "     " + rtb.readObjectTypeAndText);
                       if (x.ToString() != rtb.Handle.ToString()) return "";
                       else if (rtb.readObjectTypeAndText == false) return "";
                       string result = "";
                       result += rtb.AccessibleName.Length > 0 ? rtb.AccessibleName : rtb.AccessibleDescription + " ";
                       //result += GetAll();
                       //Debug.WriteLine("GetObjectTypeAndText " + x + " -- " + rtb.Handle.ToString() + "    -------  " + "--- " + result + "  --  " + rtb.readObjectTypeAndText);
                       rtb.readObjectTypeAndText = false;
                       return result;
                   }
                   catch { }
               }
               return "";
           }

           public string GetAll()
           {
               try
               {
                   if (rtb != null)
                       return rtb.verbalizationComplete();
               }
               catch { }
               return null;
           }

           public string getErrorCode()
           {
               return error_code;
           }

           public int isBrailleLine()
           {
               if (rtb != null)
               {
                   try
                   {
                       //Debug.WriteLine("BRAILLLLLLLLLE: " + rtb.braille);
                       int braille = rtb.braille;
                       rtb.braille = 0;
                       return braille;
                   }
                   catch { }
               }
               return 0;
           }

           public string SayFromCursor()
           {
               if (rtb != null)
               {
                   try
                   {
                       return rtb.verbalizationLine(false);
                   }
                   catch { }
               }
               return "";
           }

           public string SayToCursor()
           {
               if (rtb != null)
               {
                   try
                   {
                       return rtb.verbalizationLine(true);
                   }
                   catch { }
               }
               return "";
           }

           public string SayWord()
           {
               if (rtb != null)
               {
                   try
                   {
                       return rtb.sayWord();
                   }
                   catch { }
               }
               return "";
           }

           public string SayCharacterPhonetic()
           {
               if (rtb != null)
               {
                   try
                   {
                       return rtb.verbalizationPhonetic();
                   }
                   catch { }
               }
               return "";
           }

           public string SpellWord()
           {
               if (rtb != null)
               {
                   try
                   {
                       return rtb.spellWord();
                   }
                   catch { }
               }
               return "";
           }

           public string GetStatusBar()
           {
               if (mainView != null)
               {
                   try
                   {
                       return mainView.getStatusBarText();
                   }
                   catch { }
               }
               return "";
           }
           public bool HasObject()
           {
               //Debug.WriteLine(rtb != null ? true : false); 
               return rtb != null ? true : false;
           }

           public string GetParagraph()
           {
               try
               {
                   if (rtb != null) return rtb.verbalizationParagraph();
               }
               catch { }
               return null;
           }

        */
    }
}

