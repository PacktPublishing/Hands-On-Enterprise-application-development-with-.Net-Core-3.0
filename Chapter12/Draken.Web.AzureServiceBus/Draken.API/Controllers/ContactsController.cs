using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draken.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Draken.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _databaseContext;
        public ContactsController(DatabaseContext databaseContext, LinkGenerator linkGenerator, IDistributedCache cache, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _linkGenerator = linkGenerator;
            _cache = cache;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Contact>>> Get(int page = 0, int pageSize = 10)
        {
            var cachedContacts = await _cache.GetAsync("Contacts");
            List<Contact> contacts;
            if (cachedContacts != null)
            {
                contacts = JsonConvert.DeserializeObject<List<Contact>>(Encoding.UTF8.GetString(cachedContacts));
            }
            else
            {
                contacts = await _databaseContext.Contacts.ToListAsync();
                if (contacts != null)
                {
                    var options = new DistributedCacheEntryOptions()
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(20)
                    };

                    await _cache.SetAsync("Contacts",
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(contacts)), options);
                }
            }

            var totalCount = contacts.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var prevLink = page > 0 ? _linkGenerator.GetPathByAction("Get", "Contacts", new { page = page - 1, pageSize = 10 }) : "";
            var nextLink = page < totalPages - 1 ? _linkGenerator.GetPathByAction("Get", "Contacts", new { page = page + 1, pageSize = 10 }) : "";
            Response.Headers.Add("X-Paging-TotalCount", totalCount.ToString());
            Response.Headers.Add("X-Paging-TotalPages", totalPages.ToString());
            Response.Headers.Add("X-Paging-PageSize", pageSize.ToString());
            Response.Headers.Add("X-Paging-CurrentPage", page.ToString());
            if (!string.IsNullOrEmpty(nextLink))
            {
                Response.Headers.Add("X-Paging-Next", nextLink);
            }
            if (!string.IsNullOrEmpty(prevLink))
            {
                Response.Headers.Add("X-Paging-Previous", prevLink);
            }
            var results = contacts
                .Skip(pageSize * page)
                .Take(pageSize);

            return results.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            var contact = await _databaseContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPost]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Contact>> Post([FromBody] Contact contact)
        {
            _databaseContext.Contacts.Add(contact);
            await _databaseContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(int id, [FromBody] Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            _databaseContext.Entry(contact).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int id)
        {
            var todoItem = await _databaseContext.Contacts.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _databaseContext.Contacts.Remove(todoItem);
            await _databaseContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("/api/contact/bulkimport")]
        [AllowAnonymous]
        public async Task<ActionResult> BulkImport()
        {
            var file = Request.Form.Files[0];
            var fileName = await UploadFileToBlob(file);
            var queueClient = new QueueClient(_configuration.GetSection
                ("ServiceBusConnectionString").Value, "bulkimport");
            var message = new Message(Encoding.UTF8.GetBytes(fileName));
            await queueClient.SendAsync(message);
            return Ok();
        }

        private async Task<string> UploadFileToBlob(IFormFile file)
        {
            string storageConnectionString = _configuration.GetSection("StorageConnectionString").Value;
            CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount);
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("importedfiles");
            await cloudBlobContainer.CreateIfNotExistsAsync();
            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            var filename = Path.ChangeExtension(Guid.NewGuid().ToString("N"), "csv");
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return filename;
        }
    }
}
