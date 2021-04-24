using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.User;
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
    /// HandlesIn charge of users management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
                .Where(p => p.AzureAdB2cobjectId == model.AzureAdB2cobjectId).SingleOrDefaultAsync();
            if (userEntity == null)
            {
                userEntity = this.Mapper.Map<UserModel, ApplicationUser>(model);
                userEntity.LastLogIn = DateTimeOffset.UtcNow;
                await this.BlazorRestaurantDbContext.ApplicationUser.AddAsync(userEntity);
            }
            else
            {
                userEntity.LastLogIn = DateTimeOffset.UtcNow;
            }
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
