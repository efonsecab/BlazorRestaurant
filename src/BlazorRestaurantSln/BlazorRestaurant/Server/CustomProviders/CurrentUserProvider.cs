using BlazorRestaurant.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.CustomProviders
{
    /// <summary>
    /// Holds the logic to retrieve the current user's information
    /// </summary>
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        /// <summary>
        /// Creates a new instance of <see cref="CurrentUserProvider"/>
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the user's username
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        {
            var user = this.HttpContextAccessor.HttpContext.User;
            return user?.Identity.Name ?? "Unknown";
        }
    }
}
