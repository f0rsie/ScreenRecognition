using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Models;

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
            var resultTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.GetOcrList();

                return result;
            });

            return resultTask;
        }

        [Route("Translators")]
        [HttpGet]
        public async Task<List<Translator>> GetTranslators()
        {
            var resultTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.GetTranslatorList();

                return result;
            });

            return resultTask;
        }

        [Route("Settings")]
        [HttpGet]
        public async Task<List<Setting>> GetSettings(int userId)
        {
            var resultTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.GetSettignsList(userId);

                return result;
            });

            return resultTask;
        }

        [Route("Countries")]
        [HttpGet]
        public async Task<List<Country>> GetCountries()
        {
            var resultTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.GetCountryList();

                return result;
            });

            return resultTask;
        }

        [Route("Languages")]
        [HttpGet]
        public async Task<List<Language>> GetLanguages()
        {
            var resultTask = await Task.Run(async() =>
            {
                var result = await _dbOperations.GetLanguageList();

                return result;
            });

            return resultTask;
        }

        [Route("ProfileSettings")]
        [HttpGet]
        public async Task<Setting?> GetSettignsProfile(int userId, string name)
        {
            var resultTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.GetAllSettings(userId, name);

                return result;
            });

            return resultTask;
        }
    }
}
