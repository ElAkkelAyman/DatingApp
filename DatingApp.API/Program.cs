using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using DatingApp.API.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models.Data;
using DatingApp.API.Data;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try 
                {
                   var context = services.GetRequiredService<DataContext>();
                   context.Database.Migrate();
                   SeedUsers.Seedusers(context);  
                }
                catch(Exception ex)
                {
                     var logger = services.GetRequiredService<ILogger<Program>>();
                     logger.LogError(ex,"An error occured during migration");
                }
            }
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
