using BlazorRestaurant.Server.Configuration;
using BlazorRestaurant.Shared.Configuration;
using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of Configuration management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigurationController : ControllerBase
    {
        private AzureConfiguration AzureConfiguration { get; }
        private SystemConfiguration SystemConfiguration { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ConfigurationController"/>
        /// <paramref name="azureConfiguration"/>
        /// <paramref name="systemConfiguration"/>
        /// </summary>
        public ConfigurationController(AzureConfiguration azureConfiguration, SystemConfiguration systemConfiguration)
        {
            this.AzureConfiguration = azureConfiguration;
            this.SystemConfiguration = systemConfiguration;
        }

        /// <summary>
        /// Gets the Azure Maps Key
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Roles = Constants.Roles.User)]
        public string GetAzureMapsKey()
        {
            return this.AzureConfiguration.AzureMapsConfiguration.Key;
        }

        /// <summary>
        /// Retrieves the Url for the Power BI Error Log Report
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public string GetErrorLogPowerBIUrl()
        {
            return this.SystemConfiguration.ErrorLogPowerBIUrl;
        }
    }
}
