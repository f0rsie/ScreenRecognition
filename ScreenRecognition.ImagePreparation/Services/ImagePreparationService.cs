using SkiaSharp;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        // New methods
        public byte[] GetPreparedImage(SKBitmap image, SKColor firstColor, SKColor secondColor, SKColor startPixelColor, int imageSize)
        {
            var blackWhiteBitmap = ImageColorSetter(image, firstColor, secondColor, startPixelColor, imageSize);
            var result = blackWhiteBitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();

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
                            color = firstPixelColor;
                        else
                            color = secondPixelColor;

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
            double ratio = 4;

            if (imageSize >= 200000 && imageSize < 400000)
                ratio = 2;
            else if (imageSize >= 400000)
                ratio = 1.5;

            int colorDifference = (int)(255.0 / ratio);
            colorDifference = 150;

            bool result = Math.Sqrt(Math.Pow(firstColor.Red - secondColor.Red, 2f) + Math.Pow(firstColor.Green - secondColor.Green, 2f) + Math.Pow(firstColor.Blue - secondColor.Blue, 2f)) < colorDifference;

            return result;
        }
    }
}
