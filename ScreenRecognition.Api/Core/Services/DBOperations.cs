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

        public async Task<Models.User?> Authorization(string login, string password)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

            return result;
        }

        public async Task<List<Models.Language>> Getlanguages()
        {
            var result = await _dbContext.Languages.ToListAsync();

            return result;
        }

        // Не работает, переделать потом
        public async Task<object> GetAny(string name)
        {
            var type = TextOperations.FindElement<object>(name.Remove(name.Length - 1, 1));
            var gg = type.GetType();

            var res = _dbContext.Model.FindEntityType(gg);
            // var entity = _dbSetContext.Set(type.GetType());


            var result = new object();

            return result;
        }
    }
}
