using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models.DbModels;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Core
{
    public class SettingsProfile
    {
        private Setting _settings;
        private UniversalController _controller;

        public SettingsProfile()
        {
            _settings = new Setting();
            _controller = new UniversalController();
        }

        private async Task SaveSettingsAsync()
        {

        }

        private async Task GetSettingsAsync()
        {
            _settings = await _controller.Get<Setting, Setting>("");
        }

        public async void GetSettings() => await GetSettingsAsync();

        public async void SaveSettings() => await SaveSettingsAsync();
    }
}
