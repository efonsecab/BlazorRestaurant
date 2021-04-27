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

        public HttpClient CreatedAnonymousClient()
        {
            string assemblyName = "BlazorRestaurant";
            return this.HttpClientFactory.CreateClient($"{assemblyName}.ServerAPI.Anonymous");
        }

        public HttpClient CreatedAuthorizedClient()
        {
            string assemblyName = "BlazorRestaurant";
            return this.HttpClientFactory.CreateClient($"{assemblyName}.ServerAPI");
        }
    }
}
