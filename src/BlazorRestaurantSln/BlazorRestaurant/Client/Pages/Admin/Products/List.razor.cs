using BlazorRestaurant.Client.Services;
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
    [Route(Constants.AdminPagesRoutes.ListProducts)]
    public partial class List
    {
        [Inject]
        private HttpClientService HttpClientService { get; set; }
        [Inject]
        private ToastifyService ToastifyService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private HttpClient AuthorizedHttpClient { get; set; }
        private ProductModel[] AllProducts { get;  set; }
        private int TotalPages => this.AllProducts == null ? 0 : (int)Math.Ceiling((double)this.AllProducts.Length / 2);
        private bool IsLoading { get; set; } = false;
        private bool ShouldShowConfirmDeleteModal { get; set; }
        private ProductModel SelectedProductModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                IsLoading = true;
                this.AuthorizedHttpClient = this.HttpClientService.CreateAuthorizedClient();
                this.AllProducts = await this.AuthorizedHttpClient.GetFromJsonAsync<ProductModel[]>("api/Product/ListProducts");
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

        private void ShowConfirmDeleteModal(ProductModel selectedProductModel)
        {
            this.ShouldShowConfirmDeleteModal = true;
            this.SelectedProductModel = selectedProductModel;
            StateHasChanged();
        }

        private void EditProduct(ProductModel selectedProductModel)
        {
            this.NavigationManager.NavigateTo($"{Constants.AdminPagesRoutes.AddProduct}/{selectedProductModel.ProductTypeId}");
        }
    }
}
