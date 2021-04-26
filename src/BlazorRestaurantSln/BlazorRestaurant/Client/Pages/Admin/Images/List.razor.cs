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
        private HttpClient HttpClient { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private ImageModel[] AllImages { get; set; }


        protected async override Task OnInitializedAsync()
        {
            try
            {
                this.AllImages = await this.HttpClient.GetFromJsonAsync<ImageModel[]>("api/Image/ListImages");
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
