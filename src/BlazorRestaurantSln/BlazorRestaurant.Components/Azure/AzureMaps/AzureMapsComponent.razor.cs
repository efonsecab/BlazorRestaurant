using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Components.Azure.AzureMaps
{
    public partial class AzureMapsComponent
    {
        public delegate Task<AzureMapsDataPoint[]> SearchPOIDelegate(string searchTerm, string countryCode);
        public delegate void POISelectedDelegate(AzureMapsDataPoint selectedPOI);
        [Inject]
        public IJSRuntime JavascriptRuntime { get; set; }
        [Parameter]
        public string MapsControlId { get; set; }
        [Parameter]
        public string SubscriptionKey { get; set; }


        [Parameter]
        public GeoCoordinates RouteStart { get; set; } = null;

        [Parameter]
        public GeoCoordinates RouteEnd { get; set; } = null;
        [Parameter]
        public GeoCoordinates[] PointsInRoute { get; set; } = null;
        [Parameter]
        public SearchPOIDelegate SearchPOIAction { get; set; }
        [Parameter]
        public POISelectedDelegate SelectedPOIAction { get; set; }
        [Inject]
        public AzureMapsControlModule module { get; set; }

        private bool IsMapInitialized { get; set; } = false;
        private static Action MapInitializedAction { get; set; }
        private SearchPOIModel SearchPOIModel { get; set; } = new SearchPOIModel();
        private bool IsLoading { get; set; } = false;
        private AzureMapsDataPoint[] PointsOfInterest { get; set; }

        protected async override Task OnInitializedAsync()
        {
            MapInitializedAction = () =>
            {
                this.IsMapInitialized = true;
            };
            await this.InitializeMap();
        }

        [JSInvokable]
        public static void OnMapInitialized()
        {
        }

        private async Task SearchPOI()
        {
            this.PointsOfInterest = await this.SearchPOIAction(this.SearchPOIModel.SearchTerm, this.SearchPOIModel.CountryCode);
            StateHasChanged();
        }

        private async Task OnPOISelected(AzureMapsDataPoint selectedPOI)
        {
            await module.SetCamera(selectedPOI.Latitude, selectedPOI.Longitude);
            SelectedPOIAction(selectedPOI);
        }

        public async Task InitializeMap()
        {
            AzureMapsControlConfiguration options = new AzureMapsControlConfiguration()
            {
                Center = RouteStart,
                Language = "en-US",
                Zoom = 12,
                AuthOptions = new AuthenticationOptions()
                {
                    AuthType = "subscriptionKey",
                    SubscriptionKey = this.SubscriptionKey
                }
            };
            await module.InitializeMap(mapControlId: MapsControlId, mapOptions: options); ;
        }
    }

    public enum MapsLanguage
    {
        [Display(Name = "en-us", ShortName = "en-US")]
        English_US = 1,
        Spanish_SP = 2
    }

    public enum MapsAuthenticationType
    {
        SubscriptionKey = 1,
    }

    public class AuthenticationOptions
    {
        public string AuthType { get; set; } = "subscriptionKey";
        public string SubscriptionKey { get; set; }
    }
    public class AzureMapsControlConfiguration
    {
        public GeoCoordinates Center { get; set; }
        public int Zoom { get; set; }
        public string Language { get; set; } = "en-US";
        public AuthenticationOptions AuthOptions { get; set; }
    }

    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class SearchPOIModel
    {
        [Required]
        public string SearchTerm { get; set; }
        [Required]
        public string CountryCode { get; set; }
    }


    public class AzureMapsDataPoint
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string FreeFormAddress { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
