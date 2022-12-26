using System.Drawing;
using System.Drawing.Imaging;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        // Конвертер из цветного в чёрно-белое
        private Bitmap FloodFillImage(Bitmap bmp, Color firstColor, Color secondColor)
        {
            var result = ImageColorSetter(bmp, firstColor, secondColor);

            return result;
        }

        private Bitmap ImageColorSetter(Bitmap bmp, Color firstPixelColor, Color secondPixelColor)
        {
            int firstPixelType = 0;
            int secondPixelType = 0;

            (int, int) startPixel = (new Random().Next(0, bmp.Width), new Random().Next(0, bmp.Height));

            var result = (Bitmap)bmp.Clone();

            using (var wr0 = new ImageWrapper(bmp, true))
            using (var wr1 = new ImageWrapper(result, true))
            {
                foreach (var p in wr0)
                {
                    if (IsColorsSimilar(wr1[startPixel.Item1, startPixel.Item2], wr1[p]))
                    {
                        wr1[p] = firstPixelColor;

                        firstPixelType++;
                    }
                    else
                    {
                        wr1[p] = secondPixelColor;

                        secondPixelType++;
                    }
                }
            }

            return result;
        }

        // Проверка двух пикселей на похожесть
        // Переделать: необходимо менять colorDifference в зависимости от размера изображения; чем меньше изображение - тем меньше должно быть это числл, а чем оно больше - тем больше это число
        private bool IsColorsSimilar(Color firstColor, Color secondColor)
        {
            int colorDifference = (int)(255 / 2);
            bool result = Math.Abs(firstColor.R - secondColor.R) < colorDifference && Math.Abs(firstColor.G - secondColor.G) < colorDifference && Math.Abs(firstColor.B - secondColor.B) < colorDifference;

            return result;
        }

        // Какая-то работа с изображением (пусть на всякий останется)
        private Bitmap Convolution(Bitmap img, double[,] matrix)
        {
            var w = matrix.GetLength(0);
            var h = matrix.GetLength(1);

            using (var wr = new ImageWrapper(img) { DefaultColor = Color.Silver })
                foreach (var p in wr)
                {
                    double r = 0d, g = 0d, b = 0d;

                    for (int i = 0; i < w; i++)
                        for (int j = 0; j < h; j++)
                        {
                            var pixel = wr[p.X + i - 1, p.Y + j - 1];
                            r += matrix[j, i] * pixel.R;
                            g += matrix[j, i] * pixel.G;
                            b += matrix[j, i] * pixel.B;
                        }
                    wr.SetPixel(p, r, g, b);
                }

            return img;
        }

        // Конвертер Bitmap в Byte array
        public static byte[] BitmapToByte(Bitmap bitmap)
        {
            var sampleStream = new MemoryStream();
            bitmap.Save(sampleStream, ImageFormat.Bmp);
            
            return sampleStream.ToArray();
        }

        // Конвертер Byte array в Bitmap
        public static Bitmap ByteToBitmap(byte[] image)
        {
            var result = new Bitmap(new MemoryStream(image));

            return result;
        }

        // Вызывает всё, что нужно
        public byte[] GetPreparedImage(Bitmap image, Color firstColor, Color secondColor)
        {
            var blackWhiteBitmap = FloodFillImage(image, firstColor, secondColor);
            var result = BitmapToByte(blackWhiteBitmap);

            return result;
        }
    }
}
