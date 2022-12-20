using ScreenRecognition.Desktop.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            RegisterGlobalHotkey.Dispose();
        }
    }
}
