using GlobalHotKeys;
using ScreenRecognition.Desktop.Core;
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

            f.Content = new Pages.SettingsPage().Content;
        }
       
        private void Window_Closed(object sender, EventArgs e)
        {
            RegisterGlobalHotkey.s_Hotkey?.Dispose();
            RegisterGlobalHotkey.s_Subscription?.Dispose();
            RegisterGlobalHotkey.s_HotKeyManager?.Dispose();
        }
    }
}
