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
        public ImageSource Image;

        public ScreenshotWindow(System.Drawing.Image image) : this()
        {
            image1.ImageSource = ImageToImageSource(image);
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

        private Point startPoint;
        private Rectangle rect;
        private Rectangle lastRect;
        private double _startX = 0;
        private double _startY = 0;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Canvas.Children.Contains(lastRect) == true)
            {
                Canvas.Children.Remove(lastRect);
            }

            startPoint = e.GetPosition(Canvas);

            _startX = startPoint.X;
            _startY = startPoint.Y;

            rect = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2
            };
            Canvas.SetLeft(rect, startPoint.X);
            Canvas.SetTop(rect, startPoint.Y);
            Canvas.Children.Add(rect);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rect == null)
                return;

            var pos = e.GetPosition(Canvas);

            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);

            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(rect != null)
            {
                lastRect = rect;
            }

            rect = null;

            GetSelectedImage();
            this.Close();
        }

        private void GetSelectedImage()
        {
            var img = image1.ImageSource;

            var prImg = ImageWpfToGDI(img);

            var result = Crop(prImg, new System.Drawing.Rectangle(int.Parse(_startX.ToString()), int.Parse(_startY.ToString()), int.Parse(lastRect.Width.ToString()), int.Parse(lastRect.Height.ToString())));

            image1.ImageSource = ImageToImageSource(result);

            Image = image1.ImageSource;

        }
        private System.Drawing.Image Crop(System.Drawing.Image image, System.Drawing.Rectangle selection)
        {
            System.Drawing.Bitmap bmp = image as System.Drawing.Bitmap;

            // Crop the image:
            System.Drawing.Bitmap cropBmp = bmp.Clone(selection, bmp.PixelFormat);

            // Release the resources:
            image.Dispose();

            return cropBmp;
        }
        private System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource image)
        {
            MemoryStream ms = new MemoryStream();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image as System.Windows.Media.Imaging.BitmapSource));
            encoder.Save(ms);
            ms.Flush();
            return System.Drawing.Image.FromStream(ms);
        }
    }
}
