﻿using ScreenRecognition.Desktop.Command;
using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Core;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.Models.InputModels;
using ScreenRecognition.Desktop.Models.ResultModels.ApiResultModels;
using ScreenRecognition.Desktop.Models.SingletonModels;
using ScreenRecognition.Desktop.Resources.Styles.MessageResult;
using ScreenRecognition.Desktop.View.Pages;
using ScreenRecognition.Desktop.View.Windows;
using ScreenRecognition.Modules.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel.WindowViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // ApiKey (MyMemoryTextApi) b70fae420f93dbe21880
        #region Sheilds and Properties
        #region Commands
        public ICommand? ShowSignWindow { get; private set; }
        public ICommand? NavigateTo { get; private set; }

        #endregion
        #region Shields
        private readonly UniversalController _controller;

        private Timer _serverStatusTimer;

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
            _controller = new();

            OnStartup();
        }

        // При старте
        private void OnStartup()
        {
            RegisterCommands();
            RegisterHotkey();

            if (ConnectedUserSingleton.ConnectionStatus == false)
            {
                LoginPanelVisibilityCustom.Property = Visibility.Collapsed;
            }
            else
            {
                LoginPanelVisibilityCustom.Property = Visibility.Visible;
            }

            var page = new SettingsPage();
            CurrentPageCustom.Property = page;

            ServerStatusCheck();
        }

        // Регистрация команд
        private void RegisterCommands()
        {
            ShowSignWindow = new DelegateCommand(SignWindow);
            NavigateTo = new DelegateCommand(NavigateToPage);
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
        private void ServerStatusCheck()
        {
            TimerServerStatusCheckerTick(null, null);

            _serverStatusTimer?.Dispose();
            _serverStatusTimer = new Timer(5000);
            _serverStatusTimer.Elapsed += TimerServerStatusCheckerTick;

            _serverStatusTimer.Start();
        }

        private async void TimerServerStatusCheckerTick(object? sender, ElapsedEventArgs e)
        {
            ServerStatusCustom.Property = await ServerStatusChecker.CheckStatus();
        }

        // Вызов окна авторизации
        public void SignWindow(object? obj = null)
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

            if (string.IsNullOrEmpty(pageName))
            {
                pageName = sender.ToString();
            }

            if (ConnectedUserSingleton.ConnectionStatus == false && pageName != "Settings")
            {
                SignWindow();
                return;
            }

            var page = ProgramElementFinder.FindByName<Page?>($"{pageName}Page", Assembly.GetExecutingAssembly().FullName);
            CurrentPageCustom.Property = page;

            GC.Collect();
        }

        // Сделать скриншот
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

        // Перевод ImageSource в Bitmap
        private async Task StartAsync(ImageSource? img)
        {
            if (img != null)
            {
                var f = ConvertBitmapSourceToBitmap((BitmapSource)img);

                await GetResultAsyncV2(f);
            }
        }

        // New
        // Получение результата из Api V2
        private async Task GetResultAsyncV2(Bitmap f)
        {
            var array = BitmapToByte(f, ImageFormat.Png);
            var str = array.ToList();

            try
            {
                var resultColor = Properties.ProgramSettings.Default.ResultColor;

                var apiInputModel = new ApiInputModel
                {
                    TranslatorName = Properties.ProgramSettings.Default.TranslatorName,
                    OcrName = Properties.ProgramSettings.Default.OcrName,
                    TranslationApiKey = Properties.ProgramSettings.Default.TranslatorApiKey,
                    InputLanguage = Properties.ProgramSettings.Default.OcrLanguages,
                    OutputLanguage = Properties.ProgramSettings.Default.TranslatorLanguage,
                    User = ConnectedUserSingleton.User,
                    Image = str,
                };

                var result = await _controller.Post<ApiInputModel, ApiResultModel>("Screen/TranslateV2", apiInputModel);

                ResultCustom.Property = $"{result?.DetectedText}:::{result?.TranslatedTextVariants?.FirstOrDefault()}";

                if (result?.Error == true || result == null)
                    return;

                var resultWindow = new MessageResultWindow(result?.DetectedText, result?.TranslatedTextVariants?.FirstOrDefault(), result?.DetectedTextLanguage, result?.TranslatedTextLanguage, resultColor, f.Width, f.Height);
                resultWindow.Left = _startX;
                resultWindow.Top = _startY + 30;

                resultWindow.Show();
            }
            catch
            {
                ResultCustom.Property = "Connection Error";
            }
        }

        // Old
        // Получение результата из Api
        //private async Task GetResultAsync(Bitmap f)
        //{
        //    //await _controller.Post<ApiInputModel, ApiResultModel>($"Screen/Test", new ApiInputModel());

        //    var array = ImagePreparationService.BitmapToByte(f, ImageFormat.Png);
        //    var str = array.ToList();

        //    try
        //    {
        //        var translatorName = Properties.ProgramSettings.Default.TranslatorName;
        //        var ocrName = Properties.ProgramSettings.Default.OcrName;
        //        var apiKey = Properties.ProgramSettings.Default.TranslatorApiKey;
        //        var inputLanguage = Properties.ProgramSettings.Default.OcrLanguages;
        //        var outputLanguage = Properties.ProgramSettings.Default.TranslatorLanguage;
        //        var resultColor = Properties.ProgramSettings.Default.ResultColor;
        //        var userLogin = ConnectedUserSingleton.Login;
        //        var userPassword = ConnectedUserSingleton.Password;

        //        if (string.IsNullOrEmpty(userLogin) || userLogin == "Авторизация")
        //            userLogin = "guest";
        //        if (string.IsNullOrEmpty(userPassword))
        //            userPassword = "guest";
        //        if (string.IsNullOrEmpty(apiKey))
        //            apiKey = "123";

        //        string query = $"Screen/Translate?translatorName={translatorName}&ocrName={ocrName}&translationApiKey={apiKey}&inputLanguage={inputLanguage}&outputLanguage={outputLanguage}&userLogin={userLogin}&userPassword={userPassword}";

        //        var result = await _controller.Post<List<byte>?, ApiResultModel>(query, str);

        //        ResultCustom.Property = result?.TranslatedTextVariants?.FirstOrDefault();

        //        if ((string.IsNullOrEmpty(ResultCustom.Property) || string.IsNullOrWhiteSpace(ResultCustom.Property)) && result?.Error == true)
        //            return;

        //        var resultWindow = new MessageResultWindow(ResultCustom.Property, resultColor, f.Width, f.Height);
        //        resultWindow.Left = _startX;
        //        resultWindow.Top = _startY + 30;

        //        resultWindow.Show();
        //    }
        //    catch
        //    {
        //        ResultCustom.Property = "Connection Error";
        //    }
        //}

        // Конвертер (Image/Bitmap)Source в Bitmap
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

        // Конвертер Bitmap в Byte array
        public static byte[] BitmapToByte(Bitmap bitmap, ImageFormat? imageFormat = null)
        {
            imageFormat ??= ImageFormat.Bmp;

            var sampleStream = new MemoryStream();
            bitmap.Save(sampleStream, imageFormat);

            return sampleStream.ToArray();
        }

        // Конвертер Byte array в Bitmap
        public static Bitmap ByteToBitmap(byte[] image)
        {
            var result = new Bitmap(new MemoryStream(image));

            return result;
        }
    }
}