using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
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
        Timer _timer;

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

            _timer = new Timer(5000);

            _timer.Elapsed += OnStartTimer;
            _timer.Start();
        }

        private void resultWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void OnStartTimer(object? sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Close();
                _timer.Dispose();
            });
        }
    }
}
