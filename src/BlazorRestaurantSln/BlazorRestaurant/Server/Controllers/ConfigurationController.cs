using BlazorRestaurant.Server.Configuration;
using BlazorRestaurant.Shared.Configuration;
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
        public string GetAzureMapsKey()
        {
            return this.AzureConfiguration.AzureMapsConfiguration.Key;
        }

        /// <summary>
        /// Retrieves the Url for the Power BI Error Log Report
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public string GetErrorLogPowerBIUrl()
        {
            return this.SystemConfiguration.ErrorLogPowerBIUrl;
        }

        /// <summary>
        /// Returns the Client Azure Ad B2C Configuration
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public ClientAzureAdB2CConfiguration GetClientAzureAdB2CConfiguration()
        {
            return this.SystemConfiguration.ClientAzureAdB2CConfiguration;
        }

        /// <summary>
        /// Returns the Client Azure Ad B2C Scope
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public string GetClientAzureAdB2CScope()
        {
            return this.SystemConfiguration.ClientAzureAdB2CScope;
        }
    }
}
