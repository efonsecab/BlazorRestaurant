using BlazorRestaurant.Common.Interfaces;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.Server.Configuration;
using BlazorRestaurant.Server.CustomLoggers;
using BlazorRestaurant.Server.CustomProviders;
using BlazorRestaurant.Shared.Configuration;
using BlazorRestaurant.Shared.CustomHttpResponses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using PTI.Microservices.Library.Configuration;
using PTI.Microservices.Library.Interceptors;
using PTI.Microservices.Library.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server
{
    /// <summary>
    /// Application Configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Createsa new instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration container
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configured application services
        /// </summary>
        /// <param name="services"></param>

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            GlobalPackageConfiguration.RapidApiKey = Configuration["PTIMicroservicesLibraryConfiguration:RapidApiKey"];

            var azureAdB2CSection = Configuration.GetSection("AzureAdB2C");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(azureAdB2CSection);
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.Events.OnTokenValidated = (context) =>
                    {
                        System.Threading.Thread.CurrentPrincipal = context.Principal;
                        return Task.CompletedTask;
                    };
                    options.SaveToken = true;
                });

            services.AddTransient<ILogger<CustomHttpClientHandler>, CustomHttpClientHandlerLogger>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped(serviceProvide =>
            {
                BlazorRestaurantDbContext blazorRestaurantDbContext = CreateBlazorRestaurantDbContext(serviceProvide);
                return blazorRestaurantDbContext;
            });

            services.AddAutoMapper(configAction =>
            {
                configAction.AddMaps(new[] { typeof(Startup).Assembly });
            });

            SystemConfiguration systemConfiguration = Configuration.GetSection("SystemConfiguration").Get<SystemConfiguration>();
            services.AddSingleton(systemConfiguration);
            ConfigureDataStorage(services);
            ConfigurePTIMicroservicesLibraryDefaults(services);
            ConfigureAzureBlobStorage(services);

            AzureConfiguration azureConfiguration = Configuration.GetSection("AzureConfiguration").Get<AzureConfiguration>();
            services.AddSingleton(azureConfiguration);

            ConfigureAzureMaps(services, azureConfiguration);

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.UseInlineDefinitionsForEnums();
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "BlazorRestaurant.Server.xml");
                if (System.IO.File.Exists(filePath))
                    c.IncludeXmlComments(filePath);
            });
        }

        private BlazorRestaurantDbContext CreateBlazorRestaurantDbContext(IServiceProvider serviceProvider)
        {
            DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder =
                            new();
            BlazorRestaurantDbContext blazorRestaurantDbContext =
            new(dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"),
            sqlServerOptionsAction: (serverOptions) => serverOptions.EnableRetryOnFailure(3)).Options,
            serviceProvider.GetService<ICurrentUserProvider>());
            return blazorRestaurantDbContext;
        }

        private static void ConfigureAzureMaps(IServiceCollection services, AzureConfiguration azureConfiguration)
        {
            services.AddSingleton(azureConfiguration.AzureMapsConfiguration);
            services.AddTransient<AzureMapsService>();
        }

        private void ConfigureDataStorage(IServiceCollection services)
        {
            var dataStorageConfiguration = Configuration.GetSection(nameof(DataStorageConfiguration))
                .Get<DataStorageConfiguration>();
            services.AddSingleton(dataStorageConfiguration);
        }

        private static void ConfigurePTIMicroservicesLibraryDefaults(IServiceCollection services)
        {
            services.AddTransient<CustomHttpClientHandler>();
            services.AddTransient<CustomHttpClient>();
        }

        private void ConfigureAzureBlobStorage(IServiceCollection services)
        {
            var azureBlobStorageConfiguration =
                Configuration.GetSection($"AzureConfiguration:{nameof(AzureBlobStorageConfiguration)}")
                .Get<AzureBlobStorageConfiguration>();
            services.AddSingleton(azureBlobStorageConfiguration);
            services.AddTransient<AzureBlobStorageService>();
        }

        /// <summary>
        /// Configures Application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor Restaurant API");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(cfg =>
            {
                cfg.Run(async context =>
                {
                    var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature.Error;
                    if (error != null)
                    {
                        try
                        {
                            BlazorRestaurantDbContext blazorRestaurantDbContext =
                            this.CreateBlazorRestaurantDbContext(context.RequestServices);
                            await blazorRestaurantDbContext.ErrorLog.AddAsync(new BlazorRestaurant.DataAccess.Models.ErrorLog()
                            {
                                FullException = error.ToString(),
                                StackTrace = error.StackTrace,
                                Message = error.Message
                            });
                            await blazorRestaurantDbContext.SaveChangesAsync();
                        }
                        catch (Exception)
                        {

                        }
                    }
                    ProblemHttpResponse problemHttpResponse = new()
                    {
                        Detail = error.Message,
                    };
                    await context.Response.WriteAsJsonAsync<ProblemHttpResponse>(problemHttpResponse);
                });
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
