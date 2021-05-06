using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Profile;
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
    /// In charge of Locations management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="LocationController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        public LocationController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Adds a new Location
        /// </summary>
        /// <param name="locationModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> AddLocation(LocationModel locationModel)
        {
            var locationEntity = await this.BlazorRestaurantDbContext
                .Location.Where(p => p.Name == locationModel.Name).SingleOrDefaultAsync();
            if (locationEntity != null)
                throw new Exception($"There is already a Location named: {locationModel.Name}");
            if (locationModel.IsDefault)
            {
                var defaultColelctionsExists = await this.BlazorRestaurantDbContext.Location.AnyAsync(p => p.IsDefault == true);
                if (defaultColelctionsExists)
                    throw new Exception($"Default collections already exist.");
            }
            locationEntity = this.Mapper.Map<LocationModel, Location>(locationModel);
            await this.BlazorRestaurantDbContext.Location.AddAsync(locationEntity);
            await this.BlazorRestaurantDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
