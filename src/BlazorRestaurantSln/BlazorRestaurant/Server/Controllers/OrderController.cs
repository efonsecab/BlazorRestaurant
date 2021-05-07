using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Server.CustomProviders;
using BlazorRestaurant.Shared.Global;
using BlazorRestaurant.Shared.Orders;
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
    /// In charge of orders management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// Creates a new instance of <see cref="OrderController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="httpContextAccessor"></param>
        public OrderController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
            this.HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize(Constants.Roles.User)]
        public async Task<IActionResult> AddOrder(OrderModel orderModel)
        {
            var claims = this.HttpContextAccessor.HttpContext.User.Claims;
            var oidc = claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").SingleOrDefault();
            var userEntity = await this.BlazorRestaurantDbContext
                .ApplicationUser.SingleOrDefaultAsync(p => p.AzureAdB2cobjectId.ToString() == oidc.Value);
            Order orderEntity = this.Mapper.Map<OrderModel, Order>(orderModel);
            orderEntity.ApplicationUserId = userEntity.ApplicationUserId;
            await this.BlazorRestaurantDbContext.Order.AddAsync(orderEntity);
            foreach (var singleLine in orderEntity.OrderDetail)
            {
                this.BlazorRestaurantDbContext.Entry<Product>(singleLine.Product).State = EntityState.Unchanged;
            }
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Retrieves all of the orders in the system
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<OrderModel[]> ListOrders()
        {
            var allOrders = await this.BlazorRestaurantDbContext
                .Order
                .Include(p => p.ApplicationUser)
                .Include(p=>p.OrderDetail)
                .ThenInclude(p=>p.Product)
                .OrderByDescending(p => p.RowCreationDateTime)
                .Select(p => this.Mapper.Map<Order, OrderModel>(p)).ToArrayAsync();
            return allOrders;
        }

        /// <summary>
        /// Returns all the orders for the specified user
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Roles = Constants.Roles.User)]
        public async Task<OrderModel[]> ListOwnedOrders()
        {
            var userObjectId = this.User.Claims.First(p => p.Type == Constants.Claims.ObjectIdentifier).Value;
            return await this.BlazorRestaurantDbContext
                .Order
                .Include(p => p.OrderDetail)
                .ThenInclude(p => p.Product)
                .Include(p => p.ApplicationUser)
                .OrderByDescending(p => p.RowCreationDateTime)
                .Where(p => p.ApplicationUser.AzureAdB2cobjectId.ToString() == userObjectId)
                .Select(p => this.Mapper.Map<Order, OrderModel>(p)).ToArrayAsync();
        }
    }
}
