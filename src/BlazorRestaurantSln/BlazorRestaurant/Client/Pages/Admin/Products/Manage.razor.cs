using BlazorRestaurant.Client.Services;
using BlazorRestaurant.Shared.CustomHttpResponses;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Products
{
    [Authorize(Roles = Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.AddProduct)]
    [Route(Constants.AdminPagesRoutes.EditProduct)]
    public partial class Manage
    {
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Parameter]
        public int? ProductId { get; set; }
        private bool IsEdit => ProductId.HasValue;
        private string PageHeader => IsEdit ? "Edit Product" : "Add Product";
        private ProductModel ProductModel { get; set; } = new ProductModel();
        private HttpClient AuthorizedHttpClientService { get; set; }
        private ProductTypeModel[] AllProductTypes { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool ShowSelectImageComponent { get; set; } = false;

        protected async override Task OnInitializedAsync()
        {
            this.AuthorizedHttpClientService = this.HttpClientService.CreateAuthorizedClient();
            this.AllProductTypes = await this.AuthorizedHttpClientService
                .GetFromJsonAsync<ProductTypeModel[]>("api/Product/ListProductTypes");
            if (this.IsEdit)
            {
                this.ProductModel = await this.AuthorizedHttpClientService
                    .GetFromJsonAsync<ProductModel>($"api/Product/GetProductById?productId={this.ProductId}");
            }
            else
            {
                this.ProductModel.ProductTypeId = this.AllProductTypes[0].ProductTypeId;
            }
        }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                IsLoading = true;
                string requestUrl = IsEdit ? "api/Product/EditProduct" : "api/Product/AddProduct";
                var response = await this.AuthorizedHttpClientService
                    .PostAsJsonAsync<ProductModel>(requestUrl, this.ProductModel);
                if (!response.IsSuccessStatusCode)
                {
                    var problemHttpResponse = await response.Content.ReadFromJsonAsync<ProblemHttpResponse>();
                    await this.ToastifyService.DisplayErrorNotification(problemHttpResponse.Detail);
                }
                else
                {
                    string actionMessage = IsEdit ? "edited" : "created";
                    await ToastifyService.DisplaySuccessNotification($"Product '{this.ProductModel.Name}' " +
                        $"has been {actionMessage}");
                    NavigationManager.NavigateTo(Constants.AdminPagesRoutes.ListProducts);

                }
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

        private void OpenSelectImage()
        {
            this.ShowSelectImageComponent = true;
        }

        private void OnImageSelected(string imageUrl)
        {
            this.ProductModel.ImageUrl = imageUrl;
            this.ShowSelectImageComponent = false;
            StateHasChanged();
        }
    }
}
