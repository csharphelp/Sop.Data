using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Sop.Data.Serilog.Extensions.Autofac.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DEFAULT_LOG_TEMPLATE
            = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logpath"></param>
        /// <param name="outputTemplate"></param>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static LoggerConfiguration DefaultLoggerConfiguration(string logpath, string outputTemplate, LogEventLevel logLevel)
        {
            return new LoggerConfiguration()
                           .Enrich.FromLogContext()
                           .MinimumLevel.Is(logLevel)
                           .WriteTo.RollingFile(logpath)
                           .WriteTo.Console(outputTemplate: outputTemplate, theme: AnsiConsoleTheme.Literate);
        }
    }
}
