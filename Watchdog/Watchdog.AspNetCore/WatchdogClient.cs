using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Watchdog.AspNetCore.Builders;
using Watchdog.Common;
using Watchdog.Common.Builders;
using Watchdog.Common.Messages;

namespace Watchdog.AspNetCore
{
    public class WatchdogClient : WatchdogClientBase
    {
        private readonly ThreadLocal<HttpContext> _currentHttpContext = new ThreadLocal<HttpContext>(() => null);
        private readonly ThreadLocal<WatchdogRequestMessage> _currentRequestMessage = new ThreadLocal<WatchdogRequestMessage>(() => null);
        private readonly ThreadLocal<WatchdogResponseMessage> _currentResponseMessage = new ThreadLocal<WatchdogResponseMessage>(() => null);

        public WatchdogClient(WatchdogSettings settings, HttpContext context = null)
            : base (settings)
        {
            if (context is not null)
            {
                SetCurrentContext(context);
            }
        }

        private WatchdogSettings GetSettings()
        {
            return (WatchdogSettings)_settings;
        }

        public WatchdogClient SetCurrentContext(HttpContext context)
        {
            _currentHttpContext.Value = context;
            return this;
        }

        protected override bool CanSend(WatchdogMessage message)
        {
            return message?.Details?.Response is not null;
        }

        public override async Task SendAsync(Exception exception)
        {
            if (CanSend(exception))
            {
                var currentRequestMessage = BuildRequestMessage();
                var currentResponseMessage = BuildResponseMessage();

                _currentHttpContext.Value = null;

                _currentRequestMessage.Value = currentRequestMessage;
                _currentResponseMessage.Value = currentResponseMessage;

                await StripAndSendAsync(exception);
            }
        }

        public void TrackException(Exception exception)
        {
            var feature = new ExceptionHandlerFeature
            {
                Error = exception,
                Path = _currentHttpContext.Value.Request.Path
            };
            _currentHttpContext.Value.Features.Set<IExceptionHandlerFeature>(feature);
            _currentHttpContext.Value.Features.Set<IExceptionHandlerPathFeature>(feature);
        }

        private WatchdogRequestMessage BuildRequestMessage()
        {
            return _currentHttpContext.Value is not null ? WatchdogAspNetCoreRequestMessageBuilder.Build(_currentHttpContext.Value) : null;
        }

        private WatchdogResponseMessage BuildResponseMessage()
        {
            return _currentHttpContext.Value is not null ? WatchdogAspNetCoreResponseMessageBuilder.Build(_currentHttpContext.Value) : null;
        }

        protected override WatchdogMessage BuildMessage(Exception exception)
        {
            return WatchdogMessageBuilder.New(GetSettings())
              .SetTimeStamp(DateTime.UtcNow)
              .SetMachineName(Environment.MachineName)
              .SetEnvironmentDetails()
              .SetExceptionDetails(exception)
              .SetResponseDetails(_currentResponseMessage.Value)
              .SetRequestDetails(_currentRequestMessage.Value)
              .Build();
        }
    }
}
