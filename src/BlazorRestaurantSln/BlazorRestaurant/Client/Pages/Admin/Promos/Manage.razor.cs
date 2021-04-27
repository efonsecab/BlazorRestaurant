using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Images;
using BlazorRestaurant.Shared.Promos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Promos
{
    [Authorize(Roles = Constants.Roles.Admin)]
    public partial class Manage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Parameter]
        public Guid? PromoId { get; set; }

        private bool IsEdit => PromoId.HasValue;
        private string Title => IsEdit ? "Add Promo" : "Edit Promo";

        private PromotionModel PromotionModel { get; set; } = new PromotionModel();
        private string ErrorMessage { get; set; }

        private bool ShowSelectImageComponent { get; set; } = false;

        private void OpenSelectImage()
        {
            this.ShowSelectImageComponent = true;
        }

        private void OnImageSelected(string imageUrl)
        {
            this.PromotionModel.ImageUrl = imageUrl;
            this.ShowSelectImageComponent = false;
            StateHasChanged();
        }

        private async Task OnValidSubmit()
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/Promotion/AddPromotion", this.PromotionModel);
                if (!response.IsSuccessStatusCode)
                {
                    var problemHttpResponse = await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    await this.ToastifyService.DisplayErrorNotification(problemHttpResponse.Detail);
                }
                else
                {
                    await this.ToastifyService.DisplaySuccessNotification("Promotion has been added");
                    NavigationManager.NavigateTo("Admin/Promos/List");
                }
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
        }
    }
}
