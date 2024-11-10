using Microsoft.EntityFrameworkCore;

namespace AwesomeGICBank.Infrastructure.DataAccess
{
    public class DatabaseInitializer
    {
        private readonly BankDbContext _dbContext;

        public DatabaseInitializer(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // Ensure the database is created and apply migrations
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
    }
}
