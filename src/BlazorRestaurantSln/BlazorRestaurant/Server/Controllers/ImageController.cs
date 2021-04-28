using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.Server.Configuration;
using BlazorRestaurant.Shared.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTI.Microservices.Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of Images management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private AzureBlobStorageService AzureBlobStorageService { get; }
        private DataStorageConfiguration DataStorageConfiguration { get; }
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ImageController"/>
        /// </summary>
        /// <param name="azureBlobStorageService"></param>
        /// <param name="dataStorageConfiguration"></param>
        /// <param name="blazorRestaurantDbContext"></param>
        public ImageController(AzureBlobStorageService azureBlobStorageService,
            DataStorageConfiguration dataStorageConfiguration, BlazorRestaurantDbContext blazorRestaurantDbContext)
        {
            this.AzureBlobStorageService = azureBlobStorageService;
            this.DataStorageConfiguration = dataStorageConfiguration;
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
        }


        /// <summary>
        /// Uploads a new image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadImage(ImageUploadModel model)
        {
            try
            {
                MemoryStream memoryStream = new(model.ImageFileBytes);
                await this.AzureBlobStorageService.UploadFileAsync(containerName: DataStorageConfiguration.ImagesContainerName,
                    $"{model.Name}{model.FileExtension}", memoryStream);
                return Ok();
            }
            catch (Azure.RequestFailedException ex)
            {
                if (ex.ErrorCode == "BlobAlreadyExists")
                    return Problem(detail: "A file with the same name already exists");
                else
                    return Problem(detail: ex.Message);
            }
        }

        /// <summary>
        /// Lists all Images in the system
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ImageModel[]> ListImages()
        {
            List<ImageModel> imageModels = new();
            var pages = this.AzureBlobStorageService.ListFilesAsync(this.DataStorageConfiguration.ImagesContainerName);
            await foreach (var singlePage in pages)
            {
                var blobs = singlePage.Values.Where(p => p.IsBlob)
                    .Select(p => new ImageModel()
                    {
                        ImageUrl = $"{this.DataStorageConfiguration.ImagesContainerUrl}/{p.Blob.Name}",
                        ImageName = Path.GetFileName(p.Blob.Name)
                    });
                if (blobs.Any())
                    imageModels.AddRange(blobs);
            }
            return imageModels.ToArray();
        }

        /// <summary>
        /// Deletes the given image
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteImage(string imageName)
        {
            var promotionsUsingImage = await this.BlazorRestaurantDbContext.Promotion.Where(p => p.ImageUrl.EndsWith(imageName))
                .ToListAsync();
            if (promotionsUsingImage.Count > 0)
            {
                throw new Exception($"Image will not be deleted. It is being used by promotions");
            }
            var result = await this.AzureBlobStorageService.DeleteFileAsync(this.DataStorageConfiguration.ImagesContainerName,
                imageName);
            if (result.Status == (int)System.Net.HttpStatusCode.Accepted)
                return Ok();
            else
                throw new Exception(result.ReasonPhrase);
        }
    }
}
