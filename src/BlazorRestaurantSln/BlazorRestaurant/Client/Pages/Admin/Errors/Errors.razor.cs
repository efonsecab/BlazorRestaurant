using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Errors;
using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Errors
{
    [Authorize(Roles = Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.ErrorLog)]
    public partial class Errors
    {
        public ErrorLogModel[] AllErrors { get; private set; }
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        private bool IsLoading { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }

        protected  async override Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                string requestUrl = "api/Error/ListErrors";
                this.AllErrors= await this.AuthorizedHttpClient.GetFromJsonAsync<ErrorLogModel[]>(requestUrl);
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
    }
}
