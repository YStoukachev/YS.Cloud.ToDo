using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.EntityFramework;
using YS.Azure.ToDo.Repositories;
using YS.Azure.ToDo.Services;

namespace YS.Azure.ToDo.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });

            return services;
        }

        public static IServiceCollection AddValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services
                .AddScoped<IToDoService, ToDoService>()
                .AddScoped<IBlobStorageService, BlobStorageService>()
                .AddScoped<IBlobClientProvider, BlobClientProvider>()
                .AddScoped<IFormParser, FormParser>();
                
            services    
                .AddScoped<IArchivedTasksRepository, ArchivedTasksRepository>()
                .AddScoped<IToDoCosmosRepository, ToDoCosmosRepository>();

            return services;
        }

        public static IServiceCollection AddAppOptions(this IServiceCollection services)
        {
            services.AddOptions();
            
            services
                .AddOptions<BlobStorageOptions>()
                .Configure<IConfiguration>((options, config) => 
                    config.GetSection(nameof(BlobStorageOptions)).Bind(options));
            
            services
                .AddOptions<CosmosDbOptions>()
                .Configure<IConfiguration>((options, config) => 
                    config.GetSection(nameof(CosmosDbOptions)).Bind(options));

            services
                .AddOptions<SqlDatabaseOptions>()
                .Configure<IConfiguration>((options, config) =>
                    config.GetSection(nameof(SqlDatabaseOptions)).Bind(options));

            services
                .AddOptions<ToDoOptions>()
                .Configure<IConfiguration>((options, config) =>
                    config.GetSection(nameof(ToDoOptions)).Bind(options));

            return services;
        }

        public static IServiceCollection AddAppDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ToDoContext>((provider, options) =>
            {
                var databaseOptions = provider.GetRequiredService<IOptions<SqlDatabaseOptions>>().Value;

                options.UseSqlServer(databaseOptions.ConnectionString);
            });
            
            return services;
        }
    }
}