namespace ScreenRecognition.Api.Core.Services.Translators
{
    public interface ITextTranslatorService
    {
        Task<string> Translate(string text, string inputLanguage, string outputLanguage, string apiKey);
        bool ApiKeyValidation(string? apiKey);
    }
}
