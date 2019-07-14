using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using System.IO;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;

namespace ThumbnailGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
            });

            builder.ConfigureHostConfiguration(b =>
            {
                b.AddEnvironmentVariables().AddJsonFile("appsettings.json", true, true);
            });

            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });

            var host = builder.Build();
            using (host)
            {
                host.Run();
            }
        }
    }

    public class Functions
    {
        public static async Task ProcessQueueMessage([QueueTrigger("queue")] string message, ILogger logger)
        {
            var filename = message;
            var connectionString = "UseDevelopmentStorage=true";
            CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var imagesContainer = cloudBlobClient.GetContainerReference("images");
            var thumbnailContainer = cloudBlobClient.GetContainerReference("thumbnails");
            var cloudBlockBlob = imagesContainer.GetBlockBlobReference(filename);
            using (var memoryStream = new MemoryStream())
            {
                await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var image = Image.Load(memoryStream, out IImageFormat imageFormat);
                image.Mutate(x => x.Resize(128, 128));
                var thumbnailImage = thumbnailContainer.GetBlockBlobReference(filename);
                using var stream = await thumbnailImage.OpenWriteAsync();
                image.Save(stream, imageFormat);
            }
        }
    }
}
