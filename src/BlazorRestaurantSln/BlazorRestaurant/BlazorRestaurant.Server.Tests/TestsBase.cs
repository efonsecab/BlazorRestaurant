using BlazorRestaurant.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Tests
{
    public abstract class TestsBase
    {
        private TestServer Server { get; }
        private ServiceCollection Services { get; set; }
        private static IConfiguration Configuration { get; set; }

        public TestsBase()
        {
            ConfigurationBuilder configurationBuilder =
                new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json")
                .AddUserSecrets("b59b6219-c1e3-49f7-bf68-a0b2c3ea122b");
            IConfiguration configuration = configurationBuilder.Build();
            Configuration = configuration;
            Server = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>());
            this.Services = new ServiceCollection();
        }

        public static BlazorRestaurantDbContext CreateDbContext()
        {
            var connectionString = Configuration.GetConnectionString("Default");
            DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder =
            new DbContextOptionsBuilder<BlazorRestaurantDbContext>();
            BlazorRestaurantDbContext blazorRestaurantDbContext =
            new BlazorRestaurantDbContext(dbContextOptionsBuilder.UseSqlServer(connectionString).Options);
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
