using System.Windows.Media;

namespace ScreenRecognition.Desktop.Models.OutputModels
{
    public class PhotoTranslateOutputModel
    {
        public ImageSource? Image { get; set; }
        public string? OcrLanguages { get; set; }
        public string? TranslateLanguage { get; set; }
        public string? InputText { get; set; }
        public string? OutputText { get; set; }
    }
}
