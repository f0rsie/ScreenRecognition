using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ScreenRecognition.Api.Core.Services.Translators
{
    public class MyMemoryTextTranslator : ITextTranslatorService
    {
        public async Task<string> Translate(string text, string inputLanguage, string outputLanguage, string apiKey)
        {
            string? res = "";
            string? translatorUrl = "";

            try
            {
                bool apiKeyValidation = ApiKeyValidation(apiKey);

                translatorUrl = $"https://api.mymemory.translated.net/get?q={text}&langpair={inputLanguage}|{outputLanguage}";

                HttpClient client = new HttpClient();

                if (apiKey != null && apiKeyValidation)
                {
                    translatorUrl += $"&key={apiKey}";
                }

                var stringTask = await client.GetStringAsync(translatorUrl);

                var result = JsonSerializer.Deserialize<Models.responseData>(stringTask);

                client.Dispose();

                res = result?.matches?.OrderByDescending(e => e.match.Value).First().translation;

                //res = result?.matches?[0].translation;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return $"{res}";
        }

        public bool ApiKeyValidation(string? apiKey)
        {
            try
            {
                HttpClient client = new HttpClient();

                var stringTask = client.GetStringAsync($"https://api.mymemory.translated.net/get?q=text&langpair=en|ru&key={apiKey}").Result;

                if (stringTask.Contains("AUTHENTICATION FAILURE"))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}