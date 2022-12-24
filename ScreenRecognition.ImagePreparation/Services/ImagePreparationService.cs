using System.Drawing;
using System.Drawing.Imaging;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        // 05.10.2022 16:30 - Эта функция переводит изображение в черно-белый формат, что по сути она и должна делать. Однако цвет текста она также инвертирует
        // Пояснение для меня же: если фон черный, а текст белый - то получится идеальное изображение для ОКРа, однако если цвет текста черный, а фон белый - то ОКР будет работать через жопу
        // Нужно в функции InvertImageColor как-то делать так, чтобы цвет фона всегда был белым, а цвет текста - черным. Тогда окр будет работать практчиески идеально
        // ***
        // Конвертер изображение из цветного в чёрно-белое
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

        // Новый тестовый конвертер из цветного в чёрно-белое
        private Bitmap TestFloodFillImage(Bitmap bmp, Color textColor, Color backgroundColor, int floodValue, bool backgroundMoreThanText)
        {
            var random = new Random().Next();

            //bmp.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/origImage{floodValue}_{random}.png", ImageFormat.Png);

            Color color = new Color();

            var kernel = new double[,]
                 {{0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1}};

            var result = TestFloodFillImageInvertColorV2(bmp, textColor, backgroundColor, kernel, floodValue, backgroundMoreThanText);

            //result.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedorigImage{floodValue}_{random}.png", ImageFormat.Png);
            return result;
        }

        public Color GetBackgroundColor(Bitmap bmp, int floodValue)
        {
            var kernel = new double[,]
                 {{0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1}};

            int textPixels = 0;
            int backgroundPixels = 0;

            var dilated = (Bitmap)bmp.Clone();

            using (var wr0 = new ImageWrapper(bmp, true))
            using (var wr1 = new ImageWrapper(dilated, true))
                foreach (var p in wr0)
                    if (wr1[p].G > 0)
                        if ((wr1[p] = wr0[p]).G > floodValue)
                        {
                            textPixels++;
                        }
                        else
                        {
                            backgroundPixels++;
                        }

            Color result = new Color();

            if (backgroundPixels > textPixels)
                result = Color.Black;
            else
                result = Color.White;

            return result;
        }

        // Тест 1
        private Bitmap TestFloodFillImageInvertColorV2(Bitmap bmp, Color textColor, Color backgroundColor, double[,] kernel, int floodValue, bool backgroundMoreThanText)
        {
            int textPixels = 0;
            int backgroundPixels = 0;

            var dilated = (Bitmap)bmp.Clone();
            var result = Convolution(dilated, kernel);

            using (var wr0 = new ImageWrapper(bmp, true))
            using (var wr1 = new ImageWrapper(dilated, true))
                foreach (var p in wr0)
                    if (wr1[p].G > 0)
                        if ((wr1[p] = wr0[p]).G > floodValue)
                        {
                            textPixels++;

                            wr1[p] = textColor;
                            wr0[p] = textColor;
                        }
                        else
                        {
                            backgroundPixels++;

                            wr1[p] = backgroundColor;
                            wr0[p] = backgroundColor;
                        }

            if (backgroundMoreThanText && textPixels < backgroundPixels)
            {
                return TestFloodFillImageInvertColor(bmp, backgroundColor, textColor, kernel);
            }

            return result;
        }

        // Тест 2
        //private Bitmap TestFloodFillImageInvertColorV3FromByteArray(byte[] bmp, Color textColor, Color backgroundColor, double[,] kernel, int floodValue, bool backgroundMoreThanText)
        //{
        //    int textPixels = 0;
        //    int backgroundPixels = 0;

        //    var dilated = (Bitmap)bmp.Clone();
        //    var result = Convolution(dilated, kernel);

        //    using (var wr0 = new ImageWrapper(bmp, true))
        //    using (var wr1 = new ImageWrapper(dilated, true))
        //        foreach (var p in wr0)
        //            if (wr1[p].G > 0)
        //                if ((wr1[p] = wr0[p]).G > floodValue)
        //                {
        //                    textPixels++;

        //                    wr1[p] = textColor;
        //                    wr0[p] = textColor;
        //                }
        //                else
        //                {
        //                    backgroundPixels++;

        //                    wr1[p] = backgroundColor;
        //                    wr0[p] = backgroundColor;
        //                }

        //    if (backgroundMoreThanText && textPixels < backgroundPixels)
        //    {
        //        return TestFloodFillImageInvertColor(bmp, backgroundColor, textColor, kernel);
        //    }

        //    return result;
        //}

        // Тест 0
        private Bitmap TestFloodFillImageInvertColor(Bitmap bmp, Color textColor, Color backgroundColor, double[,] kernel)
        {
            int textPixels = 0;
            int backgroundPixels = 0;

            var dilated = (Bitmap)bmp.Clone();
            var result = Convolution(dilated, kernel);

            using (var wr0 = new ImageWrapper(bmp, true))
            using (var wr1 = new ImageWrapper(dilated, true))
                foreach (var p in wr0)
                    if (wr1[p].G > 0)
                        if ((wr1[p] = wr0[p]).G > 128)
                        {
                            textPixels++;

                            wr1[p] = textColor;
                            wr0[p] = textColor;
                        }
                        else
                        {
                            backgroundPixels++;

                            wr1[p] = backgroundColor;
                            wr0[p] = backgroundColor;
                        }

            if(textPixels < backgroundPixels)
            {
                return TestFloodFillImageInvertColor(bmp, backgroundColor, textColor, kernel);
            }

            return result;
        }

        // Какая-то работа с изображением
        //private Bitmap ConvolutionV2FromByteArray(byte[] img, double[,] matrix)
        //{
        //    var w = matrix.GetLength(0);
        //    var h = matrix.GetLength(1);

        //    using (var wr = new ImageWrapper(img) { DefaultColor = Color.Silver })
        //        foreach (var p in wr)
        //        {
        //            double r = 0d, g = 0d, b = 0d;

        //            for (int i = 0; i < w; i++)
        //                for (int j = 0; j < h; j++)
        //                {
        //                    var pixel = wr[p.X + i - 1, p.Y + j - 1];
        //                    r += matrix[j, i] * pixel.R;
        //                    g += matrix[j, i] * pixel.G;
        //                    b += matrix[j, i] * pixel.B;
        //                }
        //            wr.SetPixel(p, r, g, b);
        //        }

        //    return img;
        //}

        // Какая-то работа с изображением
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

        // Инвертер цветов
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
        public byte[] GetPreparedImage(Bitmap image, Color textColor, Color backgroundColor, bool backgroundMoreThanText, int floodValue)
        {

            //var blackWhiteBitmap = FloodFillImage(image, floodValue, textColor, backgroundColor);
            //var invertedBlackWhiteBitmap = InvertImageColor(blackWhiteBitmap, floodValue, textColor, backgroundColor);
            //var result = BitmapToByte(invertedBlackWhiteBitmap);
            //invertedBlackWhiteBitmap.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{floodValue}.png", ImageFormat.Png);

            var tt = TestFloodFillImage(image, textColor, backgroundColor, floodValue, backgroundMoreThanText);
            //tt.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedImage{floodValue}_{new Random().Next()}.png", ImageFormat.Png);
            var result = BitmapToByte(tt);

            return result;
        }
    }
}
