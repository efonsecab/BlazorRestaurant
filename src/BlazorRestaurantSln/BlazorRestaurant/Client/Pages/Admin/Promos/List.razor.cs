using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Global;
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
    [Authorize(Roles = BlazorRestaurant.Shared.Global.Constants.Roles.Admin)]
    [Route(BlazorRestaurant.Shared.Global.Constants.AdminPagesRoutes.ListPromos)]
    public partial class List
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private PromotionModel[] AllPromotions { get; set; }
        private int TotalPages => this.AllPromotions == null ? 0 : (int)Math.Ceiling((double)this.AllPromotions.Length / 2);
        private bool IsLoading { get; set; } = false;
        private HttpClient AuthorizedHttpClient { get; set; }
        private bool ShouldShowConfirmDeleteModal { get; set; } = false;
        private PromotionModel SelectedPromotionModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadPromotions();
        }

        private async Task LoadPromotions()
        {
            try
            {
                this.IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AllPromotions = await AuthorizedHttpClient.GetFromJsonAsync<PromotionModel[]>("api/Promotion/ListPromotions");
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

        private void ShowConfirmDeleteModal(PromotionModel selectedPromotionModel)
        {
            this.ShouldShowConfirmDeleteModal = true;
            this.SelectedPromotionModel = selectedPromotionModel;
            StateHasChanged();
        }

        private void HideConfirmDeleteDialog()
        {
            this.ShouldShowConfirmDeleteModal = false;
            this.SelectedPromotionModel = null;
        }

        private async Task DeleteSelectedPromotion()
        {
            try
            {
                StateHasChanged();
                IsLoading = true;
                string requestUrl = $"api/Promotion/DeletePromotion?promotionId={this.SelectedPromotionModel.PromotionId}";
                var response = await this.AuthorizedHttpClient.DeleteAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ProblemHttpResponse problemHttpResponse =
                        await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    await ToastifyService.DisplayErrorNotification(problemHttpResponse.Detail);
                }
                else
                {
                    await ToastifyService.DisplaySuccessNotification("Promotions has been deleted");
                    NavigationManager.NavigateTo(Constants.AdminPagesRoutes.ListPromos);
                }
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {
                IsLoading = false;
                this.SelectedPromotionModel = null;
                this.HideConfirmDeleteDialog();
                await this.LoadPromotions();
                StateHasChanged();
            }
        }
    }
}
