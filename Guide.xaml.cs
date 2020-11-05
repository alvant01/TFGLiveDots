using System.IO;
using System.Windows;

namespace LiveDots
{
    /// <summary>
    /// Lógica de interacción para Guide.xaml
    /// </summary>
    public partial class Guide : Window
    {

        public int GuideFontSize
        {
            get { return (int)GetValue(GuideFontSizeProperty); }
            set { SetValue(GuideFontSizeProperty, value); }
        }
        public static DependencyProperty GuideFontSizeProperty = DependencyProperty.Register("GuideFontSize", typeof(int), typeof(Window));


        public string GuideText
        {
            get { return (string)GetValue(GuideTextProperty); }
            set { SetValue(GuideTextProperty, value); }
        }
        public static readonly DependencyProperty GuideTextProperty = DependencyProperty.Register("GuideText", typeof(string), typeof(Window));

        public Guide()
        {
            InitializeComponent();
            this.DataContext = this;
            LiveDotsCommands.CommandBinding(this);

            GuideText = File.ReadAllText("Documentacion/Guia de uso.txt");
            GuideFontSize = 24;
        }

        internal void IncreaseFontSize()
        {
            GuideFontSize += 2;
        }

        internal void DecreaseFontSize()
        {
            GuideFontSize -= 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
