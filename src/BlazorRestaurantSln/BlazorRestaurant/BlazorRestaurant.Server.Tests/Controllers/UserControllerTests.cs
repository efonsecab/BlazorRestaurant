using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using BlazorRestaurant.Shared.User;
using System.Net.Http.Json;
using BlazorRestaurant.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass]
    public class UserControllerTests : TestsBase
    {
        private static readonly UserModel TestUserModel = new()
        {
            AzureAdB2cobjectId = Guid.NewGuid(),
            EmailAddress = "test@test.test",
            FullName = "AUTOMATED TEST USER"
        };

        [ClassCleanup]
        public static async Task CleanTests()
        {
            using BlazorRestaurantDbContext blazorRestaurantDbContext = TestsBase.CreateDbContext();
            var testUserEntity = await blazorRestaurantDbContext.ApplicationUser
                .Include(p => p.ApplicationUserRole)
                .Where(p => p.FullName == TestUserModel.FullName
            && p.AzureAdB2cobjectId == TestUserModel.AzureAdB2cobjectId).SingleOrDefaultAsync();
            if (testUserEntity != null)
            {
                if (testUserEntity.ApplicationUserRole != null)
                    blazorRestaurantDbContext.ApplicationUserRole.Remove(testUserEntity.ApplicationUserRole);
                blazorRestaurantDbContext.ApplicationUser.Remove(testUserEntity);
                await blazorRestaurantDbContext.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task UserLoggedInTestAsync()
        {
            var authorizedHttpClient = base.CreateAuthorizedClientAsync();
            var response = await authorizedHttpClient.PostAsJsonAsync<UserModel>("api/User/UserLoggedIn", TestUserModel);
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail(response.ReasonPhrase);
            }
        }
    }
}
