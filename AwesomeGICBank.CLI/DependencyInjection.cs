using AwesomeGICBank.Application;
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

            // Register AutoMapper with profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); // Register your profile
            });

            // Register application services
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IBankingServiceCoordinator, BankingServiceCoordinator>();

            return services;
        }
    }
}
