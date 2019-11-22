 
using Autofac;
using Sop.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Serilog;
using StackExchange.Redis;
using Sop.Data.NhRepositories;
using System.Linq;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using Sop.Data.Environment;
using Sop.Data;
using Sop.Domain.Domain.Services;

namespace Sop.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacModules : Autofac.Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {


            #region   Autofac
            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
               .As<IValuesService>()
               .InstancePerLifetimeScope();

            builder.Register(c => new PostService())
             .As<IPostService>()
             .InstancePerLifetimeScope();

            //var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            //var files = Directory.EnumerateFiles(path, "Sop.*.dll");
            //files = files.Union(Directory.EnumerateFiles(path, "Sop.*.dll"));


            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(n => n.FullName.StartsWith("Sop")).ToArray();
            ////assemblies.Add(Assembly.GetExecutingAssembly());
            ////assemblies.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList());
            ////var assemblies = assemblyList.ToArray();
            ////var types = assemblies
            ////   .SelectMany(a => a.DefinedTypes)
            ////   .Select(type => type.AsType())
            ////   .Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService") && t.IsClass).ToArray();




            //builder.Register<Serilog.ILogger>((c, p) =>
            //{
            //    return new LoggerConfiguration()
            //      .WriteTo.RollingFile(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log-{Date}.txt")
            //      .CreateLogger();
            //}).SingleInstance();
            //builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            //builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();





            //builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();

            //builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService")).AsSelf()
            //    .SingleInstance()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);



            //builder.Register(c => ConnectionMultiplexer.Connect("123.56.198.152:6379,password=app2019,writeBuffer=10240,SyncTimeout=5000,AllowAdmin=true")).SingleInstance();
            ////注册NHibernate的SessionManager
            //builder.Register(c => new Sop.Data.NhRepositories.SessionManager(assemblies)).AsSelf().SingleInstance().PropertiesAutowired();


            //IContainer container = builder.Build();
            //DiContainer.RegisterContainer(container);


            #endregion
        }
    }
}