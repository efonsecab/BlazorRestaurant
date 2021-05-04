using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorRestaurant.Shared.Global.Constants;

namespace BlazorRestaurant.Client.Pages.User.Orders
{
    [Authorize(Roles = Constants.Roles.User)]
    [Route(Constants.UserPagesRoutes.ListOrders)]
    public partial class List
    {
    }
}
