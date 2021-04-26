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
        public Guid? PromoId { get; set; }

        private bool IsEdit => PromoId.HasValue;
        private string Title => IsEdit ? "Add Promo" : "Edit Promo";
    }
}
