using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ScreenRecognition.Desktop.Resources.Styles.MessageResult
{
    /// <summary>
    /// Логика взаимодействия для MessageResultWindow.xaml
    /// </summary>
    public partial class MessageResultWindow : Window
    {
        public MessageResultWindow()
        {
            InitializeComponent();
        }

        public MessageResultWindow(string? message, string resultColor, double width = 400, double height = 200) : this()
        {
            var selectedColor = ((SolidBrush)typeof(System.Drawing.Brushes).GetProperties().FirstOrDefault(e => e.Name.ToLower() == resultColor.ToLower()).GetValue(null, null)).Color;

            result.Text = message;
            result.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));

            resultWindow.Width = width;
            //resultWindow.Height = height;
        }

        private void resultWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
