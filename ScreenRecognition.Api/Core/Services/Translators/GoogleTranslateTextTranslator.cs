using AngleSharp.Html.Parser;
using Microsoft.IdentityModel.Tokens;
using ScreenRecognition.Api.Resources.Exceptions;

namespace ScreenRecognition.Api.Core.Services.Translators
{
    /// <summary>
    /// Не рабочий
    /// </summary>
    public class GoogleTranslateTextTranslator : ITextTranslatorService
    {
        public Task<bool> ApiKeyValidation(string? apiKey)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> Translate(string text, string inputLanguage, string outputLanguage, string apiKey)
        {
            var res = new List<string>();

            try
            {
                string? translatorUrl = $"https://translate.google.com/?sl=auto&tl=ru&text={text.Replace("\n", "")}";
                var client = new HttpClient();

                var result = await client.GetStringAsync(translatorUrl);

                using (var stream = new StreamWriter("C:\\Users\\assax\\OneDrive\\Рабочий стол\\site.html", false))
                {
                    await stream.WriteAsync(result);
                }

                var parser = new HtmlParser();
                var document = await parser.ParseDocumentAsync(result);

                var g = document.GetElementsByClassName("ryNqvb");

                var gg = document.QuerySelectorAll("span").ToList();

                foreach (var item in document.QuerySelectorAll("span"))
                {
                    if (item.ClassList.Contains("ryNqvb"))
                    {

                    }
                    //ryNqvb
                }

                if (res.IsNullOrEmpty())
                {
                    throw new TranslateException();
                }

                return res;
            }
            catch (Exception ex)
            {
                return new();
            }
        }
    }
}
