namespace ScreenRecognition.Api.Core.Services
{
    public class TextTranslatorService
    {
        private readonly string _translationApiKey;
        private readonly byte[] _image;
        private readonly string _inputLanguage;
        private readonly string _outputLanguage;

        //Thread? _thread;

        public TextTranslatorService(string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage)
        {
            _translationApiKey = translationApiKey;
            _image = image.ToArray();
            _inputLanguage = inputLanguage;
            _outputLanguage = outputLanguage;
        }

        public string? GetTranslate()
        {
            var textTranslationService = new TextTranslationService();

            string textFromImage = GetText();
            string? translateResult = textTranslationService.TranslateWithMM(textFromImage, _inputLanguage, _outputLanguage);

            return translateResult;
        }

        private string GetText()
        {
            var imageAnalyzerService = new ImageAnalyzerService(_inputLanguage);

            var result = imageAnalyzerService.GetTextFromImage(_image);

            return result.Item1;
        }
    }
}
