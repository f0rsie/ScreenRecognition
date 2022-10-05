using System.Drawing;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        // 05.10.2022 16:30 - Эта функция переводит изображение в черно-белый формат, что по сути она и должна делать. Однако цвет текста она также инвертирует
        // Пояснение для меня же: если фон черный, а текст белый - то получится идеальное изображение для ОКРа, однако если цвет текста черный, а фон белый - то ОКР будет работать через жопу
        // Нужно в функции InvertImageColor как-то делать так, чтобы цвет фона всегда был белым, а цвет текста - черным. Тогда окр будет работать практчиески идеально

        private Bitmap FloodFillImage(Bitmap bmp, int p)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);
            Color color = new Color();

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    color = bmp.GetPixel(i, j);
                    int K = (color.R + color.G + color.B) / 3;
                    result.SetPixel(i, j, K <= p ? Color.Black : Color.White);
                }
            }

            return result;
        }

        // Эта функция не хочет инвертировать цвета, она ничего не меняет (в лучшую сторону по крайней мере). Исправлю позже, когда добавлю переводчик и т.д
        // 05.10.2022 16:30 - а эта шляпа не работает на данный момент. Планирую в ближайшем будущем переделать её так, чтобы текст на фото всегда был черный, а фон белым.
        private Bitmap InvertImageColor(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color oldColor = bitmap.GetPixel(x, y);
                    Color newColor;
                    newColor = Color.FromArgb(oldColor.A, 255 - oldColor.R, 255 - oldColor.G, 255 - oldColor.B);
                    bitmap.SetPixel(x, y, newColor);
                }
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

        public byte[] GetPreparedImage(Bitmap image, int floodValue = 150)
        {
            var blackWhiteBitmap = FloodFillImage(image, floodValue);
            var invertedBlackWhiteBitmap = InvertImageColor(blackWhiteBitmap);
            //blackWhiteBitmap.Save($"E:/Diplom/img_test{DateTime.Now.Second}.png", System.Drawing.Imaging.ImageFormat.Png); // Это временная отладка
            invertedBlackWhiteBitmap.Save($"E:/Diplom/img_testInverted{DateTime.Now.Minute}_{DateTime.Now.Second}_{DateTime.Now.Millisecond}.png", System.Drawing.Imaging.ImageFormat.Png); // Это временная отладка
            var result = BitmapToByte(invertedBlackWhiteBitmap);

            return result;
        }
    }
}
