using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.Shared.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorRestaurant.SystemConfigurationTool.Pages
{
    public partial class Index
    {
        private const string ServerStartConfigurationKey = "ServerStartConfiguration";

        private GlobalSystemConfigurationModel Model { get; set; } = new();
        private string ErrorMessage { get; set; }
        private string SuccessMessage { get; set; }

        private void OnValidSubmit()
        {
            StringBuilder stringBuilder = new StringBuilder();
            this.SuccessMessage = this.ErrorMessage = string.Empty;
            try
            {
                DbContextOptionsBuilder<BlazorRestaurantDbContext> dbContextOptionsBuilder = new();
                BlazorRestaurantDbContext blazorRestaurantDbContext =
                new(dbContextOptionsBuilder.UseSqlServer(this.Model.ConnectionString,
                sqlServerOptionsAction: (serverOptions) => serverOptions.EnableRetryOnFailure(3)).Options);
                if (!blazorRestaurantDbContext.Database.CanConnect())
                {
                    stringBuilder.AppendLine("Unable to connect to the database, please check your connection string");
                }
                string systemConfigurationJson = JsonSerializer.Serialize(this.Model.ConfigurationData);
                var systemConfigurationEntity = blazorRestaurantDbContext
                    .SystemConfiguration.SingleOrDefault(p => p.Name == ServerStartConfigurationKey);
                if (systemConfigurationEntity == null)
                {
                    blazorRestaurantDbContext.SystemConfiguration.Add(
                        new DataAccess.Models.SystemConfiguration()
                        {
                            Name = ServerStartConfigurationKey,
                            Value = systemConfigurationJson
                        });
                }
                else
                {
                    systemConfigurationEntity.Value = systemConfigurationJson;
                }
                blazorRestaurantDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(ex.ToString());
            }
            finally
            {
                this.ErrorMessage = stringBuilder.ToString();
                if (stringBuilder.Length == 0)
                    this.SuccessMessage = "Configuration is saved. You should now be able to run the system";
            }
        }

        private async Task ImportFromFileAsync(InputFileChangeEventArgs e)
        {
            try
            {
                using StreamReader reader = new StreamReader(e.File.OpenReadStream());
                var jsonText = await reader.ReadToEndAsync();
                this.Model = System.Text.Json.JsonSerializer.Deserialize<GlobalSystemConfigurationModel>(jsonText);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }
    }
}
