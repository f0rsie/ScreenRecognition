using ScreenRecognition.ImagePreparation.Services;
using SkiaSharp;
using System.Drawing;

namespace ScreenRecognition.Api.Core.Services
{
    /// <summary>
    /// В этом классе какая-то проблема, но я не пойму какая
    /// </summary>
    public class ImageTransformationsService
    {
        private ImagePreparationService _imagePreparationService;
        private List<byte[]> _results;

        public ImageTransformationsService()
        {
            _imagePreparationService = new();
            _results = new();
        }

        // Работа с изображением
        public List<byte[]> PreparedImages(byte[] inputImage)
        {
            _results = new()
            {
                Prepare(inputImage),
            };

            // Для дебага картинок
            ByteToBitmap(inputImage).Save("D:/Original.png", System.Drawing.Imaging.ImageFormat.Png);
            ByteToBitmap(_results.FirstOrDefault()).Save("D:/Result.png", System.Drawing.Imaging.ImageFormat.Png);

            return _results;
        }

        // Метод обработки фото
        private byte[] Prepare(byte[] startedImage)
        {
            return _imagePreparationService.GetPreparedImage(startedImage, SKColors.White, SKColors.Black);
        }

        // Для дебага картинок
        private Bitmap ByteToBitmap(byte[] image)
        {
            return new Bitmap(new MemoryStream(image));
        }
    }
}