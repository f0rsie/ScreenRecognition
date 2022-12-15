﻿using ScreenRecognition.Api.Core.Services.Translators;

namespace ScreenRecognition.Api.Core.Services
{
    public class TextTranslationService
    {
        private readonly string _translationApiKey;
        private readonly byte[] _image;
        private readonly string _inputLanguage;
        private readonly string _outputLanguage;

        private ITextTranslatorService _textTranslatorService;

        public TextTranslationService(string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage)
        {
            _translationApiKey = translationApiKey;
            _image = image.ToArray();
            _inputLanguage = inputLanguage;
            _outputLanguage = outputLanguage;

            _textTranslatorService = new MyMemoryTextTranslator();
        }

        public string? GetTranslate()
        {
            string textFromImage = GetText();
            string? translateResult = _textTranslatorService.Translate(textFromImage, _inputLanguage, _outputLanguage, _translationApiKey);

            return translateResult;
        }

        private string? GetText()
        {
            var imageAnalyzerService = new ImageAnalyzerService(_inputLanguage);

            var result = imageAnalyzerService.GetTextFromImage(_image);

            return result.TextResult;
        }
    }
}
