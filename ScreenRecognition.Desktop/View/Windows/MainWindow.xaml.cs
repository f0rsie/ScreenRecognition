using GlobalHotKeys;
using ScreenRecognition.Desktop.Core;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void Window_Closed(object sender, EventArgs e)
        {
            RegisterGlobalHotkey.Dispose();
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