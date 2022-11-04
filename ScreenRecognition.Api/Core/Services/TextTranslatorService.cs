namespace ScreenRecognition.Api.Core.Services
{
    public class TextTranslatorService
    {
        private readonly string _translationApiKey;
        private readonly byte[] _image;
        private readonly string _language;

        //Thread? _thread;

        public TextTranslatorService(string translationApiKey, List<byte> image, string language)
        {
            _translationApiKey = translationApiKey;
            _image = image.ToArray();
            _language = language.Replace("auto", "eng");
        }

        public string GetTranslate()
        {
            var textTranslationService = new TextTranslationService();

            string textFromImage = GetText();
            string translateResult = textTranslationService.TranslateWithMM(textFromImage, "en-EN");

            return translateResult;
        }

        private string GetText()
        {
            var imageAnalyzerService = new ImageAnalyzerService(_language);

            var result = imageAnalyzerService.GetTextFromImage(_image);

            return result.Item1;
        }
    }
}
