using ScreenRecognition.ImagePreparation.Services;

namespace ScreenRecognition.Api.Core.Services
{
    public class ImageAnalyzerService
    {
        private string _language;

        public ImageAnalyzerService(string language = "eng")
        {
            _language = language;
        }

        // Над этим нужно будет ещё подумать
        public string GetTextFromImage(byte[] image)
        {
            var imagePreparationService = new ImagePreparationService();
            var tesseractOcrService = new TesseractOcrService(_language);

            byte[]? preparedImage = new byte[0];
            string? textResult = "";

            // Пока есть идея, чтобы этот цикл выполнялся все 5 раз (с 50 до 250), а после запускалось одновременно 5 экземпляров ОКРа.
            // После завершения работы всех экземпляров ОКРа будут сравниваться результаты работы.
            // Тот результат, где будет больше всего текста(?), будет финальным результатом, который будет передаваться в переводчик.
            // Но так как в проекте кроме этого ничего нет, то пока пусть останется так, доработкой этого момента займусь позже.
            // 05.10.2022 16:35 - Обновление: скорее всего следующее, чем я займусь в этом проекте - это реализация того, что ныписал выше
            for (int i = 50; i <= 200; i += 50)
            {
                preparedImage = imagePreparationService.GetPreparedImage(ImagePreparationService.ByteToBitmap(image), i);

                if ((textResult = tesseractOcrService.GetText(preparedImage).Replace("\n\n", "\n").Replace("\n", " \n ")).Replace("\n", " ").Replace(" ", "").Length > 4)
                {
                    break;
                }
            }

            return textResult;
        }
    }
}
