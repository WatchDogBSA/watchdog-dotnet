namespace Watchdog.Common
{
    public abstract class WatchdogSettingsBase
    {
        private const string _defaultApiEndPoint = "https://bsa-watchdog.westeurope.cloudapp.azure.com/collector/issues/client";
        private const string _defaultCoreEndPoint = "https://bsa-watchdog.westeurope.cloudapp.azure.com/api";

        protected WatchdogSettingsBase()
        {
            ApiEndpoint = _defaultApiEndPoint;
            CoreEndpoint = _defaultCoreEndPoint;
        }

        public string ApiEndpoint { get; set; }
        public string CoreEndpoint { get; set; }

        public string ApiKey { get; set; }
    }
}
