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
