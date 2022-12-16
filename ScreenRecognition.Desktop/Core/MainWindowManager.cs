using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenRecognition.Desktop.Core
{
    public static class MainWindowManager
    {
        public static void Set(Window window)
        {
            var app = App.Current.MainWindow;
            App.Current.MainWindow = window;

            app.Close();

            window.Show();
        }
    }
}
