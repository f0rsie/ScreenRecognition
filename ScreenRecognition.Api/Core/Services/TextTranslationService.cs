using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ScreenRecognition.Api.Core.Services
{
    public class TextTranslationService
    {
        public string Translate(string text, string inputLanguage, string outputLanguage)
        {
            TranslationServiceClient client = TranslationServiceClient.Create();
            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents = { text },
                TargetLanguageCode = inputLanguage,
                Model = "",
            };
            TranslateTextResponse response = client.TranslateText(request);
            Translation translation = response.Translations[0];

            return translation.TranslatedText;
        }

        public string? TranslateWithMM(string text, string inputLanguage, string outputLanguage)
        {
            string? res = "";

            try
            {
                string key = "b70fae420f93dbe21880";
                //string path = $"get?q={text}&langpair=en|ru&key={key}";

                HttpClient client = new HttpClient();

                //client.BaseAddress = new Uri($"https://api.mymemory.translated.net/");
                var stringTask = client.GetStringAsync($"https://api.mymemory.translated.net/get?q={text}&langpair=ru|en&key={key}").Result;

                var result = JsonSerializer.Deserialize<Models.responseData>(stringTask);

                res = result?.matches?[0].translation;

                client.Dispose();
            }
            catch (Exception ex)
            {

            }

            return $"Original: {text} Translate: {res}";
        }
    }
}
