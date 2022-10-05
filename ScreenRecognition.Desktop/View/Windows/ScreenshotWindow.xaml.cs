using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ScreenshotWindow.xaml
    /// </summary>
    public partial class ScreenshotWindow : Window
    {
        public ScreenshotWindow(System.Drawing.Image image) : this()
        {
            image1.Source = ImageToImageSource(image);
        }

        public ScreenshotWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(CloseWindow);
        }

        private void CloseWindow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private ImageSource ImageToImageSource(System.Drawing.Image image)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                // Save to the stream
                image.Save(stream, ImageFormat.Png);

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                // Tell the WPF BitmapImage to use this stream
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }
    }
}
