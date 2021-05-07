using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.Shared.Profile;
using System.Net.Http.Json;
using BlazorRestaurant.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class LocationControllerTests: TestsBase
    {
        private static LocationModel TestLocation = new LocationModel()
        {
            Name = "TEST LOCATION",
            Description = "TEST DESC",
            FreeFormAddress = "TEST ADDRESS",
            ImageUrl = "TEST URL"
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testEntity = await blazorRestaurantDbContext.Location.Where(p => p.Name == TestLocation.Name).SingleAsync();
            blazorRestaurantDbContext.Location.Remove(testEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
        }

        [TestMethod()]
        public async Task AddLocationTestAsync()
        {
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync(Role.Admin);
            var response = await authorizedHttpClient.PostAsJsonAsync<LocationModel>("api/Location/AddLocation", TestLocation);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }
    }
}