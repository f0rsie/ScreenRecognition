using ScreenRecognition.Desktop.Models.DbModels;
using System.Windows.Media;

namespace ScreenRecognition.Desktop.Models.OutputModels
{
    public class HistoryOutputModel
    {
        public int Id { get; set; }

        public ImageSource Screenshot { get; set; }

        public string? RecognizedText { get; set; }

        public double? RecognizedTextAccuracy { get; set; }

        public string? Translate { get; set; }

        public virtual Language? InputLanguage { get; set; }

        public virtual Language? OutputLanguage { get; set; }

        public virtual Ocr? SelectedOcr { get; set; }

        public virtual Translator? SelectedTranslator { get; set; }

        public virtual User User { get; set; } = null!;


        public static implicit operator HistoryOutputModel(History history) => s_fromSource(history);

        private static HistoryOutputModel s_fromSource(History history)
        {
            var imagePreparation = new Core.ImagePreparation();

            var screenshot = imagePreparation.ByteToImage(history.Screenshot);

            return new HistoryOutputModel
            {
                Id = history.Id,
                Screenshot = screenshot,
                RecognizedText = history.RecognizedText,
                RecognizedTextAccuracy = history.RecognizedTextAccuracy,
                Translate = history.Translate,
                InputLanguage = history.InputLanguage,
                OutputLanguage = history.OutputLanguage,
                SelectedOcr = history.SelectedOcr,
                SelectedTranslator = history.SelectedTranslator,
                User = history.User,
            };
        }
    }
}