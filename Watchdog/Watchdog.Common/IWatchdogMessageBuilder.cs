using System;
using Watchdog.Common.Messages;

namespace Watchdog.Common
{
    public interface IWatchdogMessageBuilder
    {
        WatchdogMessage Build();

        IWatchdogMessageBuilder SetTimeStamp(DateTime? currentTime);

        IWatchdogMessageBuilder SetMachineName(string machineName);

        IWatchdogMessageBuilder SetEnvironmentDetails();

        IWatchdogMessageBuilder SetExceptionDetails(Exception exception);

        IWatchdogMessageBuilder SetRequestDetails(WatchdogRequestMessage message);

        IWatchdogMessageBuilder SetResponseDetails(WatchdogResponseMessage message);
    }
}
