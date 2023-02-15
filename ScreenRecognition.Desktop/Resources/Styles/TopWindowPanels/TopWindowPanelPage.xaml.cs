using ScreenRecognition.Desktop.View.Windows;
using ScreenRecognition.Desktop.ViewModel.WindowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenRecognition.Desktop.Resources.Styles.TopWindowPanels
{
    /// <summary>
    /// Логика взаимодействия для TopExitPanelPage.xaml
    /// </summary>
    public partial class TopWindowPanelPage : Page
    {
        private Window? _currentWindow = App.Current.MainWindow;

        public TopWindowPanelPage()
        {
            InitializeComponent();

            headerTextBlock.Text = _currentWindow.Title;
        }

        private void CheckCurrentWindow()
        {
            _currentWindow = App.Current.Windows.Cast<Window>().FirstOrDefault(e => e.IsActive);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            CheckCurrentWindow();

            MinimizeToTray();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            CheckCurrentWindow();

            _currentWindow.WindowState = WindowState.Minimized;
        }

        private void MinimizeToTray()
        {
            bool minimizeToTray = Properties.ProgramSettings.Default.MinimizeToTray;

            if (minimizeToTrayTaskbarIcon.Visibility == Visibility.Visible)
            {
                App.Current.MainWindow.Close();
            }
            else if (minimizeToTray)
            {
                _currentWindow?.Hide();

                minimizeToTrayTaskbarIcon.Visibility = Visibility.Visible;
            }
        }

        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            _currentWindow?.Show();
            _currentWindow?.Activate();

            _currentWindow.WindowState = WindowState.Normal;
            minimizeToTrayTaskbarIcon.Visibility = Visibility.Collapsed;
        }

        private void TopWindowPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckCurrentWindow();

            _currentWindow?.DragMove();
        }

        private void navigateButton_Click(object sender, RoutedEventArgs e)
        {
            TaskbarIcon_TrayLeftMouseDown(sender, e);
            CheckCurrentWindow();

            if (_currentWindow == App.Current.MainWindow)
            {
                ((_currentWindow as MainWindow)?.DataContext as MainWindowViewModel)?.NavigateToPage((sender as MenuItem).Name);
            }
        }
    }
}
