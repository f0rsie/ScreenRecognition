using Microsoft.Win32;
using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Core;
using ScreenRecognition.Desktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.CodeDom;
using System.Xml.Linq;
using HandyControl.Interactivity;
using System.Windows.Input;
using ScreenRecognition.Desktop.Resources.Styles.MessageResult;
using ScreenRecognition.Desktop.Models;
using GlobalHotKeys;
using ScreenRecognition.ImagePreparation.Services;
using ScreenRecognition.Modules.Modules;
using ScreenRecognition.Desktop.Models.SingletonModels;
using ScreenRecognition.Desktop.Models.ResultModels.ApiResultModels;

namespace ScreenRecognition.Desktop.ViewModel.WindowViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public PropShieldModel<ServerStatus> ServerStatusCustom { get; set; } = new();

        private Visibility _loginPanelVisibility;

        public Visibility LoginPanelVisibility
        {
            get => _loginPanelVisibility;
            set
            {
                _loginPanelVisibility = value;
                OnPropertyChanged(nameof(LoginPanelVisibility));
            }
        }

        public string? CurrentLogin
        {
            get => ConnectedUserSingleton.Login;
            set
            {
                ConnectedUserSingleton.Login = value;
                OnPropertyChanged(nameof(CurrentLogin));
            }
        }

        private RegisterGlobalHotkey _registerGlobalHotkey;

        private Page? _currentPage;

        private double _startX = 0;
        private double _startY = 0;

        private string? _result;
        private string _apiKey = "b70fae420f93dbe21880";
        private string _inputLanguage = "rus_eng";
        private string _outputLanguage = "eng";

        private string _translatorName = "MyMemoryTextTranslator";
        private string _ocrName = "TesseractOcr";

        private readonly UniversalController _controller;

        public Page? CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

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
            RegisterHotkey();

            if (ConnectedUserSingleton.ConnectionStatus == false)
            {
                LoginPanelVisibility = Visibility.Collapsed;
            }
            else
            {
                LoginPanelVisibility = Visibility.Visible;
            }

            _controller = new UniversalController();

            var page = new View.Pages.SettingsPage();
            CurrentPage = page;

            SeverStatusCheck();
        }

        public void RegisterHotkey()
        {
            // А это регистрация кнопки для хоткея
            var hotkeyKey = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().FirstOrDefault(e => e.ToString() == Properties.ProgramSettings.Default.HotkeyKey);
            var hotkeyModifier = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().FirstOrDefault(e => e.ToString() == Properties.ProgramSettings.Default.HotkeyModifier);

            RegisterGlobalHotkey.Dispose();
            _registerGlobalHotkey = new RegisterGlobalHotkey(hotkeyKey, hotkeyModifier, TakeScreenshot);
        }

        private async void SeverStatusCheck()
        {
            string statusText = "Не в сети";
            var taskResult = await Task.Run(async () =>
            {
                var result = await _controller.Get<bool, bool>("Server/StatusCheck");

                return result;
            });

            if (taskResult == true)
                statusText = "В сети";

            ServerStatusCustom.Property = new ServerStatus(statusText, taskResult);
        }

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

        public void NavigateToPage(object sender)
        {
            var pageName = (sender as Button)?.Name;

            if (ConnectedUserSingleton.ConnectionStatus == false && pageName != "Settings")
            {
                SignWindow();
                return;
            }

            var page = ProgramElementFinder.FindByName<Page?>($"{pageName}Page", Assembly.GetExecutingAssembly().FullName);
            CurrentPage = page;

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

                Result = result?.TranslatedTextVariants?.FirstOrDefault();

                if (Result == null || Result == string.Empty)
                    return;

                var resultWindow = new MessageResultWindow(Result, resultColor, f.Width, f.Height);
                resultWindow.Left = _startX;
                resultWindow.Top = _startY + 30;

                resultWindow.Show();
            }
            catch
            {
                Result = "Connection Error";
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