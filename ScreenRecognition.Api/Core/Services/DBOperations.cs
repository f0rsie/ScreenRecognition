﻿using Microsoft.EntityFrameworkCore;
using ScreenRecognition.Api.Models.DbModels;
using ScreenRecognition.Api.Models.ResultsModels.OcrResultModels;

namespace ScreenRecognition.Api.Core.Services
{
    public class DBOperations
    {
        private Data.ScreenRecognitionContext _dbContext;

        public DBOperations()
        {
            _dbContext = new Data.ScreenRecognitionContext();
        }

        public async Task<string?> GetOcrLanguageAlias(string language)
        {
            var result = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.Name == language))?.Ocralias;

            return result;
        }

        public async Task<string?> GetTranslatorLanguageAlias(string language)
        {
            var result = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.Name == language))?.TranslatorAlias;

            return result;
        }

        public async Task SaveHistory(string translatorName, string ocrName, List<byte> image, string inputLanguage, string outputLanguage, string userLogin, string userPassword, OcrResultModel recognizedText, string translatedText)
        {
            if (userLogin == "guest" || userPassword == "guest")
                return;

            var userId = (await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == userLogin)).Id;
            var history = new History
            {
                Id = (await _dbContext.Histories.Where(e => e.UserId == userId).CountAsync() + 1),
                InputLanguageId = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.Name.ToLower() == inputLanguage.ToLower()))?.Id,
                OutputLanguageId = (await _dbContext.Languages.FirstOrDefaultAsync(e => e.Name.ToLower() == outputLanguage.ToLower()))?.Id,
                RecognizedText = recognizedText.TextResult,
                RecognizedTextAccuracy = recognizedText.Confidence,
                Translate = translatedText,
                Screenshot = image.ToArray(),
                SelectedOcrid = (await _dbContext.Ocrs.FirstOrDefaultAsync(e => e.Name.ToLower() == ocrName.ToLower()))?.Id,
                SelectedTranslatorId = (await _dbContext.Translators.FirstOrDefaultAsync(e => e.Name.ToLower() == translatorName.ToLower()))?.Id,
                UserId = userId,
            };

            try
            {
                await _dbContext.Histories.AddAsync(history);
                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task ChangeAccountInfo(User? user)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == user.Id);

            if (existingUser == null)
                return;

            existingUser.Login = user.Login;
            existingUser.Password = user.Password;
            existingUser.NickName = user.NickName;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Birthday = user.Birthday;
            existingUser.CountryId = user.CountryId;
            existingUser.Country = null;

            try
            {
                _dbContext.Users.Update(existingUser);
                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task ClearTranslateHistory(User? user)
        {
            var userHistory = await _dbContext.Histories.Where(e => e.UserId == user.Id).ToListAsync();

            if (userHistory.Count == 0)
                return;

            try
            {
                _dbContext.Histories.RemoveRange(userHistory);
                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            var result = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Id == roleId);

            return result;
        }

        public async Task<bool> LoginAvailability(string login)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login);

            if (result == null)
                return true;

            return false;
        }

        public async Task<bool> MailAvailability(string mail)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == mail);

            if (result == null)
                return true;

            return false;
        }

        public async Task Registration(User user)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == user.Login);

            if (existingUser != null)
                return;

            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch { }
        }

        public async Task<User?> Authorization(string login, string password)
        {
            var result = await _dbContext.Users
                .Include(e => e.Country)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

            return result;
        }

        public async Task<Setting?> GetAllSettings(int userId, string profileName)
        {
            var result = await _dbContext.Settings
                .Include(e => e.InputLanguage)
                .Include(e => e.OutputLanguage)
                .Include(e => e.SelectedOcr)
                .Include(e => e.SelectedTranslator)
                .Include(e => e.User)
                    .Include(e => e.User.Country)
                    .Include(e => e.User.Role)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Name == profileName);

            return result;
        }

        public async Task<List<History>> GetTranslationHistory(int userId)
        {
            var result = await _dbContext.Histories
                .Include(e => e.InputLanguage)
                .Include(e => e.OutputLanguage)
                .Include(e => e.SelectedOcr)
                .Include(e => e.SelectedTranslator)
                .Include(e => e.User)
                    .Include(e => e.User.Country)
                    .Include(e => e.User.Role)
                .Where(e => e.UserId == userId).ToListAsync();

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

        public async Task<List<Setting>> GetSettignsList(int userId)
        {
            var result = await _dbContext.Settings
                .Include(e => e.InputLanguage)
                .Include(e => e.OutputLanguage)
                .Include(e => e.SelectedOcr)
                .Include(e => e.SelectedTranslator)
                .Include(e => e.User)
                    .Include(e => e.User.Country)
                    .Include(e => e.User.Role)
                .Where(e => e.UserId == userId).ToListAsync();

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
                    currentProfileSettings.InputLanguageId = settings.InputLanguageId;
                    currentProfileSettings.OutputLanguageId = settings.OutputLanguageId;
                    currentProfileSettings.ResultColor = settings.ResultColor;
                    currentProfileSettings.SelectedTranslatorId = settings.SelectedTranslatorId;
                    currentProfileSettings.SelectedOcrid = settings.SelectedOcrid;
                    currentProfileSettings.TranslatorApiKey = settings.TranslatorApiKey;

                    currentProfileSettings.InputLanguage = null;
                    currentProfileSettings.OutputLanguage = null;
                    currentProfileSettings.SelectedTranslator = null;
                    currentProfileSettings.SelectedOcr = null;

                    _dbContext.Settings.Update(currentProfileSettings);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}
