using SkiaSharp;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        #region Deprecated
        //// Конвертер из цветного в чёрно-белое
        //private Bitmap FloodFillImage(Bitmap bmp, Color firstColor, Color secondColor, Color startPixelColor, int imageSize)
        //{
        //    var result = ImageColorSetter(bmp, firstColor, secondColor, startPixelColor, imageSize);

        //    return result;
        //}

        //private Bitmap ImageColorSetter(Bitmap bmp, Color firstPixelColor, Color secondPixelColor, Color startPixelColor, int imageSize)
        //{
        //    int firstPixelType = 0;
        //    int secondPixelType = 0;

        //    var result = (Bitmap)bmp.Clone();

        //    using (var wr0 = new ImageWrapper(bmp, true))
        //    using (var wr1 = new ImageWrapper(result, true))
        //    {
        //        foreach (var p in wr0)
        //        {
        //            if (IsColorsSimilar(startPixelColor, wr1[p], imageSize))
        //            {
        //                wr1[p] = firstPixelColor;

        //                firstPixelType++;
        //            }
        //            else
        //            {
        //                wr1[p] = secondPixelColor;

        //                secondPixelType++;
        //            }
        //        }
        //    }

        //    return result;
        //}

        //// Проверка двух пикселей на похожесть
        //// Переделать: необходимо менять colorDifference в зависимости от размера изображения; чем меньше изображение - тем меньше должно быть это числл, а чем оно больше - тем больше это число
        //// UDP: передалано, но работает не совсем так, как расчитывал. Необходима дальнейшная переработка
        //private bool IsColorsSimilar(Color firstColor, Color secondColor, int imageSize)
        //{
        //    int ratio = 2;

        //    if (imageSize <= 800000)
        //        ratio = 6;
        //    if (imageSize <= 400000)
        //        ratio = 4;

        //    int colorDifference = (int)(255 / ratio);
        //    bool result = Math.Abs(firstColor.R - secondColor.R) < colorDifference && Math.Abs(firstColor.G - secondColor.G) < colorDifference && Math.Abs(firstColor.B - secondColor.B) < colorDifference;

        //    return result;
        //}

        //// Какая-то работа с изображением (пусть на всякий останется)
        //private Bitmap Convolution(Bitmap img, double[,] matrix)
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

        //// Конвертер Bitmap в Byte array
        //public static byte[] BitmapToByte(Bitmap bitmap, ImageFormat? imageFormat = null)
        //{
        //    imageFormat ??= ImageFormat.Bmp;

        //    var sampleStream = new MemoryStream();
        //    bitmap.Save(sampleStream, imageFormat);

        //    return sampleStream.ToArray();
        //}

        //// Конвертер Byte array в Bitmap
        //public static Bitmap ByteToBitmap(byte[] image)
        //{
        //    var result = new Bitmap(new MemoryStream(image));

        //    return result;
        //}

        //// Вызывает всё, что нужно
        //public byte[] GetPreparedImage(Bitmap image, Color firstColor, Color secondColor, Color startPixelColor, int imageSize)
        //{
        //    var blackWhiteBitmap = FloodFillImage(image, firstColor, secondColor, startPixelColor, imageSize);
        //    var result = BitmapToByte(blackWhiteBitmap);

        //    return result;
        //}

        #endregion

        // New methods
        public byte[] GetPreparedImage(SKBitmap image, SKColor firstColor, SKColor secondColor, SKColor startPixelColor, int imageSize)
        {
            var blackWhiteBitmap = FloodFillImage(image, firstColor, secondColor, startPixelColor, imageSize);
            var result = blackWhiteBitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();

            //ByteToBitmap(result).Save("D:/Result.png", ImageFormat.Png);

            return result;
        }

        private SKBitmap FloodFillImage(SKBitmap bmp, SKColor firstColor, SKColor secondColor, SKColor startPixelColor, int imageSize)
        {
            var result = ImageColorSetter(bmp, firstColor, secondColor, startPixelColor, imageSize);

            return result;
        }

        private SKBitmap ImageColorSetter(SKBitmap bmp, SKColor firstPixelColor, SKColor secondPixelColor, SKColor startPixelColor, int imageSize)
        {
            var result = bmp;

            IntPtr pixelsPtr = result.GetPixels();

            unsafe
            {
                byte* ptr = (byte*)pixelsPtr.ToPointer();

                for (int row = 0; row < result.Width; row++)
                {
                    for (int col = 0; col < result.Height; col++)
                    {
                        var color = firstPixelColor;

                        if (IsColorsSimilar(startPixelColor, new SKColor(*ptr, *(ptr + 1), *(ptr + 2), *(ptr + 3)), imageSize))
                        {
                            color = firstPixelColor;
                        }
                        else
                        {
                            color = secondPixelColor;
                        }

                        *ptr++ = color.Red;      // red
                        *ptr++ = color.Green;    // green
                        *ptr++ = color.Blue;     // blue
                        *ptr++ = color.Alpha;    // alpha
                    }
                }
            }

            return result;
        }

        private bool IsColorsSimilar(SKColor firstColor, SKColor secondColor, int imageSize)
        {
            int ratio = 2;

            if (imageSize <= 800000)
                ratio = 6;
            if (imageSize <= 400000)
                ratio = 4;

            int colorDifference = (int)(255 / ratio);
            bool result = Math.Abs(firstColor.Red - secondColor.Red) < colorDifference && Math.Abs(firstColor.Green - secondColor.Green) < colorDifference && Math.Abs(firstColor.Blue - secondColor.Blue) < colorDifference;

            return result;
        }
    }
}
