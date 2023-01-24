using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ScreenRecognition.Api.Core.Services.Translators
{
    public class MyMemoryTextTranslator : ITextTranslatorService
    {
        public async Task<string> Translate(string text, string inputLanguage, string outputLanguage, string? apiKey = null)
        {
            string? res = "";
            string? translatorUrl = "";

            string inputLangAlias = LangParse(inputLanguage);
            string outputLangAlias = LangParse(outputLanguage);

            bool apiKeyValidation = ApiKeyValidation(apiKey);

            try
            {
                translatorUrl = $"https://api.mymemory.translated.net/get?q={text}&langpair={inputLangAlias}|{outputLangAlias}";

                HttpClient client = new HttpClient();

                if (apiKey != null && apiKeyValidation)
                {
                    translatorUrl += $"&key={apiKey}";
                }

                var stringTask = await client.GetStringAsync(translatorUrl);

                var result = JsonSerializer.Deserialize<Models.responseData>(stringTask);

                res = result?.matches?.OrderByDescending(e => e.match.Value).First().translation;

                //res = result?.matches?[0].translation;

                client.Dispose();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return $"{res}";
        }

        public bool ApiKeyValidation(string? apiKey)
        {
            HttpClient client = new HttpClient();

            var stringTask = client.GetStringAsync($"https://api.mymemory.translated.net/get?q=text&langpair=en|ru&key={apiKey}").Result;

            if (stringTask.Contains("AUTHENTICATION FAILURE"))
                return false;

            return true;
        }

        private string LangParse(string language)
        {
            var result = language.Replace("rus", "ru").Replace("eng", "en").Split("_")[0];

            return result;
        }
    }
}