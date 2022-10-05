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
        private string _language;

        public TesseractOcrService(string language)
        {
            _language = language;
        }

        public string GetText(byte[] image)
        {
            var ocrEngine = new TesseractEngine(@"./Resources/Tessdata", _language, EngineMode.Default);

            var img = Pix.LoadFromMemory(image);
            var res = ocrEngine.Process(img);

            return res.GetText();
        }
    }
}
