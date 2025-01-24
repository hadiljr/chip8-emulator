using Microsoft.Extensions.Logging;

namespace Hardware.Util;

/// <summary>
/// Singleton Logger class.
/// </summary>
public class Logger
{
    private ILoggerFactory _factory;

    private static Logger? _instance;

    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }
            return _instance;
        }
    }
    private Logger()
    {
        _factory = LoggerFactory.Create(builder => builder.AddConsole());
    }

    public void Configure(ILoggerFactory factory)
    {
        if (_factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }
        _factory = factory;
    }

    public ILogger GetLogger(string category)
    {
        return _factory.CreateLogger(category);
    }

    public ILogger<T> GetLogger<T>()
    {
        return _factory.CreateLogger<T>();
    }

}
