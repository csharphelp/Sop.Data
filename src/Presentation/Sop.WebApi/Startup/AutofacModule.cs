using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Sop.Data;
using Sop.Data.Caching;
using Sop.Data.NhRepositories;
using Sop.WebApi.Services;
using StackExchange.Redis;
using Module = Autofac.Module;

namespace Sop.WebApi
{
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

            //获取当前相关的程序集
            var files = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "Sop.*.dll");
            files = files.Union(Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "Sop.Common.*.dll"));
            var assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();


            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("Service") && !t.Name.Contains("CacheService"))
                   .AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies|PropertyWiringOptions.PreserveSetValues);

////集中注册服务
//            foreach (var item in GetClassName("Service"))
//            {
//                foreach (var typeArray in item.Value)
//                {
//                    builder.Register(c => item.Key).As(typeArray).SingleInstance()
//                           .AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope()
//                           .PropertiesAutowired();
//                }
//            }


            //注册Redis服务
            string redis = "127.0.0.1:6379,defaultDatabase=1,ssl=false,writeBuffer=10240";
            var option = ConfigurationOptions.Parse(redis);
            builder.Register(c => ConnectionMultiplexer.Connect(option)).SingleInstance().PropertiesAutowired();

            builder.Register(c => new RedisCacheManager(option)).As<ICacheManager>().SingleInstance()
                   .PropertiesAutowired();


            //注册Repository
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>))
                   .InstancePerLifetimeScope();

            //注册NHibernate的SessionManager
            builder.Register(@delegate: c => new SessionManager(assemblies)).AsSelf().SingleInstance()
                   .PropertiesAutowired();


            //注册缓存服务，每次请求都是一个新的实例
//            builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().SingleInstance().PropertiesAutowired();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                var assembly = Assembly.Load(assemblyName);
                var ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }

                return result;
            }

            return new Dictionary<Type, Type[]>();
        }
    }
}