using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Sop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
             
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();


            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var logger = services.GetRequiredService<ILogger<Program>>();
            //    try
            //    {
            //        logger.LogInformation("Seeding API database");
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError("Error creating/seeding API database - " + ex);
            //    }
            //}

            host.Run();

        }



    }
}
