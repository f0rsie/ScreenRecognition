using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;
using Tesseract;

namespace ScreenRecognition.Api.Core.Services.OCRs
{
    public class TesseractOcr : IOcrService
    {
        public OcrResultModel GetText(byte[] image, string inputLanguages)
        {
            if (inputLanguages == null)
            {
                inputLanguages = "rus+eng";
            }
            else
            {
                inputLanguages = inputLanguages.Replace("_", "+");
            }

            var result = new OcrResultModel();

            try
            {
                var ocrEngine = new TesseractEngine(@"./Resources/Tessdata", inputLanguages, EngineMode.LstmOnly);

                var img = Pix.LoadFromMemory(image);
                var res = ocrEngine.Process(img);
                var confidence = res.GetMeanConfidence();

                result = new OcrResultModel
                {
                    TextResult = res.GetText().Replace("\n", ""),
                    Confidence = confidence
                };
            }
            catch { }

            return result;
        }
    }
}
