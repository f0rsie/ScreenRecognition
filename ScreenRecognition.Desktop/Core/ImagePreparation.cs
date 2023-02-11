using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}
