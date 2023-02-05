using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Mime;
using ScreenRecognition.Api.Core;
using ScreenRecognition.Api.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<string> TextTranslate(string translatorName, string ocrName, string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage, string userLogin, string userPassword, bool returnsOriginal)
        {
            try
            {
                string? result = "";
                var dbOps = new DBOperations();
                var textOps = new TextOperations();

                var inputText = await textOps.GetText(ocrName, image.ToArray(), inputLanguage);

                result = await textOps.GetTranslate(translatorName, inputText.TextResult, inputLanguage, outputLanguage, translationApiKey);

                if (result.Contains("JSON"))
                    return "Ошибка распознавания";

                await dbOps.SaveHistory(translatorName, ocrName, image, inputLanguage, outputLanguage, userLogin, userPassword, inputText, result);

                if (returnsOriginal)
                    result = $"{inputText.TextResult}:::{result}";

                return result;
            }
            catch
            {
                return "Ошибка перевода";
            }
        }

        [Route("ApiKeyValidation")]
        [HttpGet]
        public async Task<bool> ApiKeyValidation(string translatorName, string apiKey)
        {
            bool result = false;

            var translator = TextOperations.FindElement<ITextTranslatorService?>(translatorName);

            if (translator == null)
                return result;

            result = await translator.ApiKeyValidation(apiKey);

            return result;
        }
    }
}