using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Watchdog.AspNetCore
{
    public class DefaultWatchdogAspNetCoreClientProvider : IWatchdogAspNetCoreClientProvider
    {
        private readonly IOptionsMonitor<WatchdogSettings> _settings;

        public DefaultWatchdogAspNetCoreClientProvider(IOptionsMonitor<WatchdogSettings> settings)
        {
            _settings = settings;
        }

        public virtual WatchdogClient GetClient(HttpContext context)
        {
            return new WatchdogClient(_settings.CurrentValue, context);
        }
    }
}
