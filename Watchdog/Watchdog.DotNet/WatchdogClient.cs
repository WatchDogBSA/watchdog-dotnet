using Watchdog.Common;

namespace Watchdog.DotNet
{
    public class WatchdogClient : WatchdogClientBase
    {
        public WatchdogClient(string apiKey)
            : this(new WatchdogSettings {ApiKey = apiKey})
        {
        }

        public WatchdogClient(WatchdogSettingsBase settings) : base(settings)
        {
        }
    }
}