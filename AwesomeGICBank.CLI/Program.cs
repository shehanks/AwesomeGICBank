using AwesomeGICBank.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AwesomeGICBank.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, config) =>
               {
                   config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
               })
               .ConfigureServices((context, services) =>
               {
                   services.ConfigureServices(context.Configuration);
               })
               .Build();

            // Resolve the main service and run the application
            var app = host.Services.GetRequiredService<IBankingServiceCoordinator>();
            await app.Run();
        }
    }
}