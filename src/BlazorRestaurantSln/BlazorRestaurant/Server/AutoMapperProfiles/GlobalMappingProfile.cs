using AutoMapper;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.AzureMaps;
using BlazorRestaurant.Shared.Promos;
using BlazorRestaurant.Shared.User;
using PTI.Microservices.Library.Models.AzureMapsService.GetSearchPOI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.AutoMapperProfiles
{
    /// <summary>
    /// Global Autmapper Configuration
    /// </summary>
    public class GlobalMappingProfile: Profile
    {
        /// <summary>
        /// Sets how types are going to be mapped by AutoMapper
        /// </summary>
        public GlobalMappingProfile()
        {
            this.CreateMap<UserModel, ApplicationUser>().ReverseMap();

            this.CreateMap<PromotionModel, Promotion>().ReverseMap();

            this.CreateMap<GetSearchPOIResponse, PointOfInterestModel[]>().ConstructUsing(source =>
            source.results.Select(p => new PointOfInterestModel()
            {
                Name = p.poi.name,
                Latitude = p.position.lat,
                Longitude = p.position.lon,
                Country = p.address.country,
                FreeFormAddress = p.address.freeformAddress
            }).ToArray()); ;
        }
    }
}
