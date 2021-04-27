using BlazorRestaurant.Client.Services;
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
    public  partial class List
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private ImageModel[] AllImages { get; set; }


        protected async override Task OnInitializedAsync()
        {
            try
            {
                this.AuthorizedHttpClient = this.HttpClientService.CreatedAuthorizedClient();
                this.AllImages = await this.AuthorizedHttpClient.GetFromJsonAsync<ImageModel[]>("api/Image/ListImages");
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
