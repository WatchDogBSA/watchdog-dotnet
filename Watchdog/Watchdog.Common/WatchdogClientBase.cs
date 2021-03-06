using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watchdog.Common.Builders;
using Watchdog.Common.Messages;

namespace Watchdog.Common
{
    public abstract class WatchdogClientBase
    {
        private static readonly HttpClient _client = new HttpClient();
        
        private readonly List<Type> _wrapperExceptions = new List<Type>();
        
        protected readonly WatchdogSettingsBase _settings;

        protected WatchdogClientBase(WatchdogSettingsBase settings)
        {
            _settings = settings;

            _wrapperExceptions.Add(typeof(TargetInvocationException));
        }

        public void AddWrapperExceptions(params Type[] wrapperExceptions)
        {
            foreach (Type wrapper in wrapperExceptions)
            {
                if (!_wrapperExceptions.Contains(wrapper))
                {
                    _wrapperExceptions.Add(wrapper);
                }
            }
        }

        public void RemoveWrapperExceptions(params Type[] wrapperExceptions)
        {
            foreach (Type wrapper in wrapperExceptions)
            {
                _wrapperExceptions.Remove(wrapper);
            }
        }

        protected virtual bool CanSend(Exception exception)
        {
            return exception is not null;
        }

        protected virtual bool CanSend(WatchdogMessage message)
        {
            return message is not null;
        }

        public async Task SendAsync(WatchdogMessage watchdogMessage)
        {
            if (!CanSend(watchdogMessage))
            {
                return;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _settings.ApiEndpoint);

            var message = JsonConvert.SerializeObject(watchdogMessage, Formatting.Indented);
            requestMessage.Content = new StringContent(message, Encoding.UTF8, "application/json");

            await _client.SendAsync(requestMessage);
        }

        public virtual async Task SendAsync(Exception exception)
        {
            if (CanSend(exception))
            {
                await StripAndSendAsync(exception);
            }
        }
        
        public void Send(Exception exception)
        {
            SendAsync(exception).Wait();
        }

        protected async Task StripAndSendAsync(Exception exception)
        {
            foreach (Exception e in StripWrapperExceptions(exception))
            {
                await SendAsync(BuildMessage(e));
            }
        }

        protected IEnumerable<Exception> StripWrapperExceptions(Exception exception)
        {
            if (exception is not null && _wrapperExceptions.Any(wrapperException =>
                exception.GetType() == wrapperException && exception.InnerException is not null))
            {
                AggregateException aggregate = exception as AggregateException;

                if (aggregate is not null)
                {
                    foreach (Exception e in aggregate.InnerExceptions)
                    {
                        foreach (Exception ex in StripWrapperExceptions(e))
                        {
                            yield return ex;
                        }
                    }
                }

                foreach (Exception e in StripWrapperExceptions(exception.InnerException))
                {
                    yield return e;
                }
            }

            yield return exception;
        }

        protected virtual WatchdogMessage BuildMessage(Exception exception)
        {
            return WatchdogMessageBuilder.New(_settings)
                .SetTimeStamp(DateTime.UtcNow)
                .SetMachineName(Environment.MachineName)
                .SetEnvironmentDetails()
                .SetExceptionDetails(exception)
                .Build();
        }
    }
}
