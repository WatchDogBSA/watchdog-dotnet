namespace Watchdog.Common
{
    public abstract class WatchdogSettingsBase
    {
        private const string _defaultApiEndPoint = "https://bsa-watchdog.westeurope.cloudapp.azure.com/collector/issues/client";
        
        protected WatchdogSettingsBase()
        {
            ApiEndpoint = _defaultApiEndPoint;
        }

        public string ApiEndpoint { get; set; }

        public string ApiKey { get; set; }
    }
}
