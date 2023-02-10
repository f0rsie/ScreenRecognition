using ScreenRecognition.Api.Models.OcrResultModels;

namespace ScreenRecognition.Api.Core.Services.OCRs
{
    public interface IOcrService
    {
        OcrResultModel GetText(byte[] image, string inputLanguages);
    }
}
