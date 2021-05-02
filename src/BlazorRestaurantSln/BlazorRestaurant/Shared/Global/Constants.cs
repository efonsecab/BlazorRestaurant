using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Global
{
    public class Constants
    {
        public class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public class AdminPagesRoutes
        {
            public const string AdminIndex = "/Admin/Index";
            public const string ListImages = "/Admin/Images/List";
            public const string ManageImages = "/Admin/Images/Manage";
            public const string ListPromos = "/Admin/Promos/List";
            public const string AddPromo = "/Admin/Promos/Manage";
            public const string EditPromo = "/Admin/Promos/Manage/{PromoId:long}";
            public const string AddOrder = "/Admin/Orders/Manage";
            public const string EditOrder = "/Admin/Orders/Manage/{OrderId:long}";
            public const string ErrorsLogPowerBI = "/Admin/Errors/ErrorsPowerBI";
            public const string ErrorLog = "/Admin/Errors/ErrorLog";
            public const string AddProduct = "/Admin/Products/Manage";
            public const string EditProduct = "/Admin/Products/Manage/{ProductId:int}";
            public const string ListProducts = "/Admin/Products/List";
        }
    }
}
