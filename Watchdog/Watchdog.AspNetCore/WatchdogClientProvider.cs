using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Watchdog.AspNetCore
{
    public class DefaultWatchdogAspNetCoreClientProvider : IWatchdogAspNetCoreClientProvider
    {
        private readonly IOptionsMonitor<WatchdogSettings> _settings;
        private bool? isApiKeyListening;

        public DefaultWatchdogAspNetCoreClientProvider(IOptionsMonitor<WatchdogSettings> settings)
        {
            _settings = settings;
        }

        public virtual WatchdogClient GetClient(HttpContext context)
        {
            if (!isApiKeyListening.HasValue)
            {
                try
                {
                    var url = _settings.CurrentValue.CoreEndpoint + "/applications/listening/" + _settings.CurrentValue.ApiKey;
                    var result = new HttpClient().GetStringAsync(url).Result;
                    isApiKeyListening = result == "true";
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while checking api key, please restart application and try again! Error: " + ex.Message);
                    isApiKeyListening = false;
                }
            }

            if (!isApiKeyListening.Value)
            {
                throw new Exception("Your api key is not valid! Please check existing api key.");
            }

            return new WatchdogClient(_settings.CurrentValue, context);
        }
    }
}
