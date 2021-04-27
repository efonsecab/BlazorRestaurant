using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Promos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of promotions management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="PromotionController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        public PromotionController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Adds a new promotion to the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task AddPromotion(PromotionModel model)
        {
            var entity = Mapper.Map<PromotionModel, Promotion>(model);
            await this.BlazorRestaurantDbContext.AddAsync(entity);
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
        }
    }
}
