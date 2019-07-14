using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureStorageDemo.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var listOfImages = new List<Image>();
            //var connectionString = _configuration.GetConnectionString("StorageConnectionString");
            //CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            //var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            //var cloudBlobContainer = cloudBlobClient.GetContainerReference("thumbnails");
            //await cloudBlobContainer.CreateIfNotExistsAsync();
            //await cloudBlobContainer.SetPermissionsAsync(
            //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            //BlobContinuationToken blobContinuationToken = null;
            //do
            //{
            //    var resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
            //    blobContinuationToken = resultSegment.ContinuationToken;
            //    foreach (IListBlobItem item in resultSegment.Results)
            //    {
            //        listOfImages.Add(new Image("", "", item.Uri.Segments.Last(), item.Uri.AbsoluteUri) { });
            //    }
            //} while (blobContinuationToken != null);

            var connectionString = _configuration.GetConnectionString("StorageConnectionString");
            CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            var cloudTable = cloudTableClient.GetTableReference("Images");

            TableOperation retrieve = TableOperation.Retrieve<Image>("Images", filename);
            TableResult result = await cloudTable.ExecuteAsync(retrieve);
            var selectedImage = result.Result as Image;
            await cloudTable.CreateIfNotExistsAsync();
            var query = new TableQuery<Image>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Images"));
            TableContinuationToken tableContinuationToken = null;
            do
            {
                var resultSegment = await cloudTable.ExecuteQuerySegmentedAsync<Image>(query, tableContinuationToken);
                tableContinuationToken = resultSegment.ContinuationToken;
                listOfImages.AddRange(resultSegment.Results);
            } while (tableContinuationToken != null);
            return View(listOfImages);
        }

        [HttpPost]
        public IActionResult RemoveImage(IFormCollection formCollection)
        {
            var imageURL = formCollection["Filename"];
            var connectionString = _configuration.GetConnectionString("StorageConnectionString");
            CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");
            var uri = new Uri(imageURL);
            var filename = uri.Segments.Last();
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            cloudBlockBlob.DeleteIfExistsAsync();

            return RedirectToAction("Index", new { Operation = "Delete", Success = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageAsync(List<IFormFile> files)
        {
            var connectionString = _configuration.GetConnectionString("StorageConnectionString");
            CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");
            await cloudBlobContainer.CreateIfNotExistsAsync();
            await cloudBlobContainer.SetPermissionsAsync(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            var cloudQueueClient = storageAccount.CreateCloudQueueClient();
            var cloudQueueReference = cloudQueueClient.GetQueueReference("queue");
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            var cloudTable = cloudTableClient.GetTableReference("Images");
            await cloudTable.CreateIfNotExistsAsync();
            foreach (var formFile in files)
            {
                var fileName = Path.ChangeExtension(Guid.NewGuid().ToString("N"), Path.GetExtension(formFile.FileName));
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                await cloudBlockBlob.UploadFromStreamAsync(formFile.OpenReadStream());
                cloudBlockBlob.Properties.ContentType = formFile.ContentType;
                await cloudBlockBlob.SetPropertiesAsync();
                await cloudQueueReference.AddMessageAsync(new CloudQueueMessage(fileName));
                var image = new Image()
                {
                    Filename = fileName,
                    Uri = cloudBlockBlob.Uri.AbsoluteUri,
                    Title = string.Empty,
                    Description = string.Empty
                };
                var insertOperation = TableOperation.Insert(image);
                await cloudTable.ExecuteAsync(insertOperation);
            }
            return RedirectToAction("Index", new { O7peration = "Create", Success = true });
        }
    }
}
