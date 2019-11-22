using System.Collections.Generic;
using System.Text;
using Autofac;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace Sop.Data.Serilog.Extensions.Autofac.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    internal class SerilogModule : Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new LoggerFactory(new ILoggerProvider[] { new SerilogLoggerProvider() }))
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
        }
    }
}
