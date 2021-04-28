using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin
{
    [Authorize(Roles = Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.AdminIndex)]
    public partial class AdminIndex
    {
    }
}
