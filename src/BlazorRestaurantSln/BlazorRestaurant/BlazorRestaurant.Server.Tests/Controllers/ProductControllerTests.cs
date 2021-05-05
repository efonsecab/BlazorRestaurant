using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.Shared.Products;
using System.Net.Http.Json;
using BlazorRestaurant.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BlazorRestaurant.DataAccess.Models;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class ProductControllerTests : TestsBase
    {
        private static readonly ProductModel TestProductModel = new()
        {
            Name = "AUTOMATED TEST PRODUCT",
            Description = "TEST DESCRIPTION",
            ProductTypeId = 1,
            ImageUrl = "https://localhost/test.jpg"
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testEntity = await blazorRestaurantDbContext.Product.Where(p => p.Name == TestProductModel.Name).SingleAsync();
            blazorRestaurantDbContext.Product.Remove(testEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
        }

        [TestMethod()]
        public async Task AddProductTest()
        {
            var authorizedHttpClient = await base .CreateAuthorizedClientAsync();
            var response = await authorizedHttpClient.PostAsJsonAsync("api/Product/AddProduct", TestProductModel);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }

        [TestMethod()]
        public async Task ListProductTypesTestAsync()
        {
            var authorizedHttpClient = await base .CreateAuthorizedClientAsync();
            var allProductTypes = await authorizedHttpClient
                .GetFromJsonAsync<ProductTypeModel[]>("api/Product/ListProductTypes");
            Assert.IsNotNull(allProductTypes);
            Assert.IsTrue(allProductTypes.Length > 0);
        }

        [TestMethod()]
        public async Task ListProductsTestAsync()
        {
            var authorizedHttpClient = await base .CreateAuthorizedClientAsync();
            var allProducts = await authorizedHttpClient
                .GetFromJsonAsync<ProductModel[]>("api/Product/ListProducts");
            Assert.IsNotNull(allProducts);
            Assert.IsTrue(allProducts.Length > 0);
        }

        [TestMethod()]
        public async Task DeleteProductTestAsync()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var productEntity = base.Mapper.Map<ProductModel, Product>(TestProductModel);
            await blazorRestaurantDbContext.Product.AddAsync(productEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync();
            var response = await authorizedHttpClient.DeleteAsync($"api/Product/DeleteProduct?productId={productEntity.ProductId}");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }

        }
    }
}