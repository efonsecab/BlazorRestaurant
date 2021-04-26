using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Configuration
{
    /// <summary>
    /// Holds the the Data Storage configuration
    /// </summary>
    public class DataStorageConfiguration
    {
        /// <summary>
        /// Blob container where image files will be stored
        /// </summary>
        public string ImagesContainerName { get; set; }
        /// <summary>
        /// Url for the Images Blob Container
        /// </summary>
        public string ImagesContainerUrl { get; set; }
    }
}
