using BlazorRestaurant.Shared.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Orders
{
    public class OrderDetailModel
    {
        public int ProductId { get; set; }
        public int ProductQty { get; set; }
        public int LineNumber { get; set; }
        public decimal LineTotal => ProductQty * Product.UnitPrice;
        public ProductModel Product { get; set; }
    }
}
