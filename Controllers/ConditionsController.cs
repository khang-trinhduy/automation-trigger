using System.Collections.Generic;
using System.Threading.Tasks;
using Automation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        private readonly AutoContext _context;
        public ConditionsController(AutoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Condition>>> Get()
        {
            return await _context.Condition.ToListAsync();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Condition>> Get(int id) {
            var condition = await _context.Condition.FindAsync(id);
            if (condition is null) {
                return NotFound();
            }
            return Ok(condition);
        }

        [HttpPost]
        public async Task<ActionResult<Condition>> Create(Condition condition)
        {
            _context.Condition.Add(condition);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new {id = condition.Id}, condition);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Condition>> Put(int id, Condition condition) {
            if (id != condition.Id)
            {
                return BadRequest();
            }

            _context.Entry(condition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var any = await _context.Condition.AnyAsync(e => e.Id == id);
                if (!any)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //TODO add method to insert metadata
        [HttpPut("setmeta/{id}")]
        public async Task<ActionResult<Condition>> SetMeta(int id, MetaData meta)
        {
            var condition = await _context.Condition.FindAsync(id);
            if (condition == null)
            {
                return BadRequest();
            }

            var tmpMeta = await _context.MetaData.FirstOrDefaultAsync(e => e.Type == meta.Type && e.Field == meta.Field);
            if (tmpMeta == null)
            {
                return BadRequest();
            }

            condition.SetMeta(tmpMeta);
            _context.Entry(condition).State = EntityState.Modified;

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
        public async Task<ActionResult<Condition>> DeleteCondition(int id) {
            var condition = await _context.Condition.FindAsync(id);
            if (condition == null) {
                return NotFound();
            }
            _context.Remove(condition);
            await _context.SaveChangesAsync();

            return condition;
        }
    }
}