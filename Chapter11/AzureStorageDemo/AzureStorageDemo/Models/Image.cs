using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageDemo.Models
{
    public class Image : TableEntity
    {
        public Image() : this("", "", "", "")
        {

        }
        public Image(string title, string description, string filename, string uri)
        {
            PartitionKey = "Images";
            RowKey = Filename = filename;
            Title = title;
            Description = description;
            Uri = uri;
        }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
    }
}
