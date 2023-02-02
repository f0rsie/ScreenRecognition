using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ScreenRecognition.Api.Core.Services;
using ScreenRecognition.Api.Models;

namespace ScreenRecognition.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBOperations _dbOperations;

        public UserController()
        {
            _dbOperations = new DBOperations();
        }

        [Route("Auth")]
        [HttpGet]
        public async Task<User?> Authorization(string login, string password)
        {
            var asyncTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.Authorization(login, password);

                return result;
            });

            return asyncTask;
        }

        [Route("LoginCheck")]
        [HttpGet]
        public async Task<bool> LoginCheck(string login)
        {
            var asyncTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.LoginAvailability(login);

                return result;
            });

            return asyncTask;
        }

        [Route("MailCheck")]
        [HttpGet]
        public async Task<bool> MailCheck(string mail)
        {
            var asyncTask = await Task.Run(async () =>
            {
                var result = await _dbOperations.MailAvailability(mail);

                return result;
            });

            return asyncTask;
        }

        [Route("Registration")]
        [HttpPost]
        public async Task Registration(User user)
        {
            await Task.Run(async () =>
            {
                await _dbOperations.Registration(user);
            });
        }

        [Route("ChangeAccountInfo")]
        [HttpPost]
        public async Task ChangeAccountInfo(User? user)
        {
            await Task.Run(async () =>
            {
                await _dbOperations.ChangeAccountInfo(user);
            });
        }

        [Route("ClearTranslateHistory")]
        [HttpPost]
        public async Task ClearTranslateHistory(User? user)
        {
            await Task.Run(async () =>
            {
                await _dbOperations.ClearTranslateHistory(user);
            });
        }

        [Route("TranslationHistory")]
        [HttpGet]
        public async Task<List<History>?> TranslationHistory(int userId)
        {
            var asyncTask = await Task.Run(async() =>
            {
                var result = await _dbOperations.GetTranslationHistory(userId);

                return result;
            });

            return asyncTask;
        }
    }
}
