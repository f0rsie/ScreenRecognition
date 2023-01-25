using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScreenRecognition.Api.Models;

namespace ScreenRecognition.Api.Core.Services
{
    public class DBOperations
    {
        private Data.ScreenRecognitionContext _dbContext;

        public DBOperations()
        {
            _dbContext = new Data.ScreenRecognitionContext();
        }

        public async Task SaveHistory(string translatorName, string ocrName, List<byte> image, string inputLanguage, string outputLanguage, string userLogin, string userPassword, OcrResultModel recognizedText, string translatedText)
        {
            if (userLogin == "guest" || userPassword == "guest")
                return;

            var userId = (await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == userLogin)).Id;
            var history = new History
            {
                Id = (await _dbContext.Histories.Where(e => e.UserId == userId).CountAsync() + 1),
                InputLanguageId = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.Ocralias.ToLower() == inputLanguage.ToLower())).Id,
                OutputLanguageId = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.TranslatorAlias.ToLower() == outputLanguage.ToLower())).Id,
                RecognizedText = recognizedText.TextResult,
                RecognizedTextAccuracy = recognizedText.Confidence,
                Translate = translatedText,
                Screenshot = image.ToArray(),
                SelectedOcrid = (await _dbContext.Ocrs.FirstOrDefaultAsync(e => e.Name.ToLower() == ocrName.ToLower())).Id,
                SelectedTranslatorId = (await _dbContext.Translators.FirstOrDefaultAsync(e => e.Name.ToLower() == translatorName.ToLower())).Id,
                UserId = userId,
            };

            try
            {
                await _dbContext.Histories.AddAsync(history);
                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<User?> Authorization(string login, string password)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

            return result;
        }

        public async Task<Setting?> GetAllSettings(int userId, string profileName)
        {
            var result = await _dbContext.Settings.FirstOrDefaultAsync(e => e.UserId == userId && e.Name == profileName);

            return result;
        }

        public async Task<List<History>> GetTranslationHistory(int userId)
        {
            var result = await _dbContext.Histories.Where(e => e.UserId == userId).ToListAsync();

            return result;
        }

        public async Task<List<Language>> GetLanguageList()
        {
            var result = await _dbContext.Languages.ToListAsync();

            return result;
        }

        public async Task<List<Country>> GetCountryList()
        {
            var result = await _dbContext.Countries.ToListAsync();

            return result;
        }

        public async Task<List<Ocr>> GetOcrList()
        {
            var result = await _dbContext.Ocrs.ToListAsync();

            return result;
        }

        public async Task<List<Setting>> GetSettignsList()
        {
            var result = await _dbContext.Settings.ToListAsync();

            return result;
        }

        public async Task<List<Translator>> GetTranslatorList()
        {
            var result = await _dbContext.Translators.ToListAsync();

            return result;
        }

        public async Task SaveSettings(Setting settings)
        {
            try
            {
                var currentProfileSettings = await _dbContext.Settings.FirstOrDefaultAsync(e => e.Name == settings.Name && e.UserId == settings.UserId);

                if (currentProfileSettings == null)
                    await _dbContext.Settings.AddAsync(settings);
                else
                {
                    _dbContext.Settings.Remove(currentProfileSettings);
                    await _dbContext.Settings.AddAsync(settings);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}
