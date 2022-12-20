﻿using ScreenRecognition.Api.Core.Services.Translators;

namespace ScreenRecognition.Api.Core.Services
{
    public class NewTextTranslationService
    {
        private readonly string _translationApiKey;
        private readonly byte[] _image;
        private readonly string _inputLanguage;
        private readonly string _outputLanguage;

        private ITextTranslatorService _textTranslatorService;

        //public async Task<string?> GetTranslate(byte[] image, string inputLanguages, string outputLanguage, string translationApiKey)
        //{

        //}

        public NewTextTranslationService(string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage)
        {
            _translationApiKey = translationApiKey;
            _image = image.ToArray();
            _inputLanguage = inputLanguage;
            _outputLanguage = outputLanguage;

            _textTranslatorService = new MyMemoryTextTranslator();
        }

        public async Task<string?> GetTranslate1()
        {
            string textFromImage = GetText();
            string? translateResult = await _textTranslatorService.Translate(textFromImage, _inputLanguage, _outputLanguage, _translationApiKey);

            return translateResult;
        }

        private string? GetText()
        {
            var imageAnalyzerService = new ImageAnalyzerService(_inputLanguage);

            var result = imageAnalyzerService.GetTextFromImage(_image);

            if (result.TextResult != null)
            {
                return result.TextResult;
            }

            return "Не распознано";
        }
    }
}
