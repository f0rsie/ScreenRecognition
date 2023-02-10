using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Models.DbModels;

namespace ScreenRecognition.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private DBOperations _dbOperations;

        public SettingsController()
        {
            _dbOperations = new DBOperations();
        }

        [Route("SaveProfile")]
        [HttpPost]
        public async Task SaveProfile(Setting settings)
        {
            await _dbOperations.SaveSettings(settings);
        }

        [Route("Ocrs")]
        [HttpGet]
        public async Task<List<Ocr>> GetOcrs()
        {
            var result = await _dbOperations.GetOcrList();

            return result;
        }

        [Route("Translators")]
        [HttpGet]
        public async Task<List<Translator>> GetTranslators()
        {
            var result = await _dbOperations.GetTranslatorList();

            return result;
        }

        [Route("Settings")]
        [HttpGet]
        public async Task<List<Setting>> GetSettings(int userId)
        {
            var result = await _dbOperations.GetSettignsList(userId);

            return result;
        }

        [Route("Countries")]
        [HttpGet]
        public async Task<List<Country>> GetCountries()
        {
            var result = await _dbOperations.GetCountryList();

            return result;
        }

        [Route("Languages")]
        [HttpGet]
        public async Task<List<Language>> GetLanguages()
        {
            var result = await _dbOperations.GetLanguageList();

            return result;
        }

        [Route("ProfileSettings")]
        [HttpGet]
        public async Task<Setting?> GetSettignsProfile(int userId, string name)
        {
            var result = await _dbOperations.GetAllSettings(userId, name);

            return result;
        }
    }
}
