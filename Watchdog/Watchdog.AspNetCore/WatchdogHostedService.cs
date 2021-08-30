using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Watchdog.AspNetCore
{
    public sealed class WatchdogHostedService : IHostedService
    {
        private IDisposable _subscription = null;

        private readonly AspNetCoreDiagnosticObserver _observer;
        private readonly DiagnosticListener _diagnosticListenerSource;

        public WatchdogHostedService(AspNetCoreDiagnosticObserver observer, DiagnosticListener diagnosticListenerSource)
        {
            _observer = observer;
            _diagnosticListenerSource = diagnosticListenerSource;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscription = _diagnosticListenerSource.Subscribe(_observer);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscription?.Dispose();
            return Task.CompletedTask;
        }
    }
}
