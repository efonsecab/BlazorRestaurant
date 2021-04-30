using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Errors
{
    [Authorize(Roles = Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.ErrorsLogPowerBI)]
    public partial class ErrorsPowerBI
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        public HttpClient AuthorizedHttpClient { get; private set; }
        public string ReportUrl { get; private set; }

        protected async override Task OnInitializedAsync()
        {
            this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
            try
            {
                string requestUrl = "api/Configuration/GetErrorLogPowerBIUrl";
                this.ReportUrl = await this.AuthorizedHttpClient.GetStringAsync(requestUrl);
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
