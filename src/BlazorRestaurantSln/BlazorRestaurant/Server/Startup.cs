using BlazorRestaurant.DataAccess.Data;
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
using Microsoft.Identity.Web;
using System;
using System.Linq;

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
            services.AddDbContext<BlazorRestaurantDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.NameClaimType = "name";
                });
            services.AddAutoMapper(configAction =>
            {
                configAction.AddMaps(new[] { typeof(Startup).Assembly });
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        /// <summary>
        /// Configures Application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                            var connectionString = Configuration.GetConnectionString("Default");
                            DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder =
                            new DbContextOptionsBuilder<BlazorRestaurantDbContext>();
                            BlazorRestaurantDbContext blazorRestaurantDbContext =
                            new BlazorRestaurantDbContext(dbContextOptionsBuilder.UseSqlServer(connectionString).Options);
                            await blazorRestaurantDbContext.ErrorLog.AddAsync(new BlazorRestaurant.DataAccess.Models.ErrorLog()
                            {
                                CreatedAt = DateTimeOffset.UtcNow,
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
                    await context.Response.WriteAsync(error.Message);
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
