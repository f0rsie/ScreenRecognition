using ScreenRecognition.Api.Core.Services.OCRs;
using ScreenRecognition.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ScreenRecognition.Api.Core.Services
{
    public class TesseractOcrService : IOcrService
    {
        private string _languages;

        public TesseractOcrService(string inputLanguages)
        {
            _languages = inputLanguages;
        }

        public OcrResultModel GetText(byte[] image)
        {
            if(_languages == null)
            {
                _languages = "rus+eng";
            }
            else
            {
                _languages = _languages.Replace("_", "+");
            }

            var ocrEngine = new TesseractEngine(@"./Resources/Tessdata", _languages, EngineMode.LstmOnly);

            var img = Pix.LoadFromMemory(image);
            var res = ocrEngine.Process(img);
            var confidence = res.GetMeanConfidence();

            OcrResultModel result = new OcrResultModel
            {
                TextResult = res.GetText().Replace("\n", ""),
                Confidence = confidence
            };

            return result;
        }
    }
}
