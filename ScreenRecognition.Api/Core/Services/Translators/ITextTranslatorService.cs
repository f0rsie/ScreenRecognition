namespace ScreenRecognition.Api.Core.Services.Translators
{
    public interface ITextTranslatorService
    {
        Task<List<string>> Translate(string text, string inputLanguage, string outputLanguage, string apiKey);
        Task<bool> ApiKeyValidation(string? apiKey);
    }
}
