using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.CustomComponents
{
    public partial class PromoCard
    {
        [Parameter]
        public RenderFragment Image { get; set; }
        [Parameter]
        public string CardTitle { get; set; }
        [Parameter]
        public string CardBodyText { get; set; }
        [Parameter]
        public bool EnableDelete { get; set; }
        [Parameter]
        public Action DeleteAction { get; set; }
    }
}
