using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        protected IMapper Mapper { get; }
        internal static IHttpContextAccessor HttpContextAccesor { get; set; }
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
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
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
                })
                .UseConfiguration(configuration)
                .UseStartup<Startup>());
            this.Mapper = Server.Services.GetRequiredService<IMapper>();
            HttpContextAccesor = Server.Services.GetRequiredService<IHttpContextAccessor>();
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
            new(dbContextOptionsBuilder.UseSqlServer(connectionString).Options, 
            new CustomProviders.CurrentUserProvider(HttpContextAccesor));
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
