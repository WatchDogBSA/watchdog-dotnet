using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Watchdog.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IServiceCollection AddWatchdog(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureWathdogSettings(configuration);
            
            services.AddSingleton<IWatchdogAspNetCoreClientProvider, DefaultWatchdogAspNetCoreClientProvider>();
            services.AddSingleton<AspNetCoreDiagnosticObserver>();

            services.AddHostedService<WatchdogHostedService>();
            
            return services;
        }

        private static void ConfigureWathdogSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("WatchdogSettings");
            if (settings["ApiKey"] is null)
            {
                throw new KeyNotFoundException("Api key not found");
            }
            services.Configure<WatchdogSettings>(settings);
        }
    }
}
