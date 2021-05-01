using BlazorRestaurant.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server
{
    /// <summary>
    /// Application Entry class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application Entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Application Initializer
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                   {
                       IConfigurationRoot configurationRoot = configurationBuilder.Build();

                       var defaultConnectionString = configurationRoot.GetConnectionString("Default");
                       DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder = new();

                       using BlazorRestaurantDbContext blazorRestaurantDbContext =
                       new(dbContextOptionsBuilder.UseSqlServer(defaultConnectionString,
                       sqlServerOptionsAction: (serverOptions) => serverOptions.EnableRetryOnFailure(3)).Options);

                       var systemStartConfig = blazorRestaurantDbContext.SystemConfiguration.Where(p => p.Name == "ServerStartConfiguration")
                       .Select(p => p.Value).Single();

                       var systemConfigBytes = System.Text.Encoding.UTF8.GetBytes(systemStartConfig);
                       MemoryStream systemConfigStream = new(systemConfigBytes);
                       configurationBuilder.AddJsonStream(systemConfigStream);
                   });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
