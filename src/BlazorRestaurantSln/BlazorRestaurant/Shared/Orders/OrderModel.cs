using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Orders
{
    public class OrderModel
    {
        [Required]
        [StringLength(1000)]
        public string DestinationFreeFormAddress { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public decimal Total => OrderDetail.Sum(p => p.LineTotal);
        public List<OrderDetailModel> OrderDetail { get; set; }
    }
}
