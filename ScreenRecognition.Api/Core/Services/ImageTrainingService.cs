using Google.Protobuf.WellKnownTypes;
using ScreenRecognition.Api.Models;
using ScreenRecognition.ImagePreparation.Services;
using System.Drawing;

namespace ScreenRecognition.Api.Core.Services
{
    public class ImageTrainingService
    {
        private ImagePreparationService _imagePreparationService;

        private List<Thread> _threads;
        private List<byte[]> _results;

        private byte[] _inputImage;

        public ImageTrainingService()
        {
            _threads = new List<Thread>();
            _results = new List<byte[]>();

            _imagePreparationService = new ImagePreparationService();
        }

        public List<byte[]> PreparedImages(byte[] inputImage)
        {
            _inputImage = inputImage;

            int currentThreadNumber = 0;

            for (int i = 50; i <= 200; i += 50)
            {
                _threads.Add(new Thread(Prepare));
                _threads[currentThreadNumber].Start(i);

                currentThreadNumber++;
            }
            _threads.Add(new Thread(Prepare));
            _threads[currentThreadNumber].Start(null);

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0) { }

            return _results;
        }

        private void Prepare(object? floodValue)
        {
            if (floodValue == null)
            {
                _results.Add(_inputImage);

                return;
            }

            var image = _imagePreparationService.GetPreparedImage(ImagePreparationService.ByteToBitmap(_inputImage), Color.Black, Color.White, int.Parse(floodValue.ToString()));

            _results.Add(image);
        }
    }
}