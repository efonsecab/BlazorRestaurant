using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.User
{
    [Authorize(Roles = Constants.Roles.User)]
    [Route(Constants.UserPagesRoutes.UserIndex)]
    public partial class Index
    {
    }
}
