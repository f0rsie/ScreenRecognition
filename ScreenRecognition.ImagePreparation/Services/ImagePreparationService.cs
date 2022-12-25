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

            var kernel = new double[,]
                 {{0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1},
                  {0.1, 0.1, 0.1}};

            var result = NewTestFloodFillImageAuto(bmp, textColor, backgroundColor);

            //var res = TestFloodFillImageAuto(bmp, kernel);
            //var result = TestFloodFillImageInvertColorV2(gg, textColor, backgroundColor, kernel, res.R, backgroundMoreThanText);

            //bmp.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/origImage{res.G}_{random}.png", ImageFormat.Png);
            //result.Save($"C:/Users/fff/Desktop/Диплом на диске C/Results/convertedOrigImage{floodValue}_{random}.png", ImageFormat.Png);
            return result;
        }

        private Bitmap NewTestFloodFillImageAuto(Bitmap bmp, Color firstPixelColor, Color secondPixelColor)
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

        private Color TestFloodFillImageAuto(Bitmap bmp, double[,] kernel)
        {
            List<Color> pixelColor = new();
            int a = 0, r = 0, g = 0, b = 0;

            var dilated = (Bitmap)bmp.Clone();

            using (var wr0 = new ImageWrapper(bmp, true))
            using (var wr1 = new ImageWrapper(dilated, true))
            {
                foreach (var p in wr0)
                {
                    pixelColor.Add(wr1[p]);
                }

                foreach (var item in pixelColor)
                {
                    a += item.A;
                    r += item.R;
                    g += item.G;
                    b += item.B;
                }

                a /= pixelColor.Count;
                r /= pixelColor.Count;
                g /= pixelColor.Count;
                b /= pixelColor.Count;
            }
            Color colorResult = Color.FromArgb(a, r, g, b);

            int floodValue = ((b + g + b) / 3);

            return colorResult;
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
                    if (wr1[p].R > 0)
                        if ((wr1[p] = wr0[p]).R > floodValue)
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
                return TestFloodFillImageInvertColorV2(bmp, textColor, backgroundColor, kernel, floodValue, backgroundMoreThanText);
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

            if (textPixels < backgroundPixels)
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
