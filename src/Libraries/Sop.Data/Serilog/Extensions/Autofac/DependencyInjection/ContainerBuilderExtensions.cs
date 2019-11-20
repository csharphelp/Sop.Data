using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Sop.Data.Serilog.Extensions.Autofac.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logPath"></param>
        /// <param name="logEventLevel"></param>
        /// <param name="outputTemplate"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterSerilog(this ContainerBuilder builder, string logPath,
            LogEventLevel logEventLevel = LogEventLevel.Debug, string outputTemplate = Constants.DEFAULT_LOG_TEMPLATE)
        {
            if (string.IsNullOrWhiteSpace(logPath))
                throw new ArgumentNullException(nameof(logPath));

            if (string.IsNullOrWhiteSpace(outputTemplate))
                throw new ArgumentNullException(nameof(outputTemplate));

            return RegisterSerilogInternal(builder,
                Constants.DefaultLoggerConfiguration(logPath, outputTemplate, logEventLevel));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterSerilog(this ContainerBuilder builder,
            LoggerConfiguration loggerConfiguration) => RegisterSerilogInternal(builder, loggerConfiguration);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>
        private static ContainerBuilder RegisterSerilogInternal(this ContainerBuilder builder,
            LoggerConfiguration loggerConfiguration)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            Log.Logger = loggerConfiguration
                .CreateLogger();

            builder.RegisterModule<SerilogModule>();

            return builder;
        }
    }
}
