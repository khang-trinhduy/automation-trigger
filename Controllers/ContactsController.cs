using System.Threading.Tasks;
using Crm;
using Crm.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly CrmContext _context;
        public ContactsController(CrmContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact =await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact([FromBody] Contact c)
        {
            if (c == null)
            {
                return NotFound();
            }
            _context.Contact.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContact", new {id = c.Id}, c);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Contact>> UpdateContact(int id, [FromBody] Contact contact)
        {
            if (contact == null || id != contact.Id)
            {
                return BadRequest();
            }
            var c = await _context.Contact.FindAsync(id);
            if (c == null)
            {
                return BadRequest();
            }
            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;                
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            if(contact == null)
            {
                return  NotFound();
            }

            _context.Contact.Remove(contact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }
            return contact;
        }
    }
}