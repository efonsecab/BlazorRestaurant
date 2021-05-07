using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using BlazorRestaurant.Shared.Orders;
using BlazorRestaurant.DataAccess.Data;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class OrderControllerTests : TestsBase
    {
        private static Product TestProduct = new Product()
        {
            Name = "TEST PRODUCT",
            Description = "TEST DESC",
            ImageUrl = "TEST URL",
            ProductTypeId = 1,
            UnitPrice = 1.99M
        };

        private static Order TestOrder { get; set; }
        private static BlazorRestaurantDbContext BlazorRestaurantDbContext { get; set; }

        [ClassCleanup]
        public static async Task CleanTests()
        {
            BlazorRestaurantDbContext.OrderDetail.RemoveRange(TestOrder.OrderDetail);
            BlazorRestaurantDbContext.Order.Remove(TestOrder);
            BlazorRestaurantDbContext.Product.Remove(TestProduct);
            await BlazorRestaurantDbContext.SaveChangesAsync();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            BlazorRestaurantDbContext = CreateDbContext();
            await BlazorRestaurantDbContext.Product.AddAsync(TestProduct);
            await BlazorRestaurantDbContext.SaveChangesAsync();
            var testuserEntity = await BlazorRestaurantDbContext.ApplicationUser
                .Where(p => p.AzureAdB2cobjectId.ToString() == TestsBase.TestAzureAdB2CAuthConfiguration.AzureAdUserObjectId)
                .SingleAsync();
            TestOrder = new Order()
            {
                ApplicationUserId = testuserEntity.ApplicationUserId,
                DestinationFreeFormAddress = "TEST DESTINATION",
                OrderDetail = new OrderDetail[]
                {
                    new OrderDetail()
                    {
                        ProductId = TestProduct.ProductId,
                        ProductQty = 1,
                    }
                }
            };
            await BlazorRestaurantDbContext.Order.AddAsync(TestOrder);
            await BlazorRestaurantDbContext.SaveChangesAsync();
        }

        [TestMethod()]
        public async Task ListOrdersTestAsync()
        {
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync(Role.Admin);
            var result = await authorizedHttpClient.GetFromJsonAsync<OrderModel[]>("api/Order/ListOrders");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod()]
        public async Task ListOwnedOrdersTestAsync()
        {
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync(Role.User);
            var result = await authorizedHttpClient.GetFromJsonAsync<OrderModel[]>("api/Order/ListOwnedOrders");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}