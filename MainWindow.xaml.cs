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
                //cursorSound = new CursorPosSound(this);
                PlayCommand?.FireCanExecuteChanged();
                PauseCommand?.FireCanExecuteChanged();
                StopCommand?.FireCanExecuteChanged();
            }
        }
        public string SourceBraile
        {
            //TO DO
            get { return null; }

            set 
            {
                
            }
        }

        public static readonly DependencyProperty SourceXmlProperty = DependencyProperty.Register("SourceXml", typeof(string), typeof(Window));
        public static readonly DependencyProperty SourceXmlBrailleProperty = DependencyProperty.Register("SourceBraille", typeof(string), typeof(Window));

        public string Braille
        {
            get { return (string)GetValue(BrailleProperty); }
            set { SetValue(BrailleProperty, value); }
        }
        public static readonly DependencyProperty BrailleProperty = DependencyProperty.Register("Braille", typeof(string), typeof(Window));


        public string FileNameXml;
        public string FileNameBraille;
        public ScorePlayer player;
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

        private void text1_SelectionChanged(object sender, RoutedEventArgs e)
        {

            if (Viewer != null && Moved)
            {
                Moved = false;
                //Si va pa lante
                if (text1.CaretIndex - Viewer.GetCurrent() == 1)
                {
                    text1.CaretIndex += Viewer.GetCurrentForward() - 1;
                    Viewer.UpdateIndex(text1.CaretIndex);
                    var s = Regex.Match(Viewer.GetElement(), @"^([\w\-]+)");
                    bool nota;
                    if (s.Value != "Espacio" && s.Value != "Clave" && s.Value != "Armadura" && s.Value != "Compás" && s.Value != "Salto") nota = true;
                    else nota = false;
                    if (!Viewer.IsInMiddle() && nota) // si la celda en la que esta situada es una nota distinta, haz que suene
                    {
                        //cursorSound.setPlay(false);
                    }
            //        cursorSound.play(Pitch.G1, RhythmicDuration.Half); // placeholder, necesitamos alguna manera de acceder a la nota
                }

                //Si va pa tras
                else if (text1.CaretIndex - Viewer.GetCurrent() == -1)
                {
                    text1.CaretIndex = text1.CaretIndex - Viewer.GetCurrentBackward() + 1;
                    Viewer.UpdateIndex(text1.CaretIndex);
                }
                else
                {
                    Viewer.UpdateIndex(text1.CaretIndex);
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
    }

}
