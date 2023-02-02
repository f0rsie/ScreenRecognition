using ScreenRecognition.Desktop.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Core
{
    public class SettingsProfile
    {
        private Models.Setting _settings;
        private UniversalController _controller;

        public SettingsProfile()
        {
            _settings = new Models.Setting();
            _controller = new UniversalController();
        }

        private async Task SaveSettingsAsync()
        {

        }

        private async Task GetSettingsAsync()
        {
            _settings = await _controller.Get<Models.Setting, Models.Setting>("");
        }

        public async void GetSettings() => await GetSettingsAsync();

        public async void SaveSettings() => await SaveSettingsAsync();
    }
}
