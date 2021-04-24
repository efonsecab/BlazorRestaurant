using BlazorRestaurant.Client.Extensions;
using BlazorRestaurant.Shared.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        private HttpClient HttpClient { get; set; }
        public async void OnLogInSucceeded()
        {
            var authState = await AuthenticationStateTask;
            UserModel userModel = new UserModel()
            {
                EmailAddress = authState.User.Claims.GetUserEmails()[0],
                FullName = authState.User.Claims.GetDisplayName(),
                AzureAdB2cobjectId = Guid.Parse(authState.User.Claims.GetAzureAdB2CUserObjectId())
            };
            var response = await this.HttpClient.PostAsJsonAsync<UserModel>("api/User/UserLoggedIn", userModel);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }


        }
    }
}
