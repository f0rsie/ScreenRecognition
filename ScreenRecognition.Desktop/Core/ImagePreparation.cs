using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ScreenRecognition.Desktop.Core
{
    public class ImagePreparation
    {
        public ImageSource ImageToImageSource(System.Drawing.Image image)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);

                stream.Seek(0, SeekOrigin.Begin);

                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }

        public System.Drawing.Image? Crop(System.Drawing.Image image, System.Drawing.Rectangle selection)
        {
            System.Drawing.Bitmap? bmp = image as System.Drawing.Bitmap;

            System.Drawing.Bitmap? cropBmp = bmp?.Clone(selection, bmp.PixelFormat);

            image.Dispose();

            return cropBmp;
        }
        public System.Drawing.Image ImageWpfToGDI(ImageSource image)
        {
            MemoryStream ms = new MemoryStream();
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
            encoder.Save(ms);
            ms.Flush();

            return System.Drawing.Image.FromStream(ms);
        }
    }
}
