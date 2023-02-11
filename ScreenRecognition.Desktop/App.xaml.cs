using HandyControl.Tools;
using ScreenRecognition.Desktop.Core;
using System.Windows;

namespace ScreenRecognition.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConfigHelper.Instance.SetLang("ru");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            RegisterGlobalHotkey.Dispose();
        }
    }
}
