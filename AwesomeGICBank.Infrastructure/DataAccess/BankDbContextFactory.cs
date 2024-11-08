using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AwesomeGICBank.Infrastructure.DataAccess
{
    public class BankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")  // Ensure this file is in the ConsoleApp project's directory
                .Build();

            // Configure DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<BankDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new BankDbContext(optionsBuilder.Options);
        }
    }
}
