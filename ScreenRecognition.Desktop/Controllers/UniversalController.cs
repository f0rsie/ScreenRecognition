using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Net.Http.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Reflection.Metadata;

namespace ScreenRecognition.Desktop.Controllers
{
    public class UniversalController
    {
        private readonly string _webPath = "http://localhost:80/api/";

        public UniversalController(string webPath = "")
        {
            if (!string.IsNullOrEmpty(webPath))
            {
                _webPath = webPath;
            }
        }

        public async Task<P?> Get<T, P>(string path)
        {
            var result = await ControllerOperations<T, P>("get", path);

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

            client.BaseAddress = new Uri($"{_webPath}");
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
