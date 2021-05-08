using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Configuration
{

    public class GlobalSystemConfigurationModel
    {
        [Required]
        public string ConnectionString { get; set; }
        [ValidateComplexType]
        public SystemConfigurationModel ConfigurationData { get; set; } = new();
    }
    public class SystemConfigurationModel
    {
        [ValidateComplexType]
        public Systemconfiguration SystemConfiguration { get; set; } = new();
        [ValidateComplexType]
        public Datastorageconfiguration DataStorageConfiguration { get; set; } = new();
        [ValidateComplexType]
        public Ptimicroserviceslibraryconfiguration PTIMicroservicesLibraryConfiguration { get; set; } = new();
        [ValidateComplexType]
        public Azureconfiguration AzureConfiguration { get; set; } = new();
        [ValidateComplexType]
        public Azureadb2c1 AzureAdB2C { get; set; } = new();
    }

    public class Systemconfiguration
    {
        [Required]
        [Url]
        public string ErrorLogPowerBIUrl { get; set; }
        [ValidateComplexType]
        public Clientazureadb2cconfiguration ClientAzureAdB2CConfiguration { get; set; } = new();
        [Required]
        [Url]
        public string ClientAzureAdB2CScope { get; set; }
    }

    public class Clientazureadb2cconfiguration
    {
        [ValidateComplexType]
        public Azureadb2c AzureAdB2C { get; set; } = new();
    }

    public class Azureadb2c
    {
        [Required]
        [Url]
        public string Authority { get; set; }
        [Required]
        public string ClientId { get; set; }
        public bool ValidateAuthority { get; set; }
    }

    public class Datastorageconfiguration
    {
        [Required]
        public string ImagesContainerName { get; set; }
        [Required]
        [Url]
        public string ImagesContainerUrl { get; set; }
    }

    public class Ptimicroserviceslibraryconfiguration
    {
        [Required]
        public string RapidApiKey { get; set; }
    }

    public class Azureconfiguration
    {
        [ValidateComplexType]
        public Azureblobstorageconfiguration AzureBlobStorageConfiguration { get; set; } = new();
        [ValidateComplexType]
        public Azuremapsconfiguration AzureMapsConfiguration { get; set; } = new();
    }

    public class Azureblobstorageconfiguration
    {
        [Required]
        public string ConnectionString { get; set; }
    }

    public class Azuremapsconfiguration
    {
        [Required]
        public string Key { get; set; }
    }

    public class Azureadb2c1
    {
        [Required]
        public string Instance { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Domain { get; set; }
        [Required]
        public string SignUpSignInPolicyId { get; set; }
    }

}
