using BlazorRestaurant.Client.Extensions;
using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages
{
    public partial class Authentication
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private bool IsProcessingOnLogInSucceeded { get; set; } = false;

        public async Task OnLogInSucceeded(RemoteAuthenticationState remoteState)
        {
            ///Workaround to avoid the framwework bug of invoking this up to 3 times
            var authState = await AuthenticationStateTask;
            UserModel userModel = new UserModel()
            {
                EmailAddress = authState.User.Claims.GetUserEmails()[0],
                FullName = authState.User.Claims.GetDisplayName(),
                AzureAdB2cobjectId = Guid.Parse(authState.User.Claims.GetAzureAdB2CUserObjectId())
            };
            var authorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
            var response = await authorizedHttpClient.PostAsJsonAsync<UserModel>("api/User/UserLoggedIn", userModel);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
            else
            {
                var role = authState.User.Claims.SingleOrDefault(p => p.Type == "Role").Value;
                switch (role)
                {
                    case Constants.Roles.Admin:
                        remoteState.ReturnUrl = Constants.AdminPagesRoutes.AdminIndex;
                        break;
                    case Constants.Roles.User:
                        remoteState.ReturnUrl = Constants.UserPagesRoutes.UserIndex;
                        break;
                }
            }
        }

        private void OnLogOutSucceeded()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
