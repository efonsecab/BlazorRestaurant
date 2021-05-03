using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Components.Azure.AzureMaps;
using BlazorRestaurant.Shared.AzureMaps;
using BlazorRestaurant.Shared.Global;
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
        [Parameter]
        public long? OrderId { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool CanRenderMap { get; set; }
        private string AzureMapsKey { get; set; }

        private AzureMapsDataPoint SelectedPOI { get; set; }
        protected async override Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AzureMapsKey = await this.AuthorizedHttpClient.GetStringAsync("api/Configuration/GetAzureMapsKey");
                this.CanRenderMap = true;
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
        }
    }
}
