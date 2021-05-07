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
using BlazorRestaurant.Shared.Countries;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class CountryControllerTests : TestsBase
    {
        private static readonly CountryModel TestCountryModel = new()
        {
            Name = "TEST COUNTRY",
            Isocode = "TST"
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
            var countryEntity = base.Mapper.Map<CountryModel, Country>(TestCountryModel);
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            await blazorRestaurantDbContext.Country.AddAsync(countryEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
            var authorizedHttpClient = await base.CreateAuthorizedClientAsync(Role.User);
            var allCountries = await authorizedHttpClient
                .GetFromJsonAsync<Country[]>($"api/Country/ListCountries?searchTerm={countryEntity.Name}");
            Assert.IsNotNull(allCountries);
            Assert.IsTrue(allCountries.Length > 0);
        }
    }
}