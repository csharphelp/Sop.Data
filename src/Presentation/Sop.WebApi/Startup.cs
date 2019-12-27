using System;
using System.IO;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Sop.WebApi
{
    /// <summary>
    ///    ASP.NET Core docs for Autofac are here:https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfigurationRoot Configuration { get; private set; }

        public void Configure(IApplicationBuilder app)
        {

            //app.UseDeveloperExceptionPage();
            //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();

            app.UseStaticFiles();
            app.UseDefaultFiles();



            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                  "Sop.WebApi");

                c.SwaggerEndpoint("/swagger/v2/swagger.json",
                                  "Sop.WebApi2");
                c.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();
            app.UseExceptionHandler(c =>
            {
                c.Run(async context =>
                {
                    //判断请求
                    if (context.Request != null)
                    {
                        var method = context.Request.Method;
                    }

                    var statusCodeReExecuteFeature = context.Features.Get<IStatusCodeReExecuteFeature>();

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();


                    if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                });
            });

            app.UseCors("Access-Control-Allow-Origin");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());




        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new AppModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use extensions from libraries to register services in the
            // collection. These will be automatically added to the
            // Autofac container.
            services.AddControllers();
            services.AddSopData(opt =>
            {
                opt.UseMySql("server =127.0.0.1;database=soptestdb;uid=root;password=123456;");
            });
            //
            //            var files = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "Sop.*.dll");
            //            files = files.Union(Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "Sop.Common.*.dll"));
            //            var assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();
            //
            //            
            //            services.AddSingleton<AppSessionFactory>();
            //            services.AddScoped(x => x.GetService<AppSessionFactory>().OpenSession);



            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"ToDo API 最后更新时间2019-10-18  16:10",
                    Version = "v1", 
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
              
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath,true);


                c.IncludeXmlComments("SopWebApi.xml", true);
            });

            //读取配置文件

            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            //            Convert.tostring.
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] keyByteArray = encoding.GetBytes(symmetricKeyAsBase64);
            //            byte[] keyByteArray = symmetricKeyAsBase64.GetBytes(Encoding.UTF8);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            services.AddAuthentication(x =>
                     {
                         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                     })
                    .AddJwtBearer(o =>
                     {
                         o.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = signingKey,
                             ValidateIssuer = true,
                             ValidIssuer = audienceConfig["Issuer"], //发行人
                             ValidateAudience = true,
                             ValidAudience = audienceConfig["Audience"], //订阅人
                             ValidateLifetime = true,
                             ClockSkew = TimeSpan.Zero,
                             RequireExpirationTime = true
                         };
                     });
            services.AddCors(options =>
            {
                options.AddPolicy("Access-Control-Allow-Origin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });



            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Access-Control-Allow-Origin",
            //                      builder =>
            //                      {
            //                          builder.AllowAnyOrigin()
            //                                 .AllowAnyMethod()
            //                                 .AllowAnyHeader()
            //                                 .AllowCredentials(); //指定处理cookie
            //                      });
            //});
        }
    }
}