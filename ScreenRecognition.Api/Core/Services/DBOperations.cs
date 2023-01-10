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

        public async Task<User?> Authorization(string login, string password)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

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
            var profile = await _dbContext.Settings.FirstOrDefaultAsync(e => e.Name == settings.Name && e.UserId == settings.UserId);

            if (profile != null)
                return;

            try
            {
                await _dbContext.Settings.AddAsync(settings);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {

            }
        }
    }
}
