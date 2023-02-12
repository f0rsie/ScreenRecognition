using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Core;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.Models.ResultModels.ApiResultModels;
using ScreenRecognition.Desktop.Models.SingletonModels;
using ScreenRecognition.Desktop.Resources.Styles.MessageResult;
using ScreenRecognition.Desktop.View.Pages;
using ScreenRecognition.Desktop.View.Windows;
using ScreenRecognition.ImagePreparation.Services;
using ScreenRecognition.Modules.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel.WindowViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // ApiKey (MyMemoryTextApi) b70fae420f93dbe21880
        #region Sheilds and Properties
        #region Shields
        private readonly UniversalController _controller;

        private RegisterGlobalHotkey? _registerGlobalHotkey;

        private double _startX = 0;
        private double _startY = 0;

        private string? _result;
        #endregion
        #region Properties
        public PropShieldModel<ServerStatus> ServerStatusCustom { get; set; } = new();
        public PropShieldModel<Visibility> LoginPanelVisibilityCustom { get; set; } = new();
        public PropShieldModel<Page?> CurrentPageCustom { get; set; } = new();

        public PropShieldModel<string?> ResultCustom { get; set; } = new();
             
        public string? CurrentLogin
        {
            get => ConnectedUserSingleton.Login;
            set
            {
                ConnectedUserSingleton.Login = value;
                OnPropertyChanged(nameof(CurrentLogin));
            }
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            RegisterHotkey();

            if (ConnectedUserSingleton.ConnectionStatus == false)
            {
                LoginPanelVisibilityCustom.Property = Visibility.Collapsed;
            }
            else
            {
                LoginPanelVisibilityCustom.Property = Visibility.Visible;
            }

            _controller = new UniversalController();

            var page = new SettingsPage();
            CurrentPageCustom.Property = page;

            SeverStatusCheck();
        }

        // Регистрация комбинации клавиш
        public void RegisterHotkey()
        {
            // А это регистрация кнопки для хоткея
            var hotkeyKey = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().FirstOrDefault(e => e.ToString() == Properties.ProgramSettings.Default.HotkeyKey);
            var hotkeyModifier = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().FirstOrDefault(e => e.ToString() == Properties.ProgramSettings.Default.HotkeyModifier);

            RegisterGlobalHotkey.Dispose();
            _registerGlobalHotkey = new RegisterGlobalHotkey(hotkeyKey, hotkeyModifier, TakeScreenshot);
        }

        // Проверка статуса работы сервера
        private async void SeverStatusCheck()
        {
            string statusText = "Не в сети";
            var taskResult = await Task.Run(async () =>
            {
                var client = new HttpClient();
                var data = new StringContent("123", Encoding.UTF8, "application/json");
                client.Timeout = new TimeSpan(0, 0, 0, 0, 500);

                try
                {
                    await client.PostAsync("http://localhost:5046/api/", data);
                    return true;
                }
                catch
                {
                    return false;
                }

            });

            if (taskResult == true)
                statusText = "В сети";

            ServerStatusCustom.Property = new ServerStatus(statusText, taskResult);
        }

        // Вызов окна авторизации
        public void SignWindow()
        {
            if (ConnectedUserSingleton.ConnectionStatus == true)
            {
                if (HandyControl.Controls.MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ConnectedUserSingleton.Disconnect();
                    MainWindowManager.Set(new MainWindow());
                }

                return;
            }

            var window = new SignInWindow();

            if (window.ShowDialog() == false)
            {
                MainWindowManager.Set(new MainWindow());
            }
        }

        // Метод по смене текущей страницы в окне
        public void NavigateToPage(object sender)
        {
            var pageName = (sender as Button)?.Name;

            if (ConnectedUserSingleton.ConnectionStatus == false && pageName != "Settings")
            {
                SignWindow();
                return;
            }

            var page = ProgramElementFinder.FindByName<Page?>($"{pageName}Page", Assembly.GetExecutingAssembly().FullName);
            CurrentPageCustom.Property = page;

            GC.Collect();
        }

        private async void TakeScreenshot()
        {
            var screenshot = ScreenCapture.CaptureDesktop();

            var window = new ScreenshotWindow(screenshot);
            if (window.ShowDialog() == false)
            {
                _startX = window._startX;
                _startY = window._startY;

                if (window.Image != null)
                {
                    var img = window.Image;
                    await StartAsync(img);
                }
            }
        }

        private async Task StartAsync(ImageSource? img)
        {
            if (img != null)
            {
                var f = ConvertBitmapSourceToBitmap((BitmapSource)img);

                await GetResultAsync(f);
            }
        }

        private async Task GetResultAsync(Bitmap f)
        {
            var array = ImagePreparationService.BitmapToByte(f, ImageFormat.Png);
            var str = array.ToList();

            try
            {
                var translatorName = Properties.ProgramSettings.Default.TranslatorName;
                var ocrName = Properties.ProgramSettings.Default.OcrName;
                var apiKey = Properties.ProgramSettings.Default.TranslatorApiKey;
                var inputLanguage = Properties.ProgramSettings.Default.OcrLanguages;
                var outputLanguage = Properties.ProgramSettings.Default.TranslatorLanguage;
                var resultColor = Properties.ProgramSettings.Default.ResultColor;
                var userLogin = ConnectedUserSingleton.Login;
                var userPassword = ConnectedUserSingleton.Password;

                if (string.IsNullOrEmpty(userLogin) || userLogin == "Авторизация")
                    userLogin = "guest";
                if (string.IsNullOrEmpty(userPassword))
                    userPassword = "guest";
                if (string.IsNullOrEmpty(apiKey))
                    apiKey = "123";

                string query = $"Screen/Translate?translatorName={translatorName}&ocrName={ocrName}&translationApiKey={apiKey}&inputLanguage={inputLanguage}&outputLanguage={outputLanguage}&userLogin={userLogin}&userPassword={userPassword}";

                var result = await _controller.Post<List<byte>?, ApiResultModel>(query, str);

                ResultCustom.Property = result?.TranslatedTextVariants?.FirstOrDefault();

                if (ResultCustom.Property == null || ResultCustom.Property == string.Empty)
                    return;

                var resultWindow = new MessageResultWindow(ResultCustom.Property, resultColor, f.Width, f.Height);
                resultWindow.Left = _startX;
                resultWindow.Top = _startY + 30;

                if (!string.IsNullOrEmpty(ResultCustom.Property) && !string.IsNullOrWhiteSpace(ResultCustom.Property))
                {
                    resultWindow.Show();
                    return;
                }

                GC.Collect();
            }
            catch
            {
                ResultCustom.Property = "Connection Error";
            }
        }

        private Bitmap ConvertBitmapSourceToBitmap(BitmapSource bitmapSource)
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