using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorRestaurant.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRestaurant.Server.Tests;
using System.Net.Http;
using System.IO;
using BlazorRestaurant.Shared.Images;
using System.Net.Http.Json;
using Azure.Storage.Blobs;

namespace BlazorRestaurant.Server.Controllers.Tests
{
    [TestClass]
    public class ImageControllerTests : TestsBase
    {
        [ClassCleanup]
        public static async Task CleanTests()
        {
            BlobContainerClient blobContainerClient =
                new(TestsBase.AzureBlobStorageConnectionString,
                TestsBase.TestImagesContainerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            await foreach (var singleBlob in blobs)
            {
                await blobContainerClient.DeleteBlobAsync(singleBlob.Name);
            }
        }

        [TestMethod]
        public async Task UploadImageTest()
        {
            var authorizedHttpClient = base.CreateAuthorizedClientAsync();
            ImageUploadModel model = new()
            {
                FileExtension = Path.GetExtension(this.TestImageFilePath),
                Name = Path.GetFileNameWithoutExtension(this.TestImageFilePath),
                ImageFileBytes = File.ReadAllBytes(this.TestImageFilePath)
            };

            HttpResponseMessage response = await authorizedHttpClient.PostAsJsonAsync("api/Image/UploadImage", model);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Fail(content);
            }
        }

        [TestMethod]
        public async Task ListImagesTest()
        {
            var authorizedHttpClient = base.CreateAuthorizedClientAsync();
            var images = await authorizedHttpClient
                .GetFromJsonAsync<ImageModel[]>("api/Image/ListImages");
            Assert.IsTrue(images.Length > 0);
        }
    }
}