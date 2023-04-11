using ScreenRecognition.ImagePreparation.Services;
using SkiaSharp;

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

            // Для дебага фото
            SaveImgToFile("D:/Диплом/Results/Original.png", inputImage);
            SaveImgToFile("D:/Диплом/Results/Result.png", _results.FirstOrDefault());

            return _results;
        }

        // Метод обработки фото
        private byte[] Prepare(byte[] startedImage)
        {
            return _imagePreparationService.GetPreparedImage(startedImage, SKColors.White, SKColors.Black);
        }

        // Для дебага фото
        private async void SaveImgToFile(string filePath, byte[]? inputImage)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await fileStream.WriteAsync(inputImage);
                }
            }
            catch { }
        }
    }
}