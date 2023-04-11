using SkiaSharp;

namespace ScreenRecognition.ImagePreparation.Services
{
    public class ImagePreparationService
    {
        private SKBitmap _image = null!;

        private SKColor _startPixelColor;
        private SKColor _firstColor;
        private SKColor _secondColor;

        private int _imageWidth = 0;
        private int _imageHeight = 0;

        public byte[] GetPreparedImage(byte[] image, SKColor firstColor, SKColor secondColor)
        {
            _image = SKBitmap.Decode(image);
            _firstColor = firstColor;
            _secondColor = secondColor;
            _startPixelColor = _image.GetPixel(0, 0);
            _imageWidth = _image.Width;
            _imageHeight = _image.Height;

            var blackWhiteBitmap = ImageColorSetter();
            var result = blackWhiteBitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();

            return result;
        }

        private SKBitmap ImageColorSetter()
        {
            var result = _image;

            IntPtr pixelsPtr = result.GetPixels();

            unsafe
            {
                byte* ptr = (byte*)pixelsPtr.ToPointer();

                for (int row = 0; row < result.Width; row++)
                {
                    for (int col = 0; col < result.Height; col++)
                    {
                        var color = _firstColor;

                        if (IsColorsSimilar(_startPixelColor, new SKColor(*ptr, *(ptr + 1), *(ptr + 2), *(ptr + 3))))
                            color = _firstColor;
                        else
                            color = _secondColor;

                        *ptr++ = color.Red;      // red
                        *ptr++ = color.Green;    // green
                        *ptr++ = color.Blue;     // blue
                        *ptr++ = color.Alpha;    // alpha
                    }
                }
            }

            return result;
        }

        private bool IsColorsSimilar(SKColor firstColor, SKColor secondColor)
        {
            double ratio = Math.Abs(_imageWidth - _imageHeight);

            int colorDifference = (int)(255.0 / ratio);
            colorDifference = 100;

            bool result = Math.Sqrt(Math.Pow(firstColor.Red - secondColor.Red, 2f) + Math.Pow(firstColor.Green - secondColor.Green, 2f) + Math.Pow(firstColor.Blue - secondColor.Blue, 2f)) < colorDifference;

            return result;
        }
    }
}
