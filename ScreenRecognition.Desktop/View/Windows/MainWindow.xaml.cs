using ScreenRecognition.Desktop.ViewModel.WindowViewModels;
using System.Windows;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavigateButtons_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.NavigateToPage(sender);
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.SignWindow();
        }
    }
}