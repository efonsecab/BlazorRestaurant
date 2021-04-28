using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Images
{
    [Authorize(Roles = BlazorRestaurant.Shared.Global.Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.ManageImages)]
    public partial class Manage
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private ImageUploadModel ImageUploadModel { get; set; } = new ImageUploadModel();
        private string ErrorMessage { get; set; }
        private bool IsLoading { get; set; }
        private string ImageBase64Url { get; set; }

        protected override void OnInitialized()
        {
            this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
        }

        private async Task OnFileSelectionChangeAsync(InputFileChangeEventArgs e)
        {
            try
            {
                this.IsLoading = true;
                int maxFileSize = 5120000; //5 MB
                var fileStream = e.File.OpenReadStream(maxAllowedSize: maxFileSize);
                using MemoryStream memoryStream = new();
                await fileStream.CopyToAsync(memoryStream);
                this.ImageUploadModel.FileExtension = Path.GetExtension(e.File.Name);
                this.ImageUploadModel.ImageFileBytes = memoryStream.ToArray();
                this.ImageBase64Url = $"data:image/jpg;base64,{Convert.ToBase64String(this.ImageUploadModel.ImageFileBytes)}";
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private async Task OnValidSubmit()
        {
            try
            {
                this.IsLoading = true;
                var response = await this.AuthorizedHttpClient.PostAsJsonAsync<ImageUploadModel>("api/Image/UploadImage",
                    this.ImageUploadModel);
                if (!response.IsSuccessStatusCode)
                {
                    var problemHttpResponse = await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    this.ErrorMessage = problemHttpResponse.Detail;
                }
                else
                {
                    this.ImageUploadModel = new ImageUploadModel();
                    await ToastifyService.DisplaySuccessNotification("Image has been uploaded");
                    this.NavigationManager.NavigateTo(Constants.AdminPagesRoutes.ListImages);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    await ToastifyService.DisplayErrorNotification(this.ErrorMessage);
                }
                this.IsLoading = false;
            }
        }
    }
}
