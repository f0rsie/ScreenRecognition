using Microsoft.AspNetCore.Mvc;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Core.Services.Translators;
using ScreenRecognition.Api.Models.InputModels;
using ScreenRecognition.Api.Models.ResultsModels.ApiResultModels;
using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;
using ScreenRecognition.Api.Resources.Exceptions;

namespace ScreenRecognition.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        // New
        [Route("TranslateV2")]
        [HttpPost]
        public async Task<ApiResultModel> TextTranslateV2(ApiInputModel apiInputModel)
        {
            var detectedText = new OcrResultModel();
            var translatedText = new List<string>();

            var result = new ApiResultModel();
            var dbOps = new DBOperations();
            var textOps = new TextOperations();

            try
            {
                detectedText = await textOps.GetText(apiInputModel.OcrName, apiInputModel.Image?.ToArray(), apiInputModel.InputLanguage);

                translatedText = await textOps.GetTranslate(apiInputModel.TranslatorName, detectedText.TextResult, apiInputModel.InputLanguage, apiInputModel.OutputLanguage, apiInputModel.TranslationApiKey);

                if (apiInputModel.User != null)
                    await dbOps.SaveHistory(apiInputModel.TranslatorName, apiInputModel.OcrName, apiInputModel.Image, apiInputModel.InputLanguage, apiInputModel.OutputLanguage, apiInputModel.User.Login, apiInputModel.User.Password, detectedText, translatedText[0]);

                result = new ApiResultModel
                {
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    DetectedTextLanguage = apiInputModel.InputLanguage,
                    Error = false,
                    OcrName = apiInputModel.OcrName,
                    Image = apiInputModel.Image,
                    TranslatedTextLanguage = apiInputModel.OutputLanguage,
                    TranslatedTextVariants = translatedText,
                    TranslatorName = apiInputModel.TranslatorName,
                };

                return result;
            }
            #region Обработчики исключений
            catch (RecognitionException ex)
            {
                return new ApiResultModel
                {
                    TranslatorName = apiInputModel.TranslatorName,
                    OcrName = apiInputModel.OcrName,
                    Image = apiInputModel.Image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    TranslatedTextLanguage = apiInputModel.OutputLanguage,
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
                    TranslatorName = apiInputModel.TranslatorName,
                    OcrName = apiInputModel.OcrName,
                    Image = apiInputModel.Image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    TranslatedTextLanguage = apiInputModel.OutputLanguage,
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
                    TranslatorName = apiInputModel.TranslatorName,
                    OcrName = apiInputModel.OcrName,
                    Image = apiInputModel.Image,
                    DetectedText = detectedText.TextResult,
                    DetectedTextConfidence = detectedText.Confidence,
                    TranslatedTextLanguage = apiInputModel.OutputLanguage,
                    TranslatedTextVariants = translatedText,
                    Error = true,
                    ErrorCode = "000",
                    ErrorMessage = "Ошибка",
                };
            }
            #endregion
        }

        // Old
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