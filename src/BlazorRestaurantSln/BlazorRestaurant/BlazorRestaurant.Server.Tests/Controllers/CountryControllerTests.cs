using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.DataAccess.Data;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass()]
    public class CountryControllerTests
    {
        private static readonly CountryController TestProductModel = new()
        {
            Name = "AUTOMATED TEST PRODUCT",
            Description = "TEST DESCRIPTION",
            ProductTypeId = 1
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testEntity = await blazorRestaurantDbContext.Country.Where(p => p.Name == TestProductModel.Name).SingleAsync();
            blazorRestaurantDbContext.Product.Remove(testEntity);
            await blazorRestaurantDbContext.SaveChangesAsync();
        }


        [TestMethod()]
        public void ListCountriesTest()
        {
            Assert.Fail();
        }
    }
}