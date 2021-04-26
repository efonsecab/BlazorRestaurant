using BlazorRestaurant.Client.Configuration;
using BlazorRestaurant.Client.CustomClaims;
using BlazorRestaurant.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient($"{assemblyName}.ServerAPI"));

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomRemoteUserAccount>(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                var scopeUri = builder.Configuration.GetValue<string>("AzureAdB2CScope");
                options.ProviderOptions.DefaultAccessTokenScopes.Add(scopeUri);
                options.ProviderOptions.LoginMode = "redirect";
                options.UserOptions.RoleClaim = "Role";
            }).AddAccountClaimsPrincipalFactory<
                RemoteAuthenticationState, CustomRemoteUserAccount, CustomAccountClaimsPrincipalFactory
                >();

            Configuration.SiteConfiguration siteConfiguration = builder.Configuration.GetSection("SiteConfiguration")
                .Get<SiteConfiguration>();
            siteConfiguration ??= new SiteConfiguration();
            builder.Services.AddSingleton(siteConfiguration);
            builder.Services.AddScoped<ToastifyService>();

            await builder.Build().RunAsync();
        }
    }
}
