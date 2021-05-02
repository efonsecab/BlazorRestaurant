using Microsoft.Authentication.WebAssembly.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Configuration
{
    public class SystemConfiguration
    {
        public string ErrorLogPowerBIUrl { get; set; }
        public ClientAzureAdB2CConfiguration ClientAzureAdB2CConfiguration { get; set; }
        public string ClientAzureAdB2CScope { get; set; }
    }


}
