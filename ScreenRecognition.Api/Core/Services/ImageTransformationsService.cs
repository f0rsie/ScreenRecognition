using ScreenRecognition.Api.Models;
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

        private List<Thread> _threads;
        private List<byte[]> _results;

        private SKColor _startPixelColor;
        private int _imageSize;

        private int _countParts = 0;
        private double _maxImagePartLength = 500000.0;

        private List<ImageSeparationThreadModel> _newResults;

        private byte[] _inputImage;

        public ImageTransformationsService()
        {
            _threads = new List<Thread>();
            _results = new List<byte[]>();

            _newResults = new List<ImageSeparationThreadModel>();

            _imagePreparationService = new ImagePreparationService();
        }

        // Работа с изображением
        public List<byte[]> PreparedImages(byte[] inputImage)
        {
            List<byte[]>? result = new List<byte[]>();

            try
            {
                _results = new();
                _threads = new();
                _newResults = new();

                _inputImage = inputImage;
                //var bmp = ImagePreparationService.ByteToBitmap(inputImage);
                var bmp = SKBitmap.Decode(inputImage);

                //_startPixelColor = bmp.GetPixel(new Random().Next(0, bmp.Width), new Random().Next(0, bmp.Height));
                _startPixelColor = bmp.GetPixel(0, 0);
                _imageSize = bmp.Width * bmp.Height;

                List<ImageSeparationThreadModel> models = new();

                int countParts = (int)(inputImage.Length / _maxImagePartLength);
                if (countParts <= 0)
                    countParts = 1;
                else if (inputImage.Length / _maxImagePartLength > countParts)
                    countParts++;

                //var imageParts = ImageSeparation(bmp, countParts);
                var imageParts = new List<byte[]> { inputImage };

                int currentThreadsNumber = 0;

                for (int i = 0; i < imageParts.Count; i++)
                {
                    var model = new ImageSeparationThreadModel(imageParts[i], i, "default");
                    models.Add(model);
                    _threads.Add(new Thread(Prepare));
                    _threads[currentThreadsNumber].Start(model);

                    currentThreadsNumber++;
                }

                while (true)
                {
                    if (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() == 0)
                        break;
                }

                //result = WholeImage(countParts);
                result = _newResults.Select(e => e.ImagePart).ToList();

                ByteToBitmap(inputImage).Save("D:/Original.png", System.Drawing.Imaging.ImageFormat.Png);
                ByteToBitmap(result.FirstOrDefault()).Save("D:/Result.png", System.Drawing.Imaging.ImageFormat.Png);
            }
            catch { }

            return result;
        }

        // На удаление в будущем
        public static Bitmap ByteToBitmap(byte[] image)
        {
            var result = new Bitmap(new MemoryStream(image));

            return result;
        }

        // New
        private void Prepare(object? model)
        {
            try
            {
                int i = 0;
                var sepModel = model as ImageSeparationThreadModel;

                var bmp = SKBitmap.Decode(sepModel?.ImagePart);

                var image = _imagePreparationService.GetPreparedImage(bmp, SKColors.White, SKColors.Black, _startPixelColor, _imageSize);

                ImageSeparationThreadModel result = new ImageSeparationThreadModel
                {
                    ImagePart = image,
                    Number = sepModel.Number,
                    Type = "default",
                };

                _newResults.Add(result);
            }
            catch { }
        }
    }
}