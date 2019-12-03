using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Sop.WebApi
{
    /// <summary>
    ///     ///
    ///     <summary>
    ///         ASP.NET Core docs for Autofac are here:https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html
    ///     </summary>
    /// </summary>
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(env.ContentRootPath)
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration
        {
            get;
            private set;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();


            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                  "Sop.WebApi");
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
            app.UseMvc();


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
            builder.RegisterModule(new AutofacModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use extensions from libraries to register services in the
            // collection. These will be automatically added to the
            // Autofac container.
            services.AddControllers();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Info {Version = "v1", Title = "API", Description = "api文档", TermsOfService = "None"});

                var xmlPath = Path.Combine(AppContext.BaseDirectory,
                                           "SopWebApi.xml");
                options.IncludeXmlComments("SopWebApi.xml",
                                           true);
                var security = new Dictionary<string, IEnumerable<string>> {{"Sop.WebApi", new string[] { }}};
                options.AddSecurityRequirement(security);
                options.AddSecurityDefinition("Sop.WebApi",
                                              new ApiKeyScheme
                                              {
                                                  Description = "token",
                                                  Name = "token",
                                                  In = "header",
                                                  Type = "apiKey"
                                              });
            });


            //读取配置文件

            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
//            Convert.tostring.
            System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
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
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyMethod()
                                             .AllowAnyHeader()
                                             .AllowCredentials(); //指定处理cookie
                                  });
            });
        }
    }
}