using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Linq;
using Manufaktura.Controls.Parser;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using Manufaktura.Music.Model; // temporal
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Linq;

namespace LiveDots
{
    public partial class MainWindow : Window
    {
        private void init()
        {
            noteViewer1.IsSelectable = true;
            noteViewer1.PreviewMouseLeftButtonUp += noteViewer1_PreviewMouseLeftButtonUp;
            //noteViewer1.QueryCursor += NoteViewer1_QueryCursor;


            text1.IsReadOnly = false;
            text1.SelectionChanged += text1_SelectionChanged;
            text1.TextChanged += text1_TextChanged;
        }

        /*
         * Here we have to update viewer, backward and forward with the new info
         */
        private void text1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(text1.Text);            
        }

        private void NoteViewer1_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            Console.WriteLine(e);
        }


        public string SourceXml
        {
            get { return (string)GetValue(SourceXmlProperty); }
            set
            {
                SetValue(SourceXmlProperty, value);
                var score = value.ToScore();
                if (player != null) ((IDisposable)player).Dispose();
                player = new MyMidiTaskScorePlayer(score);
                cursorSound = new CursorPosSound(this);
                PlayCommand?.FireCanExecuteChanged();
                PauseCommand?.FireCanExecuteChanged();
                StopCommand?.FireCanExecuteChanged();
            }
        }

        public static readonly DependencyProperty SourceXmlProperty = DependencyProperty.Register("SourceXml", typeof(string), typeof(Window));
 
        public string Braille
        {
            get { return (string)GetValue(BrailleProperty); }
            set { SetValue(BrailleProperty, value); }
        }
        public static readonly DependencyProperty BrailleProperty = DependencyProperty.Register("Braille", typeof(string), typeof(Window));


        public string FileNameXml;
        public string FileNameBraille;
        public MyMidiTaskScorePlayer player;
        public ScorePlayer cursor;
        public BrailleText BrailleText;
        public BrailleMusicViewer Viewer;
        public bool Moved;
        public CursorPosSound cursorSound;


        public new int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public new static DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(int), typeof(Window));

        public double ScoreZoomFactor
        {
            get { return (double)GetValue(ScoreZoomFactorProperty); }
            set { SetValue(ScoreZoomFactorProperty, value); }
        }
        public static DependencyProperty ScoreZoomFactorProperty = DependencyProperty.Register("ScoreZoomFactor", typeof(double), typeof(Window));

        public int BrailleSize
        {
            get { return (int)GetValue(BrailleSizeProperty); }
            set { SetValue(BrailleSizeProperty, value); }
        }
        public static DependencyProperty BrailleSizeProperty = DependencyProperty.Register("BrailleSize", typeof(int), typeof(Window));

        public void IncreaseFontSize(int n = 2)
        {
            FontSize += n;
        }
        public void DecreaseFontSize(int n = 2)
        {
            FontSize -= n;
        }
        public void IncreaseBrailleSize(int n = 2)
        {
            BrailleSize += n;
        }
        public void DecreaseBrailleSize(int n = 2)
        {
            BrailleSize -= n;
        }

        public void IncreaseScoreSize()
        {
            ScoreZoomFactor += 0.1;
        }

        public void DecreaseScoreSize()
        {
            ScoreZoomFactor -= 0.1;
        }

        public StopCommand StopCommand { get; }
        public PlayCommand PlayCommand { get; }
        public PauseCommand PauseCommand { get; }


        public MainWindow()
        {
            
            DicBraille d = new DicBraille(); 
            
            try
            {
                string fuentes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fuentes\\edico_es_br6.ttf");
                File.Copy(fuentes, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts", "edico_es_br6.ttf"), true);
                Microsoft.Win32.RegistryKey keyfont = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                keyfont.SetValue("Fuente braille", "edico_es_br6.ttf");
                keyfont.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());      
            }
            Thread.Sleep(2000);

            InitializeComponent();         
            this.Loaded += MainWindow_Loaded;
            this.DataContext = this;

            PlayCommand = new PlayCommand(this);
            PauseCommand = new PauseCommand(this);
            StopCommand = new StopCommand(this);
            PlayCommand?.FireCanExecuteChanged();
            PauseCommand?.FireCanExecuteChanged();
            StopCommand?.FireCanExecuteChanged();

            LiveDotsCommands.CommandBinding(this);


            if (!JawsSettings.CheckJawsInstalled())
            {
                MessageBox.Show("ERROR No se ha configurado bien el JAWS");
            }
            LiveDotsCOMObj.SetMainWindow(this);

            ScoreZoomFactor = 1.3;
            BrailleSize = 34;
            FontSize = 24;
        }

        /*
         * Executed when the window is completely ready for interaction
         */
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            player?.Stop();
            LiveDotsCommands.Open(this);
        }

        private void SaveBrailleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LiveDotsCommands.SaveBraille(this);
        }

        private void SaveXmlMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LiveDotsCommands.SaveXml(this);
        }


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LiveDotsCommands.Exit(this);
        }

        private void noteViewer1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MusicXmlParser parser = new MusicXmlParser();
            string nuevo = parser.ParseBack(noteViewer1.InnerScore).ToString();
            System.IO.File.WriteAllText("prueba.xml", nuevo);

            //======================================================
            //Alvar:temporal
            SourceXml = File.ReadAllText("prueba.xml");
            //======================================================

            BrailleScore bs = Converter.Xml2Braille("prueba.xml");
            BrailleText = new BrailleText(bs);
            Braille = BrailleText.GetBrailleString();
            Viewer = BrailleText.GetViewer();
            
            LiveDotsCOMObj.SetCurrent(BrailleText.GetViewer());
            //MessageBox.Show("Ha habido cambios");
        }

        /*
        private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LiveDotsCommands.Play(this);
        }

        private void StopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            player?.Stop();          
        }

        private void PauseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            player?.Pause();
        }
        */

        private void IncreaseScoreSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IncreaseScoreSize();
        }
        private void DecreaseScoreSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DecreaseScoreSize();
        }
        private void IncreaseFontSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IncreaseFontSize();
        }
        private void DecreaseFontSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DecreaseFontSize();
        }
        private void IncreaseBrailleSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IncreaseBrailleSize();
        }
        private void DecreaseBrailleSizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DecreaseBrailleSize();
        }

        private void setNote(string rKey, string octava)
        {
            Pitch pitch = null;
            RhythmicDuration rhythmicDuration = RhythmicDuration.Quarter;
            //todo en la misma tonalidad pero funciona
            switch (octava)
            {
                case "Quinta":
                    switch (rKey[0])
                    {
                        case 'A':
                            pitch = Pitch.A5;
                            break;
                        case 'B':
                            pitch = Pitch.B5;
                            break;
                        case 'C':
                            pitch = Pitch.C5;
                            break;
                        case 'D':
                            pitch = Pitch.D5;
                            break;
                        case 'E':
                            pitch = Pitch.E5;
                            break;
                        case 'F':
                            pitch = Pitch.F5;
                            break;
                        case 'G':
                            pitch = Pitch.G5;
                            break;
                    }
                    break;
                case "Cuarta":
                    switch (rKey[0])
                    {
                        case 'A':
                            pitch = Pitch.A4;
                            break;
                        case 'B':
                            pitch = Pitch.B4;
                            break;
                        case 'C':
                            pitch = Pitch.C4;
                            break;
                        case 'D':
                            pitch = Pitch.D4;
                            break;
                        case 'E':
                            pitch = Pitch.E4;
                            break;
                        case 'F':
                            pitch = Pitch.F4;
                            break;
                        case 'G':
                            pitch = Pitch.G4;
                            break;
                    }
                    break;
                default:
                    switch (rKey[0])
                    {
                        //cuarta octava
                        case 'A':
                            pitch = Pitch.A4;
                            break;
                        case 'B':
                            pitch = Pitch.B4;
                            break;
                        case 'C':
                            pitch = Pitch.C4;
                            break;
                        case 'D':
                            pitch = Pitch.D4;
                            break;
                        case 'E':
                            pitch = Pitch.E4;
                            break;
                        case 'F':
                            pitch = Pitch.F4;
                            break;
                        case 'G':
                            pitch = Pitch.G4;
                            break;
                    }
                    break;
            }
          
            string rDuration = rKey.Substring(1);
            switch(rDuration)
            {
                case "1/2":
                    rhythmicDuration = RhythmicDuration.Eighth;
                    break;
                case "1":
                    rhythmicDuration = RhythmicDuration.Quarter;
                    break;
                case "2":
                    rhythmicDuration = RhythmicDuration.Half;
                    break;
                case "4":
                    rhythmicDuration = RhythmicDuration.Whole;
                    break;
            }
            cursorSound.play(pitch, rhythmicDuration);
        }
        private void getNote(string ViewerValue, string octava)
        {
             ResourceManager MyResourceClass = new ResourceManager(typeof(ViewerRES));
            //Metodo 1, con un for, poco optimo
            /*
             ResourceSet resourceSet =
                 ViewerRES.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
             foreach (DictionaryEntry entry in resourceSet)
             {
                 string resourceKey = entry.Key.ToString();
                 string resourceValue = (string)entry.Value;
                 if (resourceValue == ViewerValue)
                 {
                     setNote(resourceKey);
                     //cursorSound.play(Pitch.A1, RhythmicDuration.Half);
                     break;
                 }
             }*/
            //Metodo 2, lo busca con el string directamente, pero he duplicado el resources por comodidad, se limpiara mas adelante
            string key = MyResourceClass.GetString(ViewerValue);
            string clave = Viewer.GetElement(5).Trim(); // quiza esto influya en los tonos
            if(key != null) setNote(key, octava);
        }
        private void text1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Viewer != null && Moved)
            {
                Moved = false;
                //Si va para delante
                if (text1.CaretIndex - Viewer.GetCurrent() == 1)
                {
                    text1.CaretIndex += Viewer.GetCurrentForward() - 1;
                    Viewer.UpdateIndex(text1.CaretIndex);
                }
                //Si va para atras
                else if (text1.CaretIndex - Viewer.GetCurrent() == -1)
                {
                    text1.CaretIndex = text1.CaretIndex - Viewer.GetCurrentBackward() + 1;
                    Viewer.UpdateIndex(text1.CaretIndex);
                }
                else
                {
                    Viewer.UpdateIndex(text1.CaretIndex);
                }

                //Quiza se pueda dar uso a la armadura para averiguar que tonalidad usar, pero implicaria usar un switch?, de momento usa el tono 4 (algo que no comprendo de musica)
                var s = Regex.Match(Viewer.GetElement(), @"^([\w\-]+)");
                if (!Viewer.IsInMiddle() &&
                    s.Value != "Espacio" && s.Value != "Clave" && s.Value != "Armadura" && s.Value != "Compás" && s.Value != "Salto") // si la celda en la que esta situada es una nota distinta, haz que suene
                {
                    cursorSound.setPlay(false);
                }
                else if (Viewer.GetCurrentBackward() == 1 && Viewer.GetCurrentForward() == 1)
                {
                    cursorSound.setPlay(false);
                }
                if (!cursorSound.getPlay())
                {
                    //Comprobar con todo el resources el valor actual del viewer y crear un pitch y duration deseados, hacer esto en el resources? Duda pendiente
                    //funciona asi como esta, no es lo mas optimo
                    string nota = Viewer.GetElement(Viewer.GetCurrent()).Trim();

                    string aux_nota = nota.Split(' ').Skip(1).FirstOrDefault();
                    string num_octava = null;
                    if (aux_nota == "octava")
                    {
                        num_octava = aux_nota.Split(' ')[0];
                        var WordsArray = nota.Split();
                        //coge los ultimos dos, que contienen las notas
                        nota= WordsArray[WordsArray.Length -2] + ' ' + WordsArray[WordsArray.Length - 1];
                    }
                    getNote(nota, num_octava);
                }

                Moved = true;
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText("Documentacion/AcercaDe.txt"), "Acerca de LiveDots");
        }

        private void GuideMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Guide().Show();
        }

        public void changeXML(string xml)
        {
            //noteViewer1.XmlSource = xml;
            this.SourceXml = xml;
        }
    }

}
