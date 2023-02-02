﻿using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenRecognition.Desktop.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            ProfilePasswordSetter();
        }

        private void ProfilePasswordSetter()
        {
            userPassword.Password = ConnectedUserSingleton.Password;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsPageViewModel).SaveSettings();
        }

        private void SaveAccountInfo_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsPageViewModel).UserCustom.Property.Password = userPassword.Password;
            (DataContext as SettingsPageViewModel).SaveAccountInfo();
        }
    }
}
