using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Models.ResultModels.ApiResultModels
{
    public class ApiResultModel
    {
        public string? DetectedText { get; set; }
        public string? OcrName { get; set; }
        public double? DetectedTextConfidence { get; set; }

        public List<string>? TranslatedTextVariants { get; set; }
        public string? TranslatorName { get; set; }

        public string? DetectedTextLanguage { get; set; }
        public string? TranslatedTextLanguage { get; set; }

        public List<byte>? Image { get; set; }

        public bool? Error { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
    }
}
