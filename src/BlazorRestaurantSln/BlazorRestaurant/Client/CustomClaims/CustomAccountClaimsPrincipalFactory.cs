using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.CustomClaims
{
    public class CustomAccountClaimsPrincipalFactory: AccountClaimsPrincipalFactory<CustomRemoteUserAccount>
    {
        private IAccessTokenProviderAccessor Accessor { get; }
        private IHttpClientFactory HttpClientFactory { get; }

        public CustomAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor,
            IHttpClientFactory httpClientFactory) :base(accessor)
        {
            this.Accessor = accessor;
            this.HttpClientFactory = httpClientFactory;
        }

        public override ValueTask<ClaimsPrincipal> CreateUserAsync(CustomRemoteUserAccount account, 
            RemoteAuthenticationUserOptions options)
        {
            var httpClient = this.HttpClientFactory.CreateClient();
            return base.CreateUserAsync(account, options);
        }
    }
}
