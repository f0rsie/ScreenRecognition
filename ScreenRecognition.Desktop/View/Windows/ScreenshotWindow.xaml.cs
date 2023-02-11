using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ScreenshotWindow.xaml
    /// </summary>
    public partial class ScreenshotWindow : Window
    {
        Core.ImagePreparation _imagePreparation = new Core.ImagePreparation();

        private Point _startPoint;
        private Rectangle? _rect;
        private Rectangle? _lastRect;
        public double _startX = 0;
        public double _startY = 0;

        public ImageSource Image;

        public ScreenshotWindow(System.Drawing.Image image) : this()
        {
            backgroundImage.ImageSource = _imagePreparation.ImageToImageSource(image);
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

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Canvas.Children.Contains(_lastRect) == true)
            {
                Canvas.Children.Remove(_lastRect);
            }

            _startPoint = e.GetPosition(Canvas);

            _startX = _startPoint.X;
            _startY = _startPoint.Y;

            _rect = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2
            };
            Canvas.SetLeft(_rect, _startPoint.X);
            Canvas.SetTop(_rect, _startPoint.Y);
            Canvas.Children.Add(_rect);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || _rect == null)
                return;

            var pos = e.GetPosition(Canvas);

            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);

            var w = Math.Max(pos.X, _startPoint.X) - x;
            var h = Math.Max(pos.Y, _startPoint.Y) - y;

            _rect.Width = w;
            _rect.Height = h;

            Canvas.SetLeft(_rect, x);
            Canvas.SetTop(_rect, y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_rect != null)
            {
                _lastRect = _rect;
            }

            _rect = null;

            GetSelectedImage();
            this.Close();
        }

        private void GetSelectedImage()
        {
            try
            {
                var img = backgroundImage.ImageSource;

                var prImg = _imagePreparation.ImageWpfToGDI(img);

                var result = _imagePreparation.Crop(prImg, new System.Drawing.Rectangle(int.Parse(_startX.ToString()), int.Parse(_startY.ToString()), int.Parse(_lastRect.Width.ToString()), int.Parse(_lastRect.Height.ToString())));

                backgroundImage.ImageSource = _imagePreparation.ImageToImageSource(result);

                Image = backgroundImage.ImageSource;

            }
            catch { }
        }
    }
}
