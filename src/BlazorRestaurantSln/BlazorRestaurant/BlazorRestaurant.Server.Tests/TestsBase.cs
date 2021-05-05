using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.Server.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTI.Microservices.Library.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Tests
{
    public abstract class TestsBase
    {
        private TestServer Server { get; }
        protected IMapper Mapper { get; }
        internal static IHttpContextAccessor HttpContextAccesor { get; set; }
        internal static string TestImageFilePath { get; set; }
        internal static DataStorageConfiguration DataStorageConfiguration { get; set; }
        internal static AzureBlobStorageConfiguration AzureBlobStorageConfiguration { get; set; }
        private TestAzureAdB2CAuthConfiguration TestAzureAdB2CAuthConfiguration { get; }
        private static IConfiguration Configuration { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }

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
            TestImageFilePath = testImageFilePath;
            DataStorageConfiguration = Server.Services.GetRequiredService<DataStorageConfiguration>();
            AzureBlobStorageConfiguration = Server.Services.GetRequiredService<AzureBlobStorageConfiguration>();
            this.TestAzureAdB2CAuthConfiguration = Configuration.GetSection("TestAzureAdB2CAuthConfiguration").Get<TestAzureAdB2CAuthConfiguration>();
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

        protected async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            if (this.AuthorizedHttpClient != null)
                return this.AuthorizedHttpClient;
            HttpClient httpClient = new HttpClient();
            List<KeyValuePair<string?, string?>> formData = new List<KeyValuePair<string?, string?>>();
            formData.Add(new KeyValuePair<string?, string?>("username", this.TestAzureAdB2CAuthConfiguration.Username));
            formData.Add(new KeyValuePair<string?, string?>("password", this.TestAzureAdB2CAuthConfiguration.Password));
            formData.Add(new KeyValuePair<string?, string?>("grant_type", "password"));
            string applicationId = this.TestAzureAdB2CAuthConfiguration.ApplicationId;
            formData.Add(new KeyValuePair<string?, string?>("scope", $"openid {applicationId} offline_access"));
            formData.Add(new KeyValuePair<string?, string?>("client_id", applicationId));
            formData.Add(new KeyValuePair<string?, string?>("response_type", "token id_token"));
            //password=Passxword1&grant_type=password&scope=openid+bef22d56-552f-4a5b-b90a-1988a7d634ce+offline_access&client_id=bef22d56-552f-4a5b-b90a-1988a7d634ce&response_type=token+id_token

            System.Net.Http.FormUrlEncodedContent form = new FormUrlEncodedContent(formData);
            var response = await httpClient.PostAsync(this.TestAzureAdB2CAuthConfiguration.TokenUrl, form);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                var client = this.Server.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.access_token);
                this.AuthorizedHttpClient = client;
                return client;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

    }


    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }

    public class TestAzureAdB2CAuthConfiguration
    {
        public string TokenUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
    }


}
