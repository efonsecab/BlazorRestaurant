using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Client.CustomComponents
{
    public partial class LoadingComponent
    {
        [Parameter]
        public bool IsLoading { get; set; }
    }
}
