using System.Collections.Generic;
using System.Threading.Tasks;
using Draken.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Draken.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public ContactsController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Contact>>> Get()
        {
            return await _databaseContext.Contacts.ToListAsync();
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
    }
}
