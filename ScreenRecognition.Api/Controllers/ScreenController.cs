using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Mime;
using ScreenRecognition.Api.Core;
using ScreenRecognition.Api.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ScreenRecognition.Api.Controllers
{
    /// <summary>
    /// Сюда из клиент-программы будет передаваться скриншот части экрана для перевода и обнаружения того, что изображено.
    /// Тип принимаемых данных у TextTranslate будет либо массив байтов, либо битмап.
    /// Если массив байтов, то в json из клиент-программы нужно будет передавать переделанное в byte[] формат, с помощью BitmapToByte в Core библиотеке (пока её там нет, смотреть файл TextOcrService)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        [Route("Translate")]
        [HttpPost]
        public async Task<string> TextTranslate(string translatorName, string ocrName, string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage)
        {
            var asyncTask = await Task.Run(async () =>
            {
                var textOps = new TextOperations();

                var inputText = textOps.GetText(ocrName, image.ToArray(), inputLanguage);

                string result = await textOps.GetTranslate(translatorName, inputText, inputLanguage, outputLanguage, translationApiKey);

                return result;
            });

            return asyncTask;
        }

        [Route("ApiKeyValidation")]
        [HttpGet]
        public bool ApiKeyValidation(string apiKey)
        {
            var api = new Core.Services.Translators.MyMemoryTextTranslator();
            
            var result = api.ApiKeyValidation(apiKey);

            return result;
        }
    }
}