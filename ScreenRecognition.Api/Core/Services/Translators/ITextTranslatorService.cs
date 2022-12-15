namespace ScreenRecognition.Api.Core.Services.Translators
{
    public interface ITextTranslatorService
    {
        string Translate(string text, string inputLanguage, string outputLanguage, string? apiKey = null);
        bool ApiKeyValidation(string? apiKey);
    }
}
