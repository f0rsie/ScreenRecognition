using Microsoft.Win32;
using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.ImagePreparation.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string? _result;
        private string _apiKey = "123";
        private string _language = "rus";
        UniversalController _controller;

        public string? Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public MainWindowViewModel()
        {
            _controller = new UniversalController("http://localhost:5046/api/");
            Result = "313123";
        }

        public async void Start()
        {
            await StartAsync();
        }

        private async Task StartAsync()
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                var f = new Bitmap(ofd.FileName);

                var array = ImagePreparationService.BitmapToByte(f);
                var str = array.ToList();

                var result = await _controller.Post($"Screen/Translate?translationApiKey={_apiKey}&language={_language}", str);

                Result = result.ToString();
            }
        }
    }
}