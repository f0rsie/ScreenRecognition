using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Controllers
{
    public class UniversalController
    {
        public static string s_webPath = "http://localhost:5046/api/";

        public UniversalController(string webPath = "")
        {
            if (!string.IsNullOrEmpty(webPath))
            {
                s_webPath = webPath;
            }
        }

        public async Task<T?> Get<T>(string path)
        {
            var result = await ControllerOperations<T, T>("get", path);

            return result;
        }

        public async Task<P?> Post<T, P>(string path, T? cl)
        {
            var result = await ControllerOperations<T, P>("post", path, cl);

            return result;
        }

        private async Task<P?> ControllerOperations<T, P>(string type, string path, T? cl = default(T))
        {
            HttpClient client = new HttpClient();
            P? result;

            client.BaseAddress = new Uri($"{s_webPath}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (type == "get")
                {
                    response = await client.GetAsync(path);
                }
                else if (type == "post")
                {
                    response = await client.PostAsJsonAsync($"{path}", cl);
                    response.EnsureSuccessStatusCode();
                }

                result = await response.Content.ReadFromJsonAsync<P>();
            }
            catch
            {
                return default(P);
            }

            return result;
        }
    }
}
