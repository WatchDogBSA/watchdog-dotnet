﻿namespace Watchdog.Common
{
    public abstract class WatchdogSettingsBase
    {
        internal const string _defaultApiEndPoint = "https://bsa-watchdog.westeurope.cloudapp.azure.com/collector/issues/client";
        
        protected WatchdogSettingsBase()
        {
            ApiEndpoint = _defaultApiEndPoint;
        }

        public string ApiEndpoint { get; set; }
    }
}