using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draken.Worker
{
    class Program
    {
        static IQueueClient queueClient;
        static void Main(string[] args)
        {
            queueClient = new QueueClient("Endpoint=sb://packtdemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=wY7Ax8OcMzFfoBfVll+Se+f1CZR+5GrZIvzpyL3Sqtg=", "bulkimport");
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            Console.ReadLine();
        }
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var fileName = Encoding.UTF8.GetString(message.Body);
            var bytes = await FileDownloadFromBlob(fileName);
            await ProcessData(bytes, fileName);
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
        static async Task ProcessData(byte[] bytes, string filename)
        {
            Thread.Sleep(2 * 1000);
            Console.WriteLine($"{filename} with {bytes.Length}");
            string storageConnectionString = "UseDevelopmentStorage=true";
            CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("importedfiles");
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            await cloudBlockBlob.DeleteIfExistsAsync();
        }

        static async Task<byte[]> FileDownloadFromBlob(string filename)
        {
            string storageConnectionString = "UseDevelopmentStorage=true";
            CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("importedfiles");
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            using var memoryStream = new MemoryStream();
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }
    }
}
