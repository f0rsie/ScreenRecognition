using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenRecognition.Desktop.ViewModel.WindowViewModels
{
    public class SignUpWindowViewModel
    {
        #region Shields and Properties
        #region Shared
        private readonly UniversalController _controller;
        #endregion
        #region Properties
        public PropShieldModel<string> Login { get; set; } = new();
        public PropShieldModel<string> Password { get; set; } = new();
        public PropShieldModel<string> Mail { get; set; } = new();

        public PropShieldModel<string?> FirstName { get; set; } = new();
        public PropShieldModel<string?> LastName { get; set; } = new();

        public PropShieldModel<List<Country?>> CountryList { get; set; } = new();
        public PropShieldModel<Country?> SelectedCountry { get; set; } = new();

        public PropShieldModel<DateTime?> BirthdayDate { get; set; } = new();
        #endregion
        #endregion

        public SignUpWindowViewModel()
        {
            _controller = new();

            DataLoader();
        }

        private async void DataLoader()
        {
            await Task.Run(async () =>
            {
                CountryList.Property = await _controller.Get<List<Country?>, List<Country?>>("Settings/Countries");
            });
        }

        public async void SignUp(Window window)
        {
            if (string.IsNullOrEmpty(Login.Property) || string.IsNullOrEmpty(Password.Property) || string.IsNullOrEmpty(Mail.Property))
            {
                HandyControl.Controls.MessageBox.Show("Введены не все данные", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (await MailCheck() == false)
            {
                HandyControl.Controls.MessageBox.Show("Данная почта уже зарегистрирована", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (await LoginCheck() == false)
            {
                HandyControl.Controls.MessageBox.Show("Данный логин уже зарегистрирована", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var user = new User
            {
                Login = Login.Property,
                Password = Password.Property,
                Email = Mail.Property,
                NickName = Login.Property,

                FirstName = FirstName.Property,
                LastName = LastName.Property,

                RoleId = 2,
                CountryId = CountryList.Property.FirstOrDefault(e => e?.Name == SelectedCountry.Property?.Name)?.Id,
                Birthday = BirthdayDate.Property,
            };

            await SignUpUser(user);

            HandyControl.Controls.MessageBox.Show("Аккаунт успешно зарегистрирован!", "Регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
            window.Close();
        }

        private async Task SignUpUser(User user)
        {
            await _controller.Post<User, User>($"User/Registration", user);
        }

        private async Task<bool> LoginCheck()
        {
            var result = await _controller.Get<bool, bool>($"User/LoginCheck?login={Login.Property}");

            return result;
        }

        private async Task<bool> MailCheck()
        {
            var result = await _controller.Get<bool, bool>($"User/MailCheck?mail={Mail.Property}");

            return result;
        }
    }
}
