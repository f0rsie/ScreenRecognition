using Microsoft.Win32;
using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Core;
using ScreenRecognition.Desktop.View.Windows;
using ScreenRecognition.ImagePreparation.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        RegisterGlobalHotkey _registerGlobalHotkey;

        private string? _result;
        private string _apiKey = "123";
        private string _inputLanguage = "rus";
        private string _outputLanguage = "eng";
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
            // А это регистрация кнопки для хоткея
            _registerGlobalHotkey = new RegisterGlobalHotkey(GlobalHotKeys.Native.Types.VirtualKeyCode.KEY_Q, GlobalHotKeys.Native.Types.Modifiers.Control, TakeScreenshot);

            _controller = new UniversalController("http://localhost:5046/api/");
            Result = "313123";
        }

        public void TakeScreenshot()
        {
            var screenshot = ScreenCapture.CaptureDesktop();

            screenshot.Save("test.png", ImageFormat.Png);

            var window = new ScreenshotWindow(screenshot);
            if (window.ShowDialog() == false)
            {
                var img = window.Image;
                Start(img);
            }
        }

        public async void Start(ImageSource? img)
        {
            await StartAsync(img);
        }

        private async Task StartAsync(ImageSource? img)
        {
            if (img == null)
            {
                var ofd = new OpenFileDialog();

                if (ofd.ShowDialog() == true)
                {
                    var f = new Bitmap(ofd.FileName);

                    await GetResultAsync(f);
                }
            }
            else if (img != null)
            {
                var f = ConvertBitmapSourceToBitmap((BitmapSource)img);

                f.Save("tested.png", ImageFormat.Png);
                await GetResultAsync(f);
            }
        }

        private async Task GetResultAsync(Bitmap f)
        {
            var array = ImagePreparationService.BitmapToByte(f);
            var str = array.ToList();

            try
            {
                var result = await _controller.Post($"Screen/Translate?translationApiKey={_apiKey}&inputLanguage={_inputLanguage}&outputLanguage={_outputLanguage}", str);

                Result = result.ToString();
            }
            catch
            {
                Result = "Connection Error";
            }
        }

        public Bitmap ConvertBitmapSourceToBitmap(BitmapSource bitmapSource)
        {
            var width = bitmapSource.PixelWidth;
            var height = bitmapSource.PixelHeight;
            var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, memoryBlockPointer);
            return bitmap;
        }
    }
}