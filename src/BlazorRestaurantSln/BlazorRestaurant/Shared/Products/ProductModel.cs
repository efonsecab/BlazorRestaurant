using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Products
{
    public class ProductModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public short ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }
}
