using BlazorRestaurant.Server.Configuration;
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
        /// <summary>
        /// Creates a new instance of <see cref="ConfigurationController"/>
        /// <paramref name="azureConfiguration"/>>
        /// </summary>
        public ConfigurationController(AzureConfiguration azureConfiguration)
        {
            this.AzureConfiguration = azureConfiguration;
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
    }
}
