using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Models
{
    public class ConnectedUserSingleton
    {
        private string? login
        {
            get => Login;
            set => Login = value;
        }
        private string? password
        {
            get => Password;
            set => Password = value;
        }

        public static string? Login { get; set; } = "Guest";
        public static string? Password { get; set; } = null;


        public static implicit operator ConnectedUserSingleton(User user) => s_fromSource(user);

        private static ConnectedUserSingleton s_fromSource(User user)
        {
            return new ConnectedUserSingleton
            {
                login = user.Login,
                password = user.Password,
            };
        }
    }
}
