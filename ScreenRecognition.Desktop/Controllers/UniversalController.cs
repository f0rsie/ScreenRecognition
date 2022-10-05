using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Net.Http.Json;

namespace ScreenRecognition.Desktop.Controllers
{
    public class UniversalController
    {
        private readonly string _webPath = "http://localhost:80/api/";
        // Lastest version of Universal Controller

        public UniversalController(string webPath = "")
        {
            if (!string.IsNullOrEmpty(webPath))
            {
                _webPath = webPath;
            }
        }

        public async Task<string> Get<T>(string path)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{_webPath}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(path);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        //public async Task<T> Get<T>(string path)
        //{
        //    HttpClient client = new HttpClient();
        //    var stringTask = await client.GetStringAsync(_webPath + path);

        //    var result = JsonSerializer.Deserialize<T>(stringTask);

        //    client.Dispose();

        //    return result;
        //}

        public async Task<string> Post<T>(string path, T cl)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{_webPath}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync($"{path}", cl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        //public async Task<object> Post<T>(string path, T cl)
        //{
        //    var jsonObject = JsonConvert.SerializeObject(cl);
        //    var url = _webPath + path;
        //    HttpClient client = new HttpClient();

        //    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    //var result = await client.PostAsync(url, content);
        //    var result = await client.PostAsJsonAsync(url, content);

        //    //var parsedResult = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        //    var parsedResult = await content.ReadAsStringAsync();

        //    client.Dispose();

        //    return parsedResult;
        //}

        //public async Task<T> Put<T>(string path, T cl)
        //{
        //    var jsonObject = JsonSerializer.Serialize(cl);
        //    var url = _webPath + path;
        //    HttpClient client = new HttpClient();

        //    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //    var result = await client.PutAsync(url, content);

        //    var parsedResult = JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync());

        //    client.Dispose();

        //    return parsedResult;
        //}

        //public async Task PostTest<T>(string path, T cl)
        //{
        //    HttpClient client = new HttpClient();
        //    string url = _webPath + path;

        //    var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        //    options.Converters.Add(new JsonStringEnumConverter());

        //    var result = await client.PostAsJsonAsync<T>(url, cl, options);

        //    client.Dispose();
        //}
    }
}
