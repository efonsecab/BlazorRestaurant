using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Profile
{
    public class LocationModel
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [StringLength(1000)]
        public string FreeFormAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}
