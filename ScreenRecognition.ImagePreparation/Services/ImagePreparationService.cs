using System.Drawing;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        // 05.10.2022 16:30 - Эта функция переводит изображение в черно-белый формат, что по сути она и должна делать. Однако цвет текста она также инвертирует
        // Пояснение для меня же: если фон черный, а текст белый - то получится идеальное изображение для ОКРа, однако если цвет текста черный, а фон белый - то ОКР будет работать через жопу
        // Нужно в функции InvertImageColor как-то делать так, чтобы цвет фона всегда был белым, а цвет текста - черным. Тогда окр будет работать практчиески идеально

        private Bitmap FloodFillImage(Bitmap bmp, int p, Color textColor, Color backgroundColor)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);
            Color color = new Color();

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    color = bmp.GetPixel(i, j);
                    int K = (color.R + color.G + color.B) / 3;
                    result.SetPixel(i, j, K <= p ? textColor : backgroundColor);
                }
            }

            return result;
        }

        private Bitmap InvertImageColor(Bitmap bitmap, int p, Color textColor, Color backgroundColor)
        {
            int textPixels = 0;
            int backgroundPixels = 0;

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    var color = bitmap.GetPixel(i, j);

                    if (color.ToArgb() == textColor.ToArgb())
                    {
                        textPixels++;
                    }
                    else if (color.ToArgb() == backgroundColor.ToArgb())
                    {
                        backgroundPixels++;
                    }
                }
            }

            if (textPixels > backgroundPixels)
            {
                return FloodFillImage(bitmap, p, backgroundColor, textColor);
            }

            return bitmap;
        }

        public static byte[] BitmapToByte(Bitmap bitmap)
        {
            var sampleStream = new MemoryStream();
            bitmap.Save(sampleStream, System.Drawing.Imaging.ImageFormat.Bmp);

            return sampleStream.ToArray();
        }

        public static Bitmap ByteToBitmap(byte[] image)
        {
            var result = new Bitmap(new MemoryStream(image));

            return result;
        }

        public byte[] GetPreparedImage(Bitmap image, Color textColor, Color backgroundColor, int floodValue = 150)
        {
            var blackWhiteBitmap = FloodFillImage(image, floodValue, textColor, backgroundColor);
            var invertedBlackWhiteBitmap = InvertImageColor(blackWhiteBitmap, floodValue, textColor, backgroundColor);
            //blackWhiteBitmap.Save($"E:/Diplom/Results/img_test{DateTime.Now.Second}.png", System.Drawing.Imaging.ImageFormat.Png); // Это временная отладка
            //invertedBlackWhiteBitmap.Save($"E:/Diplom/Results/img_testInverted{DateTime.Now.Minute}_{DateTime.Now.Second}_{DateTime.Now.Millisecond}.png", System.Drawing.Imaging.ImageFormat.Png); // Это временная отладка
            var result = BitmapToByte(invertedBlackWhiteBitmap);

            return result;
        }
    }
}
