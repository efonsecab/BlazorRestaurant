using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Components.Azure.AzureMaps;
using BlazorRestaurant.Shared.AzureMaps;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Orders;
using BlazorRestaurant.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.User.Orders
{
    [Authorize(Roles = Constants.Roles.User)]
    [Route(Constants.UserPagesRoutes.AddOrder)]
    [Route(Constants.UserPagesRoutes.EditOrder)]
    public partial class Manage
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Parameter]
        public long? OrderId { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool CanRenderMap { get; set; }
        private ProductModel[] AllProducts { get; set; }
        private string AzureMapsKey { get; set; }

        private AzureMapsDataPoint SelectedPOI { get; set; }
        private OrderModel OrderModel { get; set; } = new OrderModel()
        {
            OrderDetail = new List<OrderDetailModel>()
        };
        protected async override Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AzureMapsKey = await this.AuthorizedHttpClient.GetStringAsync("api/Configuration/GetAzureMapsKey");
                this.CanRenderMap = true;
                this.AllProducts = await this.AuthorizedHttpClient
                    .GetFromJsonAsync<ProductModel[]>("api/Product/ListProducts");
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<AzureMapsDataPoint[]> SearchPOI(string searchTerm, string countryCode)
        {
            try
            {
                IsLoading = true;
                StateHasChanged();
                var foundPOIs = await this.AuthorizedHttpClient.GetFromJsonAsync<PointOfInterestModel[]>(
                    $"api/AzureMaps/SearchPointsOfInterest" +
                    $"?searchTerm={searchTerm}" +
                    $"&countryCode={countryCode}");
                return foundPOIs.Select(p => new AzureMapsDataPoint()
                {
                    Name = p.Name,
                    Country = p.Country,
                    FreeFormAddress = p.FreeFormAddress,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                }).ToArray();
            }
            catch (Exception ex)
            {
                await this.ToastifyService.DisplayErrorNotification(ex.Message);
                return null;
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private void OnPOISelected(AzureMapsDataPoint selectedPOI)
        {
            this.SelectedPOI = selectedPOI;
            this.OrderModel.DestinationFreeFormAddress = selectedPOI.FreeFormAddress;
            this.OrderModel.DestinationLatitude = selectedPOI.Latitude;
            this.OrderModel.DestinationLongitude = selectedPOI.Longitude;
            StateHasChanged();
        }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                IsLoading = true;
                string requestUrl = "api/Order/AddOrder";
                var response = await this.AuthorizedHttpClient.PostAsJsonAsync<OrderModel>(requestUrl, this.OrderModel);
                if (!response.IsSuccessStatusCode)
                {
                    var problemHttpResponse = await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    await this.ToastifyService.DisplayErrorNotification(problemHttpResponse.Detail);
                }
                else
                {
                    await this.ToastifyService.DisplaySuccessNotification("Your order has been added");
                    this.NavigationManager.NavigateTo(Constants.UserPagesRoutes.ListOrders);
                    
                }
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
        private void AddProductLine()
        {
            this.OrderModel.OrderDetail.Add(new OrderDetailModel()
            {
                ProductId = this.AllProducts[0].ProductId,
                Product = this.AllProducts[0]
            });
        }
    }
}
