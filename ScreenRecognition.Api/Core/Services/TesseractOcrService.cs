using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ScreenRecognition.Api.Core.Services
{
    public class TesseractOcrService
    {
        private string _inputLanguage;

        public TesseractOcrService(string inputLanguage)
        {
            _inputLanguage = inputLanguage;
        }

        public (string, float) GetText(byte[] image)
        {
            var ocrEngine = new TesseractEngine(@"./Resources/Tessdata", "rus+eng", EngineMode.LstmOnly);

            var img = Pix.LoadFromMemory(image);
            var res = ocrEngine.Process(img);
            var confidence = res.GetMeanConfidence();

            var result = (res.GetText().Replace("\n", ""), confidence);

            return result;
        }
    }
}
