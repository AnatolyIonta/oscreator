using Ionta.OSC.App;
using Ionta.OSC.Domain;
using System.Reflection;

namespace Ionta.OSC.Web.Infrastructure.CustomLoggerProvider
{
    [ProviderAlias("DataBase")]
    public class DataBaseLogger : ILogger
    {
        private readonly IServiceProvider _services;
        private readonly string _name;
        public DataBaseLogger(string name, IServiceProvider services)
        {
            _services= services;
            _name= name;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull=> default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error || logLevel == LogLevel.Warning || logLevel == LogLevel.Critical;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            
            var logType = getType(logLevel);

            if (logType == null) return;

            using (var scoped = _services.CreateScope())
            {
                var _storage = scoped.ServiceProvider.GetRequiredService<IOscStorage>();
                _storage.Logs.Add(new LogData()
                {
                    Type = logType,
                    Message = formatter(state, exception),
                    StackTace = exception?.StackTrace,
                    Time = DateTime.UtcNow,
                });
                _storage.SaveChangesAsync().Wait();
            }
        }

        private LogType getType(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Warning: return LogType.Warrning;
                case LogLevel.Error: return LogType.Error;
                case LogLevel.Critical: return LogType.Error;
                case LogLevel.Information: return LogType.Secsses;
            }

            return LogType.Warrning;
        }
    }
}
