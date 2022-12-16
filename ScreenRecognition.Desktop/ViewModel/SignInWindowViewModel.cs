using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.View.Pages;
using ScreenRecognition.Desktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class SignInWindowViewModel : BaseViewModel
    {
        private readonly UniversalController _controller;

        private bool? _enabledWindowStatus;

        private User? _user;

        private string? _login;
        private string? _password;

        public bool? EnabledWindowStatus
        {
            get => _enabledWindowStatus;
            set
            {
                _enabledWindowStatus = value;
                OnPropertyChanged(nameof(EnabledWindowStatus));
            }
        }

        public string? Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SignInWindowViewModel()
        {
            _controller = new UniversalController("http://localhost:5046/api/");
        }

        public async void SignIn()
        {
            EnabledWindowStatus = false;

            _user = await _controller.Get<string, User?>($"User/Auth?login={Login}&password={Password}");

            if (_user != null)
            {
                ConnectedUserSingleton.Login = Login;
                ConnectedUserSingleton.Password = Password;
                ConnectedUserSingleton.ConnectionStatus = true;

                App.Current.Windows[2].Close();
            }

            HandyControl.Controls.MessageBox.Show("Аккаунт не найден", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);

            EnabledWindowStatus = true;
        }

        public void SignUp()
        {
            var window = new SignUpWindow();

            if(window.ShowDialog() == false)
            {

            }
        }
    }
}
