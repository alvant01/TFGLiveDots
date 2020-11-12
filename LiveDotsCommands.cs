using Manufaktura.Controls.Parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;

namespace LiveDots
{
    public static class LiveDotsCommands
    {
        public static RoutedCommand LiveDotsOpen = new RoutedCommand();
        public static RoutedCommand LiveDotsSaveBraille = new RoutedCommand();
        public static RoutedCommand LiveDotsSaveXml = new RoutedCommand();
        //public static RoutedCommand LiveDotsPlay = new RoutedCommand();
        public static RoutedCommand LiveDotsExit = new RoutedCommand();
        public static RoutedCommand LiveDotsIncrease = new RoutedCommand();
        public static RoutedCommand LiveDotsDecrease = new RoutedCommand();


        //Modificado
        public static void Open(object sender)
        {
            MainWindow window = sender as MainWindow;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Partituras");
            openFileDialog.Filter = "MusicXml files (*.musicxml)|*.musicxml|Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName.ToLower().Contains(".musicxml_braille"))
                {

                    //Idea

                    //Sacar de archivo
                    List<char> content = new List<char>();
                    window.FileNameXml = openFileDialog.FileName;
                    string aux = File.ReadAllText(window.FileNameXml);
                    content.AddRange(aux);
                    //Convertir en arbol braile
                    //Parsear Atributos
                    BrailleText bt = new BrailleText();
                    BrailleScore bs = new BrailleScore();

                    bs.ParseBraille(content, bt); 

                    //PArsear Notas

                }
                else
                {
                    window.FileNameXml = openFileDialog.FileName;
                    window.SourceXml = File.ReadAllText(window.FileNameXml);

                    BrailleScore bs = Converter.Xml2Braille(window.FileNameXml);
                    window.BrailleText = new BrailleText(bs);
                    window.Viewer = window.BrailleText.GetViewer();
                    window.Braille = window.BrailleText.GetBrailleString();

                    window.Moved = true;
                    LiveDotsCOMObj.SetCurrent(window.BrailleText.GetViewer());
                }
            }
        }

        private static void CommandOpen(object sender, ExecutedRoutedEventArgs e)
        {
            Open(sender);
        }
        private static void CommandSaveBraille(object sender, ExecutedRoutedEventArgs e)
        {
            SaveBraille(sender);
        }

        public static void SaveBraille(object sender)
        {
            MainWindow window = sender as MainWindow;
            window.FileNameBraille = window.FileNameXml + "_braille";
            System.IO.File.WriteAllText(window.FileNameBraille, window.Braille);
            //Decir que se ha guardado
        }

        private static void CommandSaveXml(object sender, ExecutedRoutedEventArgs e)
        {
            SaveXml(sender);
        }

        public static void SaveXml(object sender)
        {
            MainWindow window = sender as MainWindow;
            MusicXmlParser parser = new MusicXmlParser();
            string NewXml = parser.ParseBack(window.noteViewer1.InnerScore).ToString();
            System.IO.File.WriteAllText(window.FileNameXml, NewXml);
            // Decir que se ha guardado
        }

        private static void CommandExit(object sender, ExecutedRoutedEventArgs e)
        {
            Exit(sender);
        }

        public static void Exit(object sender)
        {
            MainWindow window = sender as MainWindow;
            window.Close();
        }

        private static void CommandIncrease(object sender, ExecutedRoutedEventArgs e)
        {
            Increase(sender);
        }

        public static void Increase(object sender)
        {
            if (sender.GetType() == typeof(MainWindow))
            {
                MainWindow window = sender as MainWindow;
                window.IncreaseFontSize();
                window.IncreaseBrailleSize();
                window.IncreaseScoreSize();
            }
            else if (sender.GetType() == typeof(Guide))
            {
                Guide window = sender as Guide;
                window.IncreaseFontSize();
            }
        }

        private static void CommandDecrease(object sender, ExecutedRoutedEventArgs e)
        {
            Decrease(sender);
        }

        public static void Decrease(object sender)
        {
            if (sender.GetType() == typeof(MainWindow))
            {
                MainWindow window = sender as MainWindow;
                window.DecreaseFontSize();
                window.DecreaseBrailleSize();
                window.DecreaseScoreSize();
            }
            else if (sender.GetType() == typeof(Guide))
            {
                Guide window = sender as Guide;
                window.DecreaseFontSize();
            }
        }

        public static void CommandBinding(Object sender)
        {
            if (sender.GetType() == typeof(MainWindow))
            {
                MainWindow window = sender as MainWindow;

                LiveDotsOpen.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
                CommandBinding OpenCommandBinding = new CommandBinding(LiveDotsOpen, CommandOpen);
                window.CommandBindings.Add(OpenCommandBinding);

                LiveDotsSaveBraille.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Control));
                CommandBinding SaveBrailleCommandBinding = new CommandBinding(LiveDotsSaveBraille, CommandSaveBraille);
                window.CommandBindings.Add(SaveBrailleCommandBinding);

                LiveDotsSaveXml.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                CommandBinding SaveXmlCommandBinding = new CommandBinding(LiveDotsSaveXml, CommandSaveXml);
                window.CommandBindings.Add(SaveXmlCommandBinding);

                LiveDotsExit.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
                CommandBinding ExitCommandBinding = new CommandBinding(LiveDotsExit, CommandExit);
                window.CommandBindings.Add(ExitCommandBinding);

                LiveDotsIncrease.InputGestures.Add(new KeyGesture(Key.OemPlus, ModifierKeys.Control));
                CommandBinding IncreaseCommandBinding = new CommandBinding(LiveDotsIncrease, CommandIncrease);
                window.CommandBindings.Add(IncreaseCommandBinding);

                LiveDotsDecrease.InputGestures.Add(new KeyGesture(Key.OemMinus, ModifierKeys.Control));
                CommandBinding DecreaseCommandBinding = new CommandBinding(LiveDotsDecrease, CommandDecrease);
                window.CommandBindings.Add(DecreaseCommandBinding);

                KeyBinding PlayKeyBinding = new KeyBinding(window.PlayCommand, new KeyGesture(Key.R, ModifierKeys.Control));
                window.InputBindings.Add(PlayKeyBinding);
                KeyBinding PauseKeyBinding = new KeyBinding(window.PauseCommand, new KeyGesture(Key.T, ModifierKeys.Control));
                window.InputBindings.Add(PauseKeyBinding);
                KeyBinding StopKeyBinding = new KeyBinding(window.StopCommand, new KeyGesture(Key.Y, ModifierKeys.Control));
                window.InputBindings.Add(StopKeyBinding);
            }
            else if (sender.GetType() == typeof(Guide))
            {
                Guide window = sender as Guide;
                LiveDotsIncrease.InputGestures.Add(new KeyGesture(Key.OemPlus, ModifierKeys.Control));
                CommandBinding IncreaseCommandBinding = new CommandBinding(LiveDotsIncrease, CommandIncrease);
                window.CommandBindings.Add(IncreaseCommandBinding);

                LiveDotsDecrease.InputGestures.Add(new KeyGesture(Key.OemMinus, ModifierKeys.Control));
                CommandBinding DecreaseCommandBinding = new CommandBinding(LiveDotsDecrease, CommandDecrease);
                window.CommandBindings.Add(DecreaseCommandBinding);

            }
        }
    }

    public class PlayCommand : PlayerCommand
    {
        public PlayCommand(MainWindow viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return mainWindow.player != null && mainWindow.player.State != Manufaktura.Controls.Audio.ScorePlayer.PlaybackState.Playing;

        }

        public override void Execute(object parameter)
        {

            mainWindow.player?.Play();

        }
    }

    public class StopCommand : PlayerCommand
    {
        public StopCommand(MainWindow viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return mainWindow.player != null;
        }

        public override void Execute(object parameter)
        {
            mainWindow.player?.Stop();
        }
    }

    public class PauseCommand : PlayerCommand
    {
        public PauseCommand(MainWindow viewModel) : base(viewModel)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return mainWindow.player != null;
        }

        public override void Execute(object parameter)
        {
            mainWindow.player?.Pause();
        }
    }

    public abstract class PlayerCommand : ICommand
    {
        protected MainWindow mainWindow;

        protected PlayerCommand(MainWindow viewModel)
        {
            this.mainWindow = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public void FireCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
