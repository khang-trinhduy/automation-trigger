using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Automation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TriggersController : ControllerBase
    {
        private readonly AutoContext _context;
        public TriggersController(AutoContext context)
        {
            _context = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trigger>>> Get() {
            return await _context.Trigger.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trigger>> GetTrigger(int id)
        {
            var trigger = await _context.Trigger.FindAsync(id);
            if (trigger == null)
            {
                return NotFound();
            }

            return Ok(trigger);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Trigger>> CreateTrigger(Trigger trigger)
        {
            _context.Trigger.Add(trigger);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTrigger", new {id = trigger.Id}, trigger);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Trigger>> Put(int id, Trigger trigger)
        {
            if (id != trigger.Id) {
                return BadRequest();
            }
            _context.Entry(trigger).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var any = await _context.Trigger.AnyAsync(e => e.Id == id);
                if (!any)
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPut("setaction/{id}")]
        public async Task<ActionResult<Trigger>> SetAction(int id, Models.Action action)
        {
            var trigger = await _context.Trigger.FindAsync(id);
            if (trigger == null)
            {
                return BadRequest();
            }

            var tmpAction = await _context.Action.FirstOrDefaultAsync(e => e.Type == action.Type && e.Value == action.Value);
            if (tmpAction == null)
            {
                return BadRequest();
            }

            trigger.AddAction(tmpAction);
            trigger.LastUpdated = DateTime.Now;
            _context.Entry(trigger).State = EntityState.Modified;
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

        [HttpPut("setcondition/{id}")]
        public async Task<ActionResult<Trigger>> SetCondition(int id, Condition condition)
        {
            var trigger = await _context.Trigger.FindAsync(id);
            if (trigger == null)
            {
                return BadRequest();
            }

            var tmpCondition = await _context.Condition.FirstOrDefaultAsync(e => e.Type == condition.Type && e.Operator == condition.Operator && e.Threshold == condition.Threshold);
            if (tmpCondition == null)
            {
                return BadRequest();
            }

            // trigger.AddCondition(tmpCondition);
            trigger.LastUpdated = DateTime.Now;
            _context.Entry(trigger).State = EntityState.Modified;
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
        //TODO test getquery, add metadata
        // [HttpGet("query/{id}")]
        // public async Task<ActionResult<string>> GetQuery(int id) 
        // {
        //     var triggers = await _context.Trigger.Include(t => t.Actions).ThenInclude(a => a.MetaData)
        //                     .Include(t => t.Conditions).ThenInclude(c => c.MetaData).ToListAsync();
        //     var trigger = triggers.FirstOrDefault(e => e.Id == id);
        //     if (trigger == null)
        //     {
        //         return NotFound();
        //     }

        //     return trigger.GetQuery();
        // }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trigger>> DeleteTrigger(int id) {
            var trigger = await _context.Trigger.FindAsync(id);
            if (trigger == null) {
                return NotFound();
            }

            _context.Remove(trigger);
            await _context.SaveChangesAsync();

            return trigger;
        }
    }
}
