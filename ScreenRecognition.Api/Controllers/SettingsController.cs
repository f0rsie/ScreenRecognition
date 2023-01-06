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

        [Route("GetAny")]
        [HttpGet]
        public async Task<object> GetAny(string name)
        {
            var resultTask = await Task.Run(async() =>
            {
                var result = await _dbOperations.GetAny(name);

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
                var result = await _dbOperations.Getlanguages();

                return result;
            });

            return resultTask;
        }
    }
}
