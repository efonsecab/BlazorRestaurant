using BlazorRestaurant.Client.Configuration;
using BlazorRestaurant.Client.CustomClaims;
using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Components.Azure.AzureMaps;
using BlazorRestaurant.Shared.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string assemblyName = "BlazorRestaurant";
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddHttpClient($"{assemblyName}.ServerAPI", client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient($"{assemblyName}.ServerAPI.Anonymous", client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient($"{assemblyName}.ServerAPI"));

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient($"{assemblyName}.ServerAPI.Anonymous"));

            await ConfigureAuthenticationAsync(builder);

            Configuration.SiteConfiguration siteConfiguration = builder.Configuration.GetSection("SiteConfiguration")
                .Get<SiteConfiguration>();
            siteConfiguration ??= new SiteConfiguration();
            builder.Services.AddSingleton(siteConfiguration);
            builder.Services.AddScoped<ToastifyService>();
            builder.Services.AddScoped<HttpClientService>();

            builder.Services.AddScoped<AzureMapsControlModule>();

            await builder.Build().RunAsync();
        }

        private static async Task ConfigureAuthenticationAsync(WebAssemblyHostBuilder builder)
        {
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var clientAzureAdB2CConfiguration = await httpClient
                .GetFromJsonAsync<ClientAzureAdB2CConfiguration>("api/Configuration/GetClientAzureAdB2CConfiguration");
            var clientAzureAdB2CScope = await httpClient.GetStringAsync("api/Configuration/GetClientAzureAdB2CScope");
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomRemoteUserAccount>(options =>
            {
                options.ProviderOptions.Authentication = clientAzureAdB2CConfiguration.AzureAdB2C;
                //builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                //var scopeUri = builder.Configuration.GetValue<string>("AzureAdB2CScope");
                var scopeUri = clientAzureAdB2CScope;
                options.ProviderOptions.DefaultAccessTokenScopes.Add(scopeUri);
                options.ProviderOptions.LoginMode = "redirect";
                options.UserOptions.RoleClaim = "Role";
            }).AddAccountClaimsPrincipalFactory<
                RemoteAuthenticationState, CustomRemoteUserAccount, CustomAccountClaimsPrincipalFactory>();
        }
    }
}
