using System.Collections.Generic;
using System.Threading.Tasks;
using Automation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MetaDatasController : ControllerBase
    {
        private readonly AutoContext _context;
        public MetaDatasController(AutoContext context) {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaData>>> Get()
        {
            return await _context.MetaData.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetaData>> GetMeta(int id) {
            var meta = await _context.MetaData.FindAsync(id);
            if (meta == null) {
                return NotFound();
            }

            return meta;
        }

        [HttpPost]
        public async Task<ActionResult<MetaData>> CreateMeta(MetaData meta)
        {
            _context.MetaData.Add(meta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeta", new {id = meta.Id}, meta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MetaData>> Put(int id, MetaData meta) {
            if (id != meta.Id) {
                return BadRequest();
            }

            _context.Entry(meta).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var any = await _context.MetaData.AnyAsync(e => e.Id == id);
                if (!any) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MetaData>> DeleteMeta(int id) {
            var meta = await _context.MetaData.FindAsync(id);
            if (meta == null)
            {
                return NotFound();
            }

            _context.Remove(meta);
            await _context.SaveChangesAsync();
            
            return meta;
        }

    }
}