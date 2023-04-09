using ScreenRecognition.Desktop.Models.DbModels;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        private string? _translatedText;
        private string? _originalText;

        private string? _originalLanguage;
        private string? _translatedLanguage;

        public MessageResultWindow()
        {
            InitializeComponent();
        }

        public MessageResultWindow(string? originalText, string translatedText, string? originalLanguage, string? translatedLanguage, string resultColor, double width = 400, double height = 200) : this()
        {
            var selectedColor = ((SolidBrush)typeof(System.Drawing.Brushes).GetProperties().FirstOrDefault(e => e.Name.ToLower() == resultColor.ToLower()).GetValue(null, null)).Color;

            _originalText = originalText;
            _translatedText = translatedText;

            _originalLanguage = originalLanguage;
            _translatedLanguage = translatedLanguage;

            originalLanguageTextBlock.Text = _originalLanguage;
            translatedLanguageTextBlock.Text = _translatedLanguage;

            result.Text = _translatedText;
            result.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B));

            result.Width = width;
            //resultWindow.Height = height;

            RestartTimeLeft();
        }

        private void StopTimer()
        {
            _timer?.Dispose();
        }

        private void RestartTimeLeft(int time = 5000)
        {
            _timer?.Dispose();

            _timer = new Timer(time);

            _timer.Elapsed += OnStartTimer;
            _timer.Start();
        }

        private void OnStartTimer(object? sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Close();
                _timer.Dispose();
            });
        }

        private void resultWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StopTimer();
            RestartTimeLeft();

            result.IsEnabled = !result.IsEnabled;
            topPanel.Visibility = topPanel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void resultWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopTimer();

            DragMove();
        }

        private void resultWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RestartTimeLeft();
        }

        private void resultWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void result_SelectionChanged(object sender, RoutedEventArgs e)
        {
            StopTimer();
            RestartTimeLeft();
        }

        private void switchTextButton_Click(object sender, RoutedEventArgs e)
        {
            result.Text = switchTextButton.IsChecked == true ? _translatedText : _originalText;
        }
    }
}
