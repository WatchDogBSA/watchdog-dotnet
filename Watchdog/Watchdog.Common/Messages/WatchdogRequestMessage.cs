using System.Collections.Generic;

namespace Watchdog.Common.Messages
{
    public class WatchdogRequestMessage
    {
        public string HostName { get; set; }

        public string Url { get; set; }

        public string HttpMethod { get; set; }

        public string IPAddress { get; set; }

        public Dictionary<string, string> QueryString { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
