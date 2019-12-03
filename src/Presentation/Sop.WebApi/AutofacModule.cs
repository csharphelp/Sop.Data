using Autofac;
using Microsoft.Extensions.Logging;

namespace Sop.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method. All of this starts
            // with the `UseServiceProviderFactory(new AutofacServiceProviderFactory())` that happens in Program and registers Autofac
            // as the service provider.
            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                   .As<IValuesService>()
                   .InstancePerLifetimeScope();
        }
    }
}