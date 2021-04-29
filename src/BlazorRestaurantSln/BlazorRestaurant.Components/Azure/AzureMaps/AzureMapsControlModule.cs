using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Components.Azure.AzureMaps
{
    public class AzureMapsControlModule
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public AzureMapsControlModule(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorRestaurant.Components/azure/azuremaps/azureMaps.js").AsTask());
        }
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        internal async Task InitializeMap(string mapControlId, AzureMapsControlConfiguration mapOptions)
        {
            var module = await this.moduleTask.Value;
            await module.InvokeVoidAsync("InitializeMap", mapControlId, mapOptions, "BlazorRestaurant.Components", "OnMapInitialized");
        }

        internal async Task SearchRoute(string mapControlId, AzureMapsControlConfiguration mapOptions,
            GeoCoordinates startingPoint, GeoCoordinates finalPoint, GeoCoordinates[] pointsInRoute)
        {
            var module = await this.moduleTask.Value;
            await module.InvokeVoidAsync("SearchRoute", mapControlId, mapOptions, startingPoint, finalPoint, pointsInRoute);
        }

        internal async Task RenderLines(GeoCoordinates routeStart, GeoCoordinates routeEnd, GeoCoordinates[] pointsInRoute)
        {
            var module = await this.moduleTask.Value;
            await module.InvokeVoidAsync("RenderLine",
                routeStart,
                routeEnd,
                pointsInRoute
                );
        }

        internal async Task SetCamera(float latitude, float longitude)
        {
            var model = await this.moduleTask.Value;
            await model.InvokeVoidAsync("SetCamera", latitude, longitude);
        }
    }
}
