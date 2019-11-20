using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Sop.Data.Environment;
using Sop.Data.NhRepositories;
using Sop.Data.NhRepositories.Caches.Redis;
using Sop.Data.Test.Service;
using StackExchange.Redis;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Sop.Data.Test
{
    [TestClass]
    public static class TestSetup
    {
        private static SQLiteConnection _connection;
        public static ISessionFactory SessionFactory;
        public static IConfiguration Configuration { get; set; }
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
          
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, reloadOnChange: true);


                Configuration = builder.Build();

            }
            catch (Exception ex)
            {

            }

            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var files = Directory.EnumerateFiles(path, "Sop.*.dll");
            files = files.Union(Directory.EnumerateFiles(path, "Sop.*.dll"));
            var assemblyList = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToList();

            assemblyList.Add(Assembly.GetExecutingAssembly());
            assemblyList.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList());
            var assemblies = assemblyList.ToArray();
            //var types = assemblies
            //   .SelectMany(a => a.DefinedTypes)
            //   .Select(type => type.AsType())
            //   .Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService") && t.IsClass).ToArray();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();

            containerBuilder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService")).AsSelf()
                .SingleInstance()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
             
            containerBuilder.Register(c => ConnectionMultiplexer.Connect("123.56.198.152:6379,password=app2019,writeBuffer=10240,SyncTimeout=5000,AllowAdmin=true")).SingleInstance();
            //注册NHibernate的SessionManager
            containerBuilder.Register(c => new Sop.Data.NhRepositories.SessionManager(assemblies)).AsSelf().SingleInstance().PropertiesAutowired();

            IContainer container = containerBuilder.Build();
            DiContainer.RegisterContainer(container);




            var mysql = Configuration.GetConnectionString("mysql");


            //var SQLite = Configuration.GetConnectionString("SQLite");

            //_connection = new SQLiteConnection(SQLite);
            //_connection.Open();

        }


        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

    }


}