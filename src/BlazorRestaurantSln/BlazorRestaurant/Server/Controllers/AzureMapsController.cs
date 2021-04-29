using AutoMapper;
using BlazorRestaurant.Shared.AzureMaps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTI.Microservices.Library.Models.AzureMapsService.GetSearchPOI;
using PTI.Microservices.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of exposing Azure Maps functionality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AzureMapsController : ControllerBase
    {
        private AzureMapsService AzureMapsService { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="AzureMapsController"/>
        /// </summary>
        /// <param name="azureMapsService"></param>
        /// <param name="mapper"></param>
        public AzureMapsController(AzureMapsService azureMapsService, IMapper mapper)
        {
            this.AzureMapsService = azureMapsService;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Searches Points Of Interests in the specified country, using the specified search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<PointOfInterestModel[]> SearchPointsOfInterest(string searchTerm, string countryCode)
        {
            var response = await this.AzureMapsService.GetSearchPOIAsync(searchTerm, countryCode);
            var result = this.Mapper.Map<GetSearchPOIResponse, PointOfInterestModel[]>(response);
            return result;
        }
    }
}
