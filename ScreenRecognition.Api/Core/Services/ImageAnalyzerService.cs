using ScreenRecognition.ImagePreparation.Services;
using System.Drawing;

namespace ScreenRecognition.Api.Core.Services
{
    public class ImageAnalyzerService
    {
        private string _language;
        private byte[] _image;

        private List<Thread> _threads;
        private static List<string>? s_results;
        private Dictionary<string, int> _threadFloodValue;

        public ImageAnalyzerService(string language = "eng")
        {
            _language = language;

            _threads = new List<Thread>();
            s_results = new List<string>();
            _threadFloodValue = new Dictionary<string, int>();
        }

        // Над этим нужно будет ещё подумать
        public string GetTextFromImage(byte[] image)
        {
            _image = image;
            int currentThreadNumber = 0;

            // Пока есть идея, чтобы этот цикл выполнялся все 5 раз (с 50 до 250), а после запускалось одновременно 5 экземпляров ОКРа.
            // После завершения работы всех экземпляров ОКРа будут сравниваться результаты работы.
            // Тот результат, где будет больше всего текста(?), будет финальным результатом, который будет передаваться в переводчик.
            // Но так как в проекте кроме этого ничего нет, то пока пусть останется так, доработкой этого момента займусь позже.
            // 05.10.2022 16:35 - Обновление: скорее всего следующее, чем я займусь в этом проекте - это реализация того, что ныписал выше
            for (int i = 50; i <= 250; i += 50)
            {
                _threads.Add(new Thread(PreparedImageMethod));
                _threads[currentThreadNumber].Name = currentThreadNumber.ToString();
                _threadFloodValue.Add(_threads[currentThreadNumber].Name, i);
                _threads[currentThreadNumber].Start();

                currentThreadNumber++;
            }

            while (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() > 0)
            {

            }

            return s_results[new Random().Next(0, s_results.Count - 1)];
        }

        private void PreparedImageMethod()
        {
            var imagePreparationService = new ImagePreparationService();
            var tesseractOcrService = new TesseractOcrService(_language);

            byte[]? preparedImage = new byte[0];
            string? textResult = "";
            int floodValue = _threadFloodValue.FirstOrDefault(e => e.Key == Thread.CurrentThread.Name).Value;

            preparedImage = imagePreparationService.GetPreparedImage(ImagePreparationService.ByteToBitmap(_image), Color.Black, Color.White, floodValue);

            if ((textResult = tesseractOcrService.GetText(preparedImage).Replace("\n\n", "\n").Replace("\n", " \n ")).Replace("\n", " ").Replace(" ", "").Length > 4)
            {
                s_results?.Add(textResult);
            }
        }
    }
}
