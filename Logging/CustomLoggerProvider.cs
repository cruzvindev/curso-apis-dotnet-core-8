using System.Collections.Concurrent;

namespace APICatalogo.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration loggerConfig;
    private readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}