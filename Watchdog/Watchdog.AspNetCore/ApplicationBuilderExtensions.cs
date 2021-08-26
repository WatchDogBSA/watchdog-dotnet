using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Watchdog.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWatchdog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WatchdogAspNetMiddleware>();
        }

        public static IServiceCollection AddWatchdog(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureWathdogSettings(configuration);

            services.AddTransient<IWatchdogAspNetCoreClientProvider>(_ => new DefaultWatchdogAspNetCoreClientProvider());
            services.AddSingleton<WatchdogMiddlewareSettings>();

            return services;
        }

        public static IServiceCollection AddWatchdog(this IServiceCollection services, IConfiguration configuration, WatchdogMiddlewareSettings middlewareSettings)
        {
            services.ConfigureWathdogSettings(configuration);

            services.AddTransient(_ => middlewareSettings.ClientProvider ?? new DefaultWatchdogAspNetCoreClientProvider());
            services.AddTransient(_ => middlewareSettings);

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
