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

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class OrderControllerTests : TestsBase
    {
        [TestMethod()]
        public async Task ListOrdersTestAsync()
        {
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync();
            var result = await authorizedHttpClient.GetFromJsonAsync<OrderModel[]>("api/Order/ListOrders");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}