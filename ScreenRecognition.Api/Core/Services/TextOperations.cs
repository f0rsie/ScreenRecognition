﻿using Microsoft.IdentityModel.Tokens;
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

        private DBOperations _dBOperations;

        private string? _inputLanguages;

        public TextOperations()
        {
            _threads = new List<Thread>();
            s_results = new List<OcrResultModel>();

            _dBOperations = new();
        }

        public async Task<string> GetTranslate(string translatorName, string inputText, string inputLanguage, string outputLanguage, string translationApiKey)
        {
            try
            {
                _textTranslatorService = FindElement<ITextTranslatorService>(translatorName);

                if (_textTranslatorService == null)
                    return "Переводчик не найден";

                var translatorInputLangAlias = await _dBOperations.GetTranslatorLanguageAlias(inputLanguage);
                var translatorOutputLangAlias = await _dBOperations.GetTranslatorLanguageAlias(outputLanguage);

                var result = await _textTranslatorService.Translate(inputText, translatorInputLangAlias, translatorOutputLangAlias, translationApiKey);

                if (!String.IsNullOrEmpty(result))
                    return result;
            }
            catch { }

            return "Ошибка перевода";
        }

        public async Task<OcrResultModel> GetText(string ocrName, byte[] image, string inputLanguage)
        {
            try
            {
                _ocrService = FindElement<IOcrService>(ocrName);

                var ocrLangAlias = await _dBOperations.GetOcrLanguageAlias(inputLanguage);

                _inputLanguages = ocrLangAlias;

                if (_ocrService == null)
                    return new OcrResultModel
                    {
                        TextResult = "OCR не найден",
                        Confidence = 0,
                    };

                var preparedImages = ImagePrepare(image);

                GetOcrResult(preparedImages);

                var result = s_results?.OrderBy(e => e.Confidence).ToArray()[s_results.Count - 1];

                if (!String.IsNullOrEmpty(result?.TextResult))
                    return result;
            }
            catch { }

            return new OcrResultModel
            {
                TextResult = "Не распознано",
                Confidence = 0,
            };
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

            var result = imageAnalyzerService.PreparedImages(image);
            result.Add(image);

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
