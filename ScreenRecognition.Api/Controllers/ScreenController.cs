using Microsoft.AspNetCore.Mvc;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models.ResultsModels.ApiResultModels;
using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;
using ScreenRecognition.Api.Resources.Exceptions;

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

            var result = new ApiResultModel();
            var dbOps = new DBOperations();
            var textOps = new TextOperations();

            try
            {
                detectedText = await textOps.GetText(ocrName, image.ToArray(), inputLanguage);

                translatedText = await textOps.GetTranslate(translatorName, detectedText.TextResult, inputLanguage, outputLanguage, translationApiKey);

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
            #region Обработчики исключений
            catch (RecognitionException ex)
            {
                return new ApiResultModel
                {
                    TranslatorName = translatorName,
                    OcrName = ocrName,
                    Image = image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    TranslatedTextLanguage = outputLanguage,
                    TranslatedTextVariants = translatedText,
                    Error = true,
                    ErrorCode = "001",
                    ErrorMessage = "Ошибка распознавания",
                };
            }
            catch (TranslateException ex)
            {
                return new ApiResultModel
                {
                    TranslatorName = translatorName,
                    OcrName = ocrName,
                    Image = image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    TranslatedTextLanguage = outputLanguage,
                    TranslatedTextVariants = translatedText,
                    Error = true,
                    ErrorCode = "002",
                    ErrorMessage = "Ошибка перевода",
                };
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
                    TranslatedTextLanguage = outputLanguage,
                    TranslatedTextVariants = translatedText,
                    Error = true,
                    ErrorCode = "000",
                    ErrorMessage = "Ошибка",
                };
            }
            #endregion
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