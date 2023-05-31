using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Core
{
    public class ServerStatusChecker
    {
        public static async Task<ServerStatus> CheckStatus()
        {
            var client = new HttpClient();
            var data = new StringContent("123", Encoding.UTF8, "application/json");
            client.Timeout = new TimeSpan(0, 0, 0, 0, 500);

            try
            {
                await client.PostAsync(UniversalController.SWebPath, data);
                return new ServerStatus("В сети", true);
            }
            catch
            {
                return new ServerStatus("Не в сети", false);
            }
        }
    }
}