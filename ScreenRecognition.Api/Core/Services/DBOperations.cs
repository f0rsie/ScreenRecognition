using Microsoft.EntityFrameworkCore;

namespace ScreenRecognition.Api.Core.Services
{
    public class DBOperations
    {
        Data.ScreenRecognitionContext _dbContext;

        public DBOperations()
        {
            _dbContext = new Data.ScreenRecognitionContext();
        }

        public async Task<Models.User?> Authorization(string login, string password)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Login == login && e.Password == password);

            return result;
        }
    }
}
