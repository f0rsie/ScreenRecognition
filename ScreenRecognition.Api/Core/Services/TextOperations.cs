using Microsoft.IdentityModel.Tokens;
using ScreenRecognition.Api.Core.Services.OCRs;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models;
using ScreenRecognition.Modules.Modules;
using System.Reflection;

namespace ScreenRecognition.Api.Core.Services
{
    public class TextOperations
    {
        private ITextTranslatorService? _textTranslatorService;
        private IOcrService? _ocrService;

        private List<Thread> _threads;
        private static List<OcrResultModel> s_results;

        private string? _inputLanguages;

        public TextOperations()
        {
            _threads = new List<Thread>();
            s_results = new List<OcrResultModel>();
        }

        public async Task<string> GetTranslate(string translatorName, string inputText, string inputLanguages, string outputLanguage, string? translationApiKey = null)
        {
            _textTranslatorService = FindElement<ITextTranslatorService>(translatorName);

            if (_textTranslatorService == null)
                return "Переводчик не найден";

            string result = await _textTranslatorService.Translate(inputText, inputLanguages, outputLanguage, translationApiKey);

            if (!String.IsNullOrEmpty(result))
                return result;

            return "Ошибка перевода";
        }

        public string GetText(string ocrName, byte[] image, string inputLanguages)
        {
            _ocrService = FindElement<IOcrService>(ocrName);
            _inputLanguages = inputLanguages;

            if (_ocrService == null)
                return "OCR не найден";

            var preparedImages = ImagePrepare(image);

            GetOcrResult(preparedImages);

            var result = s_results?.OrderBy(e => e.Confidence).ToArray()[s_results.Count - 1].TextResult;

            if (!String.IsNullOrEmpty(result))
                return result;

            return "Не распознано";
        }

        private void GetOcrResult(List<byte[]> images)
        {
            int threadsCount = 0;

            foreach (var item in images)
            {
                _threads.Add(new Thread(OcrOperationsMethod));
                _threads[threadsCount].Start(item);

                threadsCount++;
            }

            while (true) 
            {
                if (_threads.Where(e => e.ThreadState == ThreadState.Running).Count() == 0)
                    break;
            }
        }

        private void OcrOperationsMethod(object? image)
        {
            var result = _ocrService?.GetText(image as byte[], _inputLanguages);

            if (!String.IsNullOrEmpty(result?.TextResult))
                s_results.Add(result);
        }

        private List<byte[]> ImagePrepare(byte[] image)
        {
            var imageAnalyzerService = new ImageTransformationsService();

            var result = imageAnalyzerService.PreparedImagesV3(image);

            return result;
        }

        public static T? FindElement<T>(string name)
        {
            T? result = default(T);
            try
            {
                result = (T?)ProgramElementFinder.FindByName<object>(name, Assembly.GetExecutingAssembly().FullName);
            }
            catch { }

            return result;
        }
    }
}
