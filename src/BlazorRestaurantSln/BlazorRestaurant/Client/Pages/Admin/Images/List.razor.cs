using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Images
{
    [Authorize(Roles = BlazorRestaurant.Shared.Global.Constants.Roles.Admin)]
    [Route(BlazorRestaurant.Shared.Global.Constants.AdminPagesRoutes.ListImages)]
    public partial class List
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private ImageModel[] AllImages { get; set; }
        private bool IsLoading { get; set; } = false;

        private bool ShouldShowConfirmDeleteModal { get; set; } = false;
        private ImageModel SelectedImageModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadImages();
        }

        private async Task LoadImages()
        {
            try
            {
                this.IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AllImages = await this.AuthorizedHttpClient.GetFromJsonAsync<ImageModel[]>("api/Image/ListImages");
            }
            catch (Exception ex)
            {
                await ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void ShowConfirmDeleteDialog(ImageModel selectedImageModel)
        {
            this.ShouldShowConfirmDeleteModal = true;
            this.SelectedImageModel = selectedImageModel;
        }

        private void HideConfirmDeleteDialog()
        {
            this.ShouldShowConfirmDeleteModal = false;
            this.SelectedImageModel = null;
        }

        private async Task DeleteSelectedImage()
        {
            try
            {
                this.IsLoading = true;
                StateHasChanged();
                string requestUrl = $"api/Image/DeleteImage?imageName={this.SelectedImageModel.ImageName}";
                var response = await this.AuthorizedHttpClient.DeleteAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    await this.ToastifyService.DisplaySuccessNotification("Image has been deleted");
                }
                else
                {
                    ProblemHttpResponse problemHttpResponse = await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    await this.ToastifyService.DisplayErrorNotification(problemHttpResponse.Detail);
                }
            }
            catch (Exception ex)
            {
                await this.ToastifyService.DisplayErrorNotification(ex.Message);
            }
            finally
            {
                this.IsLoading = false;
                this.SelectedImageModel = null;
                this.HideConfirmDeleteDialog();
                await this.LoadImages();
                StateHasChanged();
            }
        }
    }
}
