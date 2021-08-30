using Microsoft.AspNetCore.Http;

namespace Watchdog.AspNetCore
{
    public interface IWatchdogAspNetCoreClientProvider
    {
        WatchdogClient GetClient(HttpContext context);
    }
}
