﻿using System;
using System.Globalization;
using Watchdog.Common.Messages;

namespace Watchdog.Common.Builders
{
    public class WatchdogEnvironmentBuilder
    {
        protected WatchdogEnvironmentBuilder()
        {
        }

        public static WatchdogEnvironmentMessage Build(WatchdogSettingsBase settings)
        {
            var message = new WatchdogEnvironmentMessage();

            DateTime now = DateTime.UtcNow;
            message.UtcOffset = TimeZoneInfo.Local.GetUtcOffset(now).TotalHours;
            message.Locale = CultureInfo.CurrentCulture.DisplayName;
            message.Platform = Environment.OSVersion.Platform.ToString();

            return message;
        }
    }
}