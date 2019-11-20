using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Sop.Data;
using Sop.Data.Environment;
using Sop.Data.NhRepositories;
using StackExchange.Redis;


namespace Sop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services = new ServiceCollection();
            services.AddLogging();

            #region   Autofac

            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var files = Directory.EnumerateFiles(path, "Sop.*.dll");
            //files = files.Union(Directory.EnumerateFiles(path, "Sop.*.dll"));
            //var assemblyList = files?.Select(n =>
            //{

            //    var name = AssemblyName.GetAssemblyName(n);
            //    try
            //    {
            //        var assembly = Assembly.Load(name);
            //        return assembly;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    } 


            //}).ToList();

            //assemblyList.Add(Assembly.GetExecutingAssembly());
            //assemblyList.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList());
            //var assemblies = assemblyList.ToArray();
            //var types = assemblies
            //   .SelectMany(a => a.DefinedTypes)
            //   .Select(type => type.AsType())
            //   .Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService") && t.IsClass).ToArray();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(n => n.FullName.StartsWith("Sop")).ToArray();

            var builder = new ContainerBuilder();



            builder.Register<Serilog.ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .WriteTo.RollingFile(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log-{Date}.txt")
                  .CreateLogger();
            }).SingleInstance();
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();





            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service") || t.Name.Contains("CacheService")).AsSelf()
                .SingleInstance()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.Register(c => ConnectionMultiplexer.Connect("123.56.198.152:6379,password=app2019,writeBuffer=10240,SyncTimeout=5000,AllowAdmin=true")).SingleInstance();
            //注册NHibernate的SessionManager
            builder.Register(c => new Sop.Data.NhRepositories.SessionManager(assemblies)).AsSelf().SingleInstance().PropertiesAutowired();



            builder.Populate(services);
            IContainer container = builder.Build();
            DiContainer.RegisterContainer(container);


            #endregion




            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

