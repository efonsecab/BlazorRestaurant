using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    public partial class Manage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private ImageUploadModel ImageUploadModel { get; set; } = new ImageUploadModel();
        private string ErrorMessage { get; set; }

        private async Task OnFileSelectionChangeAsync(InputFileChangeEventArgs e)
        {
            try
            {
                int maxFileSize = 5120000; //5 MB
                var fileStream = e.File.OpenReadStream(maxAllowedSize: maxFileSize);
                using MemoryStream memoryStream = new();
                await fileStream.CopyToAsync(memoryStream);
                this.ImageUploadModel.FileExtension = Path.GetExtension(e.File.Name);
                this.ImageUploadModel.ImageFileBytes = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        private async Task OnValidSubmit()
        {
            try
            {
                var response = await this.HttpClient.PostAsJsonAsync<ImageUploadModel>("api/Image/UploadImage",
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
            }
        }
    }
}
