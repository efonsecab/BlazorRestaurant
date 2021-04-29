using PTI.Microservices.Library.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Configuration
{
    /// <summary>
    /// Holds the configuration for Azure Resources
    /// </summary>
    public class AzureConfiguration
    {
        /// <summary>
        /// Holds the Azure Maps Configuration
        /// </summary>
        public AzureMapsConfiguration AzureMapsConfiguration { get; set; }
    }
}
