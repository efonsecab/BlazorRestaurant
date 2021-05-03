using BlazorRestaurant.Shared.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorRestaurant.Client.Pages.Admin
{
    [Authorize(Roles = Constants.Roles.Admin)]
    [Route(Constants.AdminPagesRoutes.AdminIndex)]
    public partial class Index
    {
    }
}
