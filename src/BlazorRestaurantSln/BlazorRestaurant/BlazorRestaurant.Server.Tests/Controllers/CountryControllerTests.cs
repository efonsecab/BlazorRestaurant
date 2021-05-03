using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class CountryControllerTests : TestsBase
    {
        private static readonly Country TestCountryModel = new()
        {
            Name = "COSTA RICA",
            Isocode = "CRC"
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testEntity = await blazorRestaurantDbContext.Country.Where(p => p.Name == TestCountryModel.Name).SingleAsync();
            blazorRestaurantDbContext.Country.Remove(testEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
        }


        [TestMethod()]
        public async Task ListCountriesTest()
        {
            var authorizedHttpClient = base.CreateAuthorizedClientAsync();
            var allCountries = await authorizedHttpClient
                .GetFromJsonAsync<Country[]>("api/Country/ListCountries=CostaRica");
            Assert.IsNotNull(allCountries);
            Assert.IsTrue(allCountries.Length > 0);
        }
    }
}