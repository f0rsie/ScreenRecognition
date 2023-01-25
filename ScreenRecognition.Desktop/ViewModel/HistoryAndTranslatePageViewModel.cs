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
    public class HistoryAndTranslatePageViewModel
    {
        #region Shields and Properties
        #region Shared
        private UniversalController _controller;
        #endregion
        #region Translation history
        public PropShieldModel<List<HistoryOutputModel>> HistoryList { get; set; } = new();
        public PropShieldModel<List<History>> HistoryListOld { get; set; } = new();
        #endregion
        #region Translate photo
        public PropShieldModel<PhotoTranslateOutputModel> Output { get; set; } = new();
        #endregion
        #endregion

        public HistoryAndTranslatePageViewModel()
        {
            _controller = new UniversalController("http://localhost:5046/api/");

            GetTranslationHistory();
        }

        private async void GetTranslationHistory()
        {
            await Task.Run(async () =>
            {
                HistoryListOld.Property = await _controller.Get<List<History>, List<History>>($"User/TranslationHistory?userId={ConnectedUserSingleton.User.Id}");
            });

            HistoryList.Property = new();

            foreach (var item in HistoryListOld.Property)
            {
                HistoryList.Property.Add(item);
            }
        }

        public async void GetTranslate(ImageSource img)
        {
            Output.Property = new();

            var byteImg = ImageSourceToList(img);

            var asyncTask = await Task.Run(async() =>
            {
                var settings = new DefaultProgramSettings();

                var result = await _controller.Post<List<byte>?, string>($"Screen/TranslateWithOrig?translatorName={settings.TranslatorName}&ocrName={settings.OcrName}&translationApiKey={settings.TranslationApiKey}&inputLanguage={settings.OcrLanguage}&outputLanguage={settings.TranslatorLanguage}", byteImg);

                return result;
            });

            var result = asyncTask.Split(":::");

            Output.Property.InputText = result[0];
            Output.Property.OutputText = result[1];
            Output.Property = Output.Property;
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