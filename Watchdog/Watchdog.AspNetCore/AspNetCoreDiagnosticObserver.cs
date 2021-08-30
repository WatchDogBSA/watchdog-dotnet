using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Watchdog.AspNetCore
{
    public class AspNetCoreDiagnosticObserver : IObserver<KeyValuePair<string, object>>
    {
        private readonly IWatchdogAspNetCoreClientProvider _clientProvider;

        public AspNetCoreDiagnosticObserver(IWatchdogAspNetCoreClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            var (httpContext, exception) = GetMessageProperties(value);
            if (httpContext is not null && exception is not null)
            {
                _clientProvider.GetClient(httpContext).SendAsync(exception).NoAwait();
            }
        }

        private (HttpContext context, Exception exception) GetMessageProperties(KeyValuePair<string, object> value)
        {
            if (value.Key == "Microsoft.AspNetCore.Hosting.EndRequest")
            {
                var httpContext = value.Value.GetType().GetProperty("httpContext").GetValue(value.Value) as HttpContext;
                var feature = httpContext.Features.Get<IExceptionHandlerFeature>();
                var exception = feature?.Error;
                return (httpContext, exception);
            }
            if (value.Key == "Microsoft.AspNetCore.Hosting.UnhandledException")
            {
                var httpContext = value.Value.GetType().GetProperty("httpContext").GetValue(value.Value) as HttpContext;
                var exception = value.Value.GetType().GetProperty("exception").GetValue(value.Value) as Exception;
                return (httpContext, exception);
            }

            return default;
        }
    }
}
