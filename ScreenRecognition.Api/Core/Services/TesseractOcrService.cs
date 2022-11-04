﻿using System;
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

        public (string, float) GetText(byte[] image)
        {
            var ocrEngine = new TesseractEngine(@"./Resources/Tessdata", _language, EngineMode.LstmOnly);

            var img = Pix.LoadFromMemory(image);
            var res = ocrEngine.Process(img);
            var r = res.GetMeanConfidence();

            var result = (res.GetText(), r);

            return result;
        }
    }
}
