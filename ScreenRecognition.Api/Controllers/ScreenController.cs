using Microsoft.AspNetCore.Mvc;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models.ResultsModels.ApiResultModels;
using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;

namespace ScreenRecognition.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        [Route("Translate")]
        [HttpPost]
        public async Task<ApiResultModel> TextTranslate(string translatorName, string ocrName, string translationApiKey, List<byte> image, string inputLanguage, string outputLanguage, string userLogin, string userPassword)
        {
            var detectedText = new OcrResultModel();
            var translatedText = new List<string>();

            try
            {
                var result = new ApiResultModel();
                var dbOps = new DBOperations();
                var textOps = new TextOperations();

                detectedText = await textOps.GetText(ocrName, image.ToArray(), inputLanguage);

                if (detectedText.Confidence == 0 && detectedText.TextResult.Contains("Не распознано"))
                    return new ApiResultModel { Error = true, ErrorMessage = "Ошибка распознавания", ErrorCode = "001" };

                translatedText = await textOps.GetTranslate(translatorName, detectedText.TextResult, inputLanguage, outputLanguage, translationApiKey);

                if (translatedText?.Where(e => e.Contains("JSON")).Count() > 0)
                    return new ApiResultModel { Error = true, ErrorMessage = "Ошибка перевода", ErrorCode = "002" };

                await dbOps.SaveHistory(translatorName, ocrName, image, inputLanguage, outputLanguage, userLogin, userPassword, detectedText, translatedText[0]);

                result = new ApiResultModel
                {
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    DetectedTextLanguage = inputLanguage,
                    Error = false,
                    OcrName = ocrName,
                    Image = image,
                    TranslatedTextLanguage = outputLanguage,
                    TranslatedTextVariants = translatedText,
                    TranslatorName = translatorName,
                };

                return result;
            }
            catch
            {
                return new ApiResultModel
                {
                    TranslatorName = translatorName,
                    OcrName = ocrName,
                    Image = image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    Error = true,
                    ErrorCode = "000",
                    ErrorMessage = "Ошибка",
                };
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