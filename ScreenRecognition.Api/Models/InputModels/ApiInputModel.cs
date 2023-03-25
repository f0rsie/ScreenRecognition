using ScreenRecognition.Api.Models.DbModels;

namespace ScreenRecognition.Api.Models.InputModels
{
    public class ApiInputModel
    {
        public string? TranslatorName { get; set; }
        public string? OcrName { get; set; }
        public string? TranslationApiKey { get; set; }
        public string? InputLanguage { get; set; }
        public string? OutputLanguage { get; set; }
        public User? User { get; set; }
        public List<byte>? Image { get; set; }
    }
}
