using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Models
{
    public class TranslatorApiKeyOutputModel
    {
        public string? ApiKey { get; set; }
        public bool? ValidationStatus { get; private set; } 
        public Color ValidationColor { get; private set; } = Color.Black;

        public TranslatorApiKeyOutputModel(string apiKey, bool? validationStatus, Color? validationColor)
        {
            ApiKey = apiKey;
            ValidationStatus = validationStatus;

            if (validationColor != null)
                ValidationColor = (Color)validationColor;
        }
    }
}
