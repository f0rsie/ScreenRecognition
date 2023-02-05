namespace ScreenRecognition.Api.Core.Services.Translators
{
    public interface ITextTranslatorService
    {
        Task<string> Translate(string text, string inputLanguage, string outputLanguage, string apiKey);
        Task<bool> ApiKeyValidation(string? apiKey);
    }
}
