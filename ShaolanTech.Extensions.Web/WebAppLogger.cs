

namespace ShaolanTech.Extensions.Web
{
    public class WebAppLogger : ILogger
    {
        public static LogPipe LogPipe { get; set; } = null;
        public IDisposable? BeginScope<TState>(TState state) { return null; }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var msg=formatter(state, exception);
            if (LogPipe != null)
            {
                await LogPipe.Write("WebHost", logLevel.ToString(), msg);
            }
        }
    }
    public sealed class WebAppLoggerConfiguration
    {
         
    }
    public static class WebAppLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddWebAppLogger(
        this ILoggingBuilder builder, LogPipe logpipe)
        {
            WebAppLogger.LogPipe = logpipe;
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(  ServiceDescriptor.Singleton<ILoggerProvider, WebAppLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<WebAppLoggerConfiguration, WebAppLoggerProvider>(builder.Services);

            return builder;
        }

       
    }

    public class WebAppLoggerProvider : ILoggerProvider
    {
        private static WebAppLogger logger = new WebAppLogger();

        public WebAppLoggerProvider()
        {
            
        }

        public ILogger CreateLogger(string categoryName) => logger ;

      

        public void Dispose()
        {
            
        }
    }
}
