using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.Pages.Admin.Promos
{
    [Authorize(Roles = Constants.Roles.Admin)]
    public partial class Manage
    {
    }
}
