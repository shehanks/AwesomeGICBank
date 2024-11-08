using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Application.Services;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Infrastructure.DataAccess;
using AwesomeGICBank.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank.CLI
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BankDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register application services
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();

            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));


            services.AddTransient<BankSystem>();

            return services;
        }
    }
}
