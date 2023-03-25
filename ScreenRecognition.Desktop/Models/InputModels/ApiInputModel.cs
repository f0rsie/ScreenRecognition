using ScreenRecognition.Desktop.Models.DbModels;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models.InputModels
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
