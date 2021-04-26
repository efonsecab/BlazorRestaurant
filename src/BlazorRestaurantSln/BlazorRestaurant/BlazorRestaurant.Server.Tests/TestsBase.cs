using BlazorRestaurant.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Tests
{
    public abstract class TestsBase
    {
        private TestServer Server { get; }
        protected string TestImageFilePath { get; }
        internal static string AzureBlobStorageConnectionString { get; set; }
        internal static string TestImagesContainerName { get; set; }
        private static IConfiguration Configuration { get; set; }

        public TestsBase()
        {
            ConfigurationBuilder configurationBuilder =
                new();
            configurationBuilder.AddJsonFile("appsettings.json")
                .AddUserSecrets("b59b6219-c1e3-49f7-bf68-a0b2c3ea122b");
            IConfiguration configuration = configurationBuilder.Build();
            Configuration = configuration;
            Server = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>());

            string appDirectory = AppContext.BaseDirectory;
            string testImageFilePath = Path.Combine(appDirectory, @"ResourceFiles\Images\thumbnail_IMG_2501.jpg");
            this.TestImageFilePath = testImageFilePath;
            AzureBlobStorageConnectionString = Configuration["AzureConfiguration:AzureBlobStorageConfiguration:ConnectionString"];
            TestImagesContainerName = Configuration["DataStorageConfiguration:ImagesContainerName"];
        }

        public static BlazorRestaurantDbContext CreateDbContext()
        {
            var connectionString = Configuration.GetConnectionString("Default");
            DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder =
            new();
            BlazorRestaurantDbContext blazorRestaurantDbContext =
            new(dbContextOptionsBuilder.UseSqlServer(connectionString).Options);
            return blazorRestaurantDbContext;
        }

        protected HttpClient CreateAuthorizedClientAsync()
        {
            var client = this.Server.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Configuration["TestUserAccessToken"]);
            return client;
        }

    }
}
