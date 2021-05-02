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

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class ProductControllerTests: TestsBase
    {
        private static readonly ProductModel TestProductModel = new()
        {
            Name = "AUTOMATED TEST PRODUCT",
            Description = "TEST DESCRIPTION",
            ProductTypeId = 1
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
            var authorizedHttpClient = base.CreateAuthorizedClientAsync();
            var response = await authorizedHttpClient.PostAsJsonAsync("api/Product/AddProduct", TestProductModel);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }
    }
}