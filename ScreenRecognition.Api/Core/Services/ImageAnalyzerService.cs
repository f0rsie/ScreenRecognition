using Google.Protobuf.WellKnownTypes;
using ScreenRecognition.Api.Models;
using ScreenRecognition.ImagePreparation.Services;
using System.Drawing;

namespace ScreenRecognition.Api.Core.Services
{
    public class ImageAnalyzerService
    {
        private string _inputLanguage;
        private byte[]? _image;

        private List<Thread> _threads;
        private List<OcrResultModel>? _results;

        public ImageAnalyzerService(string inputLanguage)
        {
            _inputLanguage = inputLanguage;

            _threads = new List<Thread>();
            _results = new List<OcrResultModel>();
        }

        public OcrResultModel GetTextFromImage(byte[] image)
        {
            int currentThreadNumber = 0;
            _image = image;

            for (int i = 50; i <= 200; i += 50)
            {
                _threads.Add(new Thread(PreparedImageMethod));
                _threads[currentThreadNumber].Start(i);

                currentThreadNumber++;
            }
            _threads.Add(new Thread(PreparedImageMethod));
            _threads[currentThreadNumber].Start(null);
            currentThreadNumber++;

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0)
            {

            }

            OcrResultModel result = new OcrResultModel();

            if (_results?.Count > 0)
            {
                result = _results[0];

                foreach (var item in _results)
                {
                    if (result.Confidence < item.Confidence)
                        result = item;
                }
            }

            return result;
        }

        private void PreparedImageMethod(object? floodValue)
        {
            try
            {
                var imagePreparationService = new ImagePreparationService();
                var tesseractOcrService = new TesseractOcrService(_inputLanguage);

                byte[]? preparedImage = new byte[0];
                OcrResultModel textResult;

                if (floodValue != null)
                    preparedImage = imagePreparationService.GetPreparedImage(ImagePreparationService.ByteToBitmap(_image), Color.Black, Color.White, int.Parse(floodValue.ToString()));
                else
                    preparedImage = _image;

                if ((textResult = tesseractOcrService.GetText(preparedImage)).TextResult?.Replace("\n\n", "\n").Replace("\n", " \n ").Replace("\n", " ").Replace(" ", "").Length >= 2)
                {
                    _results?.Add(textResult);
                }
            }

            catch { }
        }
    }
}