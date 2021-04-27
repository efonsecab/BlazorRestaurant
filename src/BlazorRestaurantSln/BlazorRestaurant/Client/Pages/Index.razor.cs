using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Promos;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        public HttpClient AnonymousHttpClient { get; private set; }
        private PromotionModel[] AllPromotions { get;  set; }
        private int TotalPages => this.AllPromotions == null?0: (int)Math.Ceiling( (double)this.AllPromotions.Length / 2);
        protected async override Task OnInitializedAsync()
        {
            try
            {
                this.AnonymousHttpClient = this.HttpClientService.CreatedAnonymousClient();
                this.AllPromotions = await AnonymousHttpClient.GetFromJsonAsync<PromotionModel[]>("api/Promotion/ListPromotions");
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {

            }
        }
    }
}
