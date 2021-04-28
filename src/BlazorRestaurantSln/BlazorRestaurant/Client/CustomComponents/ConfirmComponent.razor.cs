using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.CustomComponents
{
    public partial class ConfirmComponent
    {
        [Parameter]
        public Action OkAction { get; set; }
        [Parameter]
        public Action CloseAction { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public RenderFragment BodyContent { get; set; }
        [Parameter]
        public string OkText { get; set; }
        [Parameter]
        public string CloseText { get; set; }
    }
}
