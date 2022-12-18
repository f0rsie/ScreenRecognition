using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScreenRecognition.Desktop.Resources.Styles.MessageResult
{
    /// <summary>
    /// Логика взаимодействия для MessageResultWindow.xaml
    /// </summary>
    public partial class MessageResultWindow : Window
    {
        public MessageResultWindow()
        {
            InitializeComponent();
        }

        public MessageResultWindow(string? message, double width = 400, double height = 200) : this()
        {
            result.Text = message;

            resultWindow.Width = 800;
            resultWindow.Height = height;
            result.FontSize = 14;
        }

        private void resultWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
