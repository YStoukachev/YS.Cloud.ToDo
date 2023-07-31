using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YS.Azure.ToDo.IoC;

namespace YS.Azure.ToDo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddAppOptions()
                        .AddLogging()
                        .AddValidator()
                        .AddBusinessLogic()
                        .AddAppDbContext()
                        .AddAppMappers();
                })
                .ConfigureAppConfiguration(ConfigureOptions)
                .Build();

            await host.RunAsync();
        }

        private static void ConfigureOptions(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);

            configurationBuilder
                .AddEnvironmentVariables();

            configurationBuilder.Build();
        }
    }
}