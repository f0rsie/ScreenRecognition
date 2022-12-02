using ScreenRecognition.ImagePreparation.Services;
using System.Drawing;

namespace ScreenRecognition.Api.Core.Services
{
    public class ImageAnalyzerService
    {
        private string _inputLanguage;
        private byte[]? _image;

        private List<Thread> _threads;
        private List<(string?, float)>? _results;

        public ImageAnalyzerService(string inputLanguage = "eng")
        {
            _inputLanguage = inputLanguage;

            _threads = new List<Thread>();
            _results = new List<(string?, float)>();
        }

        public (string?, float) GetTextFromImage(byte[] image)
        {
            _image = image;
            int currentThreadNumber = 0;

            for (int i = 50; i <= 200; i += 50)
            {
                _threads.Add(new Thread(PreparedImageMethod));
                _threads[currentThreadNumber].Start(i);

                currentThreadNumber++;
            }

            _threads.ForEach(new System.Threading.)

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0)
            {

            }

            (string?, float) result = ("", 0);

            if (_results?.Count > 0)
            {
                result = _results[0];

                foreach (var item in _results)
                {
                    if (result.Item2 < item.Item2)
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
                (string?, float) textResult = ("", 0);

                preparedImage = imagePreparationService.GetPreparedImage(ImagePreparationService.ByteToBitmap(_image), Color.Black, Color.White, int.Parse(floodValue.ToString()));

                if ((textResult = tesseractOcrService.GetText(preparedImage)).Item1?.Replace("\n\n", "\n").Replace("\n", " \n ").Replace("\n", " ").Replace(" ", "").Length >= 2)
                {
                    _results?.Add(textResult);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
