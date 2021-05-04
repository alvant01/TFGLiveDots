using LiveDots.Factories;
using LiveDots.MusicXmlBraile;
using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Linq;
using Manufaktura.Controls.Parser;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Controls;

using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveDots
{
    public partial class MainWindow : Window
    {
        private void Init()
        {
            playingGroupOfNotes = false;
            noteViewer1.IsSelectable = true;
            noteViewer1.PreviewMouseLeftButtonUp += NoteViewer1_PreviewMouseLeftButtonUp;
            //noteViewer1.QueryCursor += NoteViewer1_QueryCursor;

            text1.IsReadOnly = false;
            text1.SelectionChanged += Text1_SelectionChanged;
            text1.TextChanged += Text1_TextChanged;
            text1.PreviewKeyDown += Text1_Keydown;
            //----------------------
            //if not found create xml
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load("Notas.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " -XML not found");
                CreateXML.createDocument();
            }     
        }


        bool editing = false;
        /*
         * Here we have to update viewer, backward and forward with the new info
         */
        private void Text1_Keydown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Up && e.Key != Key.Down)
            {
                editing = true;
                Console.WriteLine("Editando " + editing);
            }
            if(e.Key == Key.Enter)
            {
                // try
                // {
                List<char> con = new List<char>();
                con.AddRange(text1.Text);

                BrailleText bt = new BrailleText();

                BXBrailleScore bs = FactoryLoad.GetInstance().GetBXBrailleScore();

                bs.Parse(con, bt);

                BrailleText = bt;
                Viewer = BrailleText.GetViewer();
                Braille = BrailleText.GetBrailleString();

                MusicXmlBraileParser mp = new MusicXmlBraileParser();
                string xml = mp.createXML(bs);

                changeXML(xml);

                Moved = true;
                LiveDotsCOMObj.SetCurrent(BrailleText.GetViewer());

                editing = false;
                // }
                /*catch (Exception x)
                {
                    Console.WriteLine("Error de edicion");
                }*/
            }
        }
        
        private void Text1_TextChanged(object sender, TextChangedEventArgs e)
        {           
                Console.WriteLine(text1.Text);
                char compas = text1.Text[text1.Text.IndexOf('#') + 1];
                char[] div = { ' ', '\n' };
                string[] text = text1.Text.Split(div, StringSplitOptions.RemoveEmptyEntries);

                //MessageBox.Show("Tus cambios no son validos");

            Console.WriteLine(Viewer.GetText());

            string g = "";
            foreach (string s in BrailleText.getText())
            {
                g += s;
            }

            //Console.WriteLine(g);

            //player?.Stop();
            //LiveDotsCommands.Open(this);
        }

        private void text1_selectedtext(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(text1.SelectedText);
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
        private bool playingGroupOfNotes;

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
            Init();
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

        private void NoteViewer1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public CancellationToken cancellationToken;
        private void setCancelInSelectionPlaying()
        {
            if (Viewer != null && Moved && playingGroupOfNotes) {
                // se ha movido asi que se cancela el play anterior
                cancellationTokenSource.Cancel();
            }
        }

        private async void PlayGroupOfNotes()
        {
            Console.WriteLine(text1.SelectedText);
            List<string> ListNotas = StringToNote.BrailleToStringNote(Viewer.GetCurrent(), Viewer.GetCurrent() + text1.SelectedText.Length, Viewer);
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            playingGroupOfNotes = true;
            //tocar todas las notas de la lista, espera el tiempo adecuado dependiendo de la BPM de la partitura y toca la siguiente nota tras acabar la espera
            foreach (string it in ListNotas)
            {
                string temporal = it;
                StringToNote.SetNoteForPlay(ref temporal, out string num_octava);
                cursorSound.TransformStringToNoteAndPlay(temporal, num_octava);

                int awaiting_time = StringToNote.BPMToMs(player.Tempo.BeatsPerMinute, cursorSound.GetRhythmicDuration(temporal));
                //si se ha movido el cursor, se deselecciona y se lanza un cancel request, que es controlado con esto y dejará de reproducir las notas restantes
                if (!cancellationToken.IsCancellationRequested)
                    await Task.Delay(awaiting_time);
                else break;
            }
            playingGroupOfNotes = false;
        }
        private void ProcessSelectedNotesAndPlay()
        {
            if (Viewer != null && Moved && !editing)
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

                Console.WriteLine(text1.SelectedText.Length);
 
                bool bigText = false;
                //mas de una palabra seleccionada
                if (text1.SelectedText.Length > 1) bigText = true;
                else Console.WriteLine("No hat selección");


                //Quiza se pueda dar uso a la armadura para averiguar que tonalidad usar, pero implicaria usar un switch?, de momento usa el tono 4 (algo que no comprendo de musica)
                var s = Regex.Match(Viewer.GetElement(), @"^([\w\-]+)");
                /*if (!Viewer.IsInMiddle() &&
                    s.Value != "Espacio" && s.Value != "Clave" && s.Value != "Armadura" && s.Value != "Compás" && s.Value != "Salto") // si la celda en la que esta situada es una nota distinta, haz que suene
                {
                    cursorSound.SetPlay(false);
                }
                else if (Viewer.GetCurrentBackward() == 1 && Viewer.GetCurrentForward() == 1)
                {
                    cursorSound.SetPlay(false);
                }
               */
                // si no hay texto seleccionado 
                if (!bigText)
                {
                    //Comprobar con todo el resources el valor actual del viewer y crear un pitch y duration deseados, hacer esto en el resources? Duda pendiente
                    //funciona asi como esta, no es lo mas optimo
                    string nota = Viewer.GetElement(Viewer.GetCurrent()).Trim();
                    StringToNote.SetNoteForPlay(ref nota, out string num_octava);

                    //tocaria solo si hay una nota     
                    cursorSound.TransformStringToNoteAndPlay(nota, num_octava);
                }
                else if (bigText) PlayGroupOfNotes();
                Moved = true;
            }
        }
        private  void Text1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            setCancelInSelectionPlaying();
            ProcessSelectedNotesAndPlay();
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
