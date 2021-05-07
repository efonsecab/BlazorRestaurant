using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Errors;
using BlazorRestaurant.Shared.Global;
using LinqToTwitter;
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
    /// In charge of errorrs management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class ErrorController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorController"/>
        /// </summary>
        public ErrorController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Lists all of the errors in the system
        /// </summary>
        [HttpGet("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<ErrorLogModel[]> ListErrors()
        {
            var allErrors = await this.BlazorRestaurantDbContext.ErrorLog
                .OrderByDescending(p => p.RowCreationDateTime)
                .Select(p => this.Mapper.Map<ErrorLog, ErrorLogModel>(p))
                .ToArrayAsync();
            return allErrors;
        }
    }
}
