using Ionta.OSC.App;
using System.Collections.Concurrent;

namespace Ionta.OSC.Web.Infrastructure.CustomLoggerProvider
{
    [ProviderAlias("DataBase")]
    public class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? _onChangeToken;
        private readonly ConcurrentDictionary<string, DataBaseLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly IServiceProvider _services;

        public DataBaseLoggerProvider(IServiceProvider services)
        {
            _services = services;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DataBaseLogger(name, _services));
        }

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
    }
}
