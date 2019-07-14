using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draken.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bulkimport",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var filename = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Processing {filename}");
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    var bytes = FileDownloadFromBlob(filename).Result;
                    ProcessData(bytes, filename).Wait();
                };

                channel.BasicConsume(queue: "bulkimport",
                                        autoAck: false,
                                        consumer: consumer);
                Console.ReadLine();
            }
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
