using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Mime;
using ScreenRecognition.Api.Core;
using ScreenRecognition.Api.Core.Services;
using Microsoft.EntityFrameworkCore;

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
        public async Task<string?> TextTranslate(string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage)
        {
            var asyncTask = await Task.Run(() =>
            {
                var textTranslator = new TextTranslationService(translationApiKey, image, inputLanguage, outputLanguage);

                string? result = textTranslator.GetTranslate();

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