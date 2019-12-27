using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using System;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Sop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
