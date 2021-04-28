using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Promos
{
    [Authorize(Roles = BlazorRestaurant.Shared.Global.Constants.Roles.Admin)]
    [Route(BlazorRestaurant.Shared.Global.Constants.AdminPagesRoutes.ListPromos)]
    public partial class List
    {
    }
}
