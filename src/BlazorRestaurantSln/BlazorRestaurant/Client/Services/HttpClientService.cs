using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Services
{
    public class HttpClientService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            this.HttpClientFactory = httpClientFactory;
        }

        public HttpClient CreateAnonymousClient()
        {
            string assemblyName = "BlazorRestaurant";
            return this.HttpClientFactory.CreateClient($"{assemblyName}.ServerAPI.Anonymous");
        }

        public HttpClient CreateAuthorizedClient()
        {
            string assemblyName = "BlazorRestaurant";
            return this.HttpClientFactory.CreateClient($"{assemblyName}.ServerAPI");
        }
    }
}
