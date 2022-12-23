using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class TestHistoryAndTranslatePageViewModel
    {
        #region Shields and Properties
        #region Shared
        private UniversalController _controller;
        #endregion
        #region Translate history
        #endregion
        #region Translate photo
        public PropShieldModel<PhotoTranslateOutputModel> Output { get; set; } = new();
        #endregion
        #endregion

        public TestHistoryAndTranslatePageViewModel()
        {
            _controller = new UniversalController("http://localhost:5046/api/");

            Output.Property = new();
        }

        public async void GetTranslate()
        {
            var img = ImageSourceToList(Output.Property.Image);

            var asyncTask = await Task.Run(async() =>
            {
                var settings = new DefaultProgramSettings();

                var result = await _controller.Post<List<byte>?, string>($"Screen/Translate?translatorName={settings.TranslatorName}&ocrName={settings.OcrName}&translationApiKey={settings.TranslationApiKey}&inputLanguage={settings.OcrLanguage}&outputLanguage={settings.TranslatorLanguage}", img);

                return result;
            });

            var result = asyncTask;

            Output.Property.OutputText = result;
            //Output.Reload();
        }

        public List<byte> ImageSourceToList(ImageSource imageSource)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            var result = data.ToList();

            return result;
        }
    }
}