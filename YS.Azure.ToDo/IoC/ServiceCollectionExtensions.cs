using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts;
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
                .AddScoped<IToDoService, ToDoService>();
            
            return services;
        }

        public static IServiceCollection AddAppOptions(this IServiceCollection services)
        {
            services.AddOptions();
            
            return services;
        }
    }
}