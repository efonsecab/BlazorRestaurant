using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of users management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="UserController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext">Database context</param>
        /// <param name="mapper"></param>
        public UserController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Creates a new user record if this is the first time logging in, otherwise, updates the AD values in case 
        /// they have been changed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> UserLoggedIn(UserModel model)
        {
            var userEntity = await this.BlazorRestaurantDbContext.ApplicationUser
                .Include(p=>p.ApplicationUserRole)
                .ThenInclude(p=>p.ApplicationRole)
                .Where(p => p.AzureAdB2cobjectId == model.AzureAdB2cobjectId).SingleOrDefaultAsync();
            if (userEntity == null)
            {
                userEntity = this.Mapper.Map<UserModel, ApplicationUser>(model);
                userEntity.LastLogIn = DateTimeOffset.UtcNow;
                var userRole = await this.BlazorRestaurantDbContext.ApplicationRole.SingleAsync(p => p.Name == "User");
                userEntity.ApplicationUserRole = new ApplicationUserRole()
                {
                    ApplicationRole = userRole
                };
                await this.BlazorRestaurantDbContext.ApplicationUser.AddAsync(userEntity);
            }
            else
            {
                userEntity.LastLogIn = DateTimeOffset.UtcNow;
            }
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Gets the name of the role assigned to the specified user
        /// </summary>
        /// <param name="userAdB2CObjectId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<string> GetUserRole(Guid userAdB2CObjectId)
        {
            var role = await this.BlazorRestaurantDbContext.ApplicationUserRole
                .Include(p => p.ApplicationUser)
                .Include(p => p.ApplicationRole)
                .Where(p => p.ApplicationUser.AzureAdB2cobjectId == userAdB2CObjectId)
                .Select(p => p.ApplicationRole.Name).SingleAsync();
            return role;
        }
    }
}
