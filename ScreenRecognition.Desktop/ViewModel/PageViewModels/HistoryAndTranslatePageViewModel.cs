using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.Models.DbModels;
using ScreenRecognition.Desktop.Models.OutputModels;
using ScreenRecognition.Desktop.Models.ResultModels.ApiResultModels;
using ScreenRecognition.Desktop.Models.SingletonModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel.PageViewModels
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
            _controller = new UniversalController();

            GetTranslationHistory();
        }

        public async void ClearHistory()
        {
            await _controller.Post<User?, User?>($"User/ClearTranslateHistory", ConnectedUserSingleton.User);
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

            var asyncTask = await Task.Run(async () =>
            {
                var settings = new DefaultProgramSettings();

                var result = await _controller.Post<List<byte>, ApiResultModel>($"Screen/Translate?translatorName={settings.TranslatorName}&ocrName={settings.OcrName}&translationApiKey={settings.TranslationApiKey}&inputLanguage={settings.OcrLanguage}&outputLanguage={settings.TranslatorLanguage}&userLogin={ConnectedUserSingleton.Login}&userPassword={ConnectedUserSingleton.Password}", byteImg);

                return result;
            });

            Output.Property.InputText = asyncTask?.DetectedText;
            Output.Property.OutputText = asyncTask?.TranslatedTextVariants?.FirstOrDefault();
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