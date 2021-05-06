using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static BlazorRestaurant.Shared.Global.Constants;

namespace BlazorRestaurant.Client.Pages.User.Orders
{
    [Authorize(Roles = Constants.Roles.User)]
    [Route(Constants.UserPagesRoutes.ListOrders)]
    public partial class List
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private bool IsLoading { get; set; } = false;
        private HttpClient AuthorizedHttpClient { get; set; }
        private OrderModel[] AllOrders { get; set; }
        private int TotalPages => this.AllOrders == null ? 0 : (int)Math.Ceiling((double)this.AllOrders.Length / 2);

        protected async override Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AllOrders = await this.AuthorizedHttpClient.GetFromJsonAsync<OrderModel[]>("api/Order/ListOwnedOrders");
            }
            catch (Exception ex)
            {
                await this.ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
