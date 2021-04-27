using AutoMapper;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Promos;
using BlazorRestaurant.Shared.User;
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
        }
    }
}
