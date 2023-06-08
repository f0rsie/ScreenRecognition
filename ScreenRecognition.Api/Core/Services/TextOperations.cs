using Microsoft.IdentityModel.Tokens;
using ScreenRecognition.Api.Core.Services.OCRs;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;
using ScreenRecognition.Api.Resources.Exceptions;
using ScreenRecognition.Modules.Modules;
using System.Reflection;

namespace ScreenRecognition.Api.Core.Services
{
    public class TextOperations
    {
        private ITextTranslatorService? _textTranslatorService;
        private IOcrService? _ocrService;

        private List<Thread> _threads;
        private List<OcrResultModel>? _results;

        private DBOperations _dBOperations;

        private string? _inputLanguages;

        public TextOperations()
        {
            _threads = new();
            _results = new();
            _dBOperations = new();
        }

        // TODO: Проверить новый метод (ещё не проверял)
        public async Task<List<string>?> GetTranslate(string translatorName, string inputText, string inputLanguage, string outputLanguage, string translationApiKey)
        {
            if (!translatorName.ToLower().Contains("texttranslator"))
                translatorName += "TextTranslator";

            _textTranslatorService = FindElement<ITextTranslatorService>(translatorName);

            if (_textTranslatorService == null)
                throw new FindElementException();

            List<string>? result = new();
            var translatorInputLangAlias = await _dBOperations.GetTranslatorLanguageAlias(inputLanguage);
            var translatorOutputLangAlias = await _dBOperations.GetTranslatorLanguageAlias(outputLanguage);

            var textList = TextToTextArray(inputText);

            if (textList.Count > 0)
            {
                foreach (var text in textList)
                {
                    var textResult = await _textTranslatorService.Translate(inputText, translatorInputLangAlias, translatorOutputLangAlias, translationApiKey);
                    result = result.Concat(textResult).ToList();
                }
            }
            else
            {
                result = await _textTranslatorService.Translate(inputText, translatorInputLangAlias, translatorOutputLangAlias, translationApiKey);
            }

            if (result.IsNullOrEmpty())
                throw new TranslateException();

            return result;
        }

        public async Task<OcrResultModel> GetText(string ocrName, byte[] image, string inputLanguage)
        {
            if (!ocrName.ToLower().Contains("ocr"))
                ocrName += "Ocr";

            _ocrService = FindElement<IOcrService>(ocrName);

            var ocrLangAlias = await _dBOperations.GetOcrLanguageAlias(inputLanguage);

            _inputLanguages = ocrLangAlias;

            if (_ocrService == null)
                throw new FindElementException();

            var preparedImages = ImagePrepare(image);

            GetOcrResult(preparedImages);

            if (_results.IsNullOrEmpty())
                throw new RecognitionException();

            var result = _results?.OrderBy(e => e.Confidence).LastOrDefault();

            if (string.IsNullOrEmpty(result?.TextResult) || string.IsNullOrWhiteSpace(result?.TextResult))
                throw new RecognitionException();

            return result;
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
                if (!_threads.Where(e => e.ThreadState == ThreadState.Running).Any())
                    break;
            }
        }

        private void OcrOperationsMethod(object? image)
        {
            var result = _ocrService?.GetText(image as byte[], _inputLanguages);

            if (!String.IsNullOrEmpty(result?.TextResult))
                _results?.Add(result);
        }

        // TODO: Сделать разделение на предложения по 500 символов каждое
        private List<string> TextToTextArray(string text)
        {
            List<string> result = new();
            string resultStr = string.Empty;
            var splittedText = text.Split(".");

            for (int i = 0; i < splittedText.Length; i++)
            {
                resultStr += $"{splittedText[i]}.";

                if (resultStr.Length > 500)
                {
                    resultStr = resultStr.Replace($"{splittedText[i]}.", "");

                    result.Add(resultStr);
                    resultStr = string.Empty;
                    i--;
                }
                else
                {
                    continue;
                }
            }

            return result;
        }

        private List<byte[]> ImagePrepare(byte[] image)
        {
            var imageAnalyzerService = new ImageTransformationsService();

            var result = imageAnalyzerService.PreparedImages(image);
            result.Add(image);

            return result;
        }

        public static T? FindElement<T>(string name)
        {
            return ProgramElementFinder.FindByName<T?>(name, Assembly.GetExecutingAssembly().FullName);
        }
    }
}
