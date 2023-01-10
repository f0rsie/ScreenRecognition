﻿using ScreenRecognition.Desktop.Models;
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
        private User user
        {
            get => User;
            set => User = value;
        }

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

        private bool? connectionStatus
        {
            get => ConnectionStatus;
            set => ConnectionStatus = value;
        }

        public static User User { get; set; }

        public static string? Login { get; set; } = "Авторизация";
        public static string? Password { get; set; } = null;

        public static bool? ConnectionStatus { get; set; } = false;


        public static implicit operator ConnectedUserSingleton(User user) => s_fromSource(user);

        private static ConnectedUserSingleton s_fromSource(User user)
        {
            return new ConnectedUserSingleton
            {
                user = user,

                login = user.Login,
                password = user.Password,

                connectionStatus = true,
            };
        }

        public static void Disconnect()
        {
            ConnectionStatus = false;
            Login = "Авторизация";
            Password = null;
        }
    }
}
