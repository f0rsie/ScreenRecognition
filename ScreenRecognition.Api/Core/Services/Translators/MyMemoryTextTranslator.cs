using System.Text.Json;
using ScreenRecognition.Api.Models.ResultsModels.TranslatorsJsonModels;

namespace ScreenRecognition.Api.Core.Services.Translators
{
    public class MyMemoryTextTranslator : ITextTranslatorService
    {
        // Поменял проверку валидности апи ключа при вызове этой функции, что выиграло от 100+ мс (было в среднем ~700, стало ~400, но зависит от размера переводимого текста)
        // Теперь если ключ валидный, то скорость работы данной функции быстрее в ~1.5 раза, по сравнению со старой проверкой
        public async Task<List<string>> Translate(string text, string inputLanguage, string outputLanguage, string apiKey)
        {
            var res = new List<string>();
            string? translatorUrl = "";

            try
            {
                //bool apiKeyValidation = await ApiKeyValidation(apiKey);

                translatorUrl = $"https://api.mymemory.translated.net/get?q={text}&langpair={inputLanguage}|{outputLanguage}";

                var client = new HttpClient();

                if (apiKey != null)
                {
                    translatorUrl += $"&key={apiKey}";
                }

                var stringTask = await client.GetStringAsync(translatorUrl);

                if (stringTask.Contains("AUTHENTICATION FAILURE"))
                {
                    translatorUrl = translatorUrl.Replace($"&key={apiKey}", "");
                    stringTask = await client.GetStringAsync(translatorUrl);
                }

                var result = JsonSerializer.Deserialize<responseData>(stringTask);

                client.Dispose();

                res = result.matches.Select(e => e.translation).ToList();
            }
            catch (Exception ex)
            {
                return new() { ex.Message };
            }

            return res;
        }

        public async Task<bool> ApiKeyValidation(string? apiKey)
        {
            try
            {
                HttpClient client = new HttpClient();

                var stringTask = await client.GetStringAsync($"https://api.mymemory.translated.net/get?q=text&langpair=en|ru&key={apiKey}");

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