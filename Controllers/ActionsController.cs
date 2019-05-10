using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Automation.API.Models;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly AutoContext _context;

        public ActionsController(AutoContext context)
        {
            _context = context;
        }

        // GET: api/Actions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Action>>> GetAction()
        {
            return await _context.Action.ToListAsync();
        }

        // GET: api/Actions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Action>> GetAction(int id)
        {
            var action = await _context.Action.FindAsync(id);

            if (action == null)
            {
                return NotFound();
            }

            return action;
        }

        // PUT: api/Actions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAction(int id, Models.Action action)
        {
            if (id != action.Id)
            {
                return BadRequest();
            }

            _context.Entry(action).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionExists(id))
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

        // POST: api/Actions
        [HttpPost]
        public async Task<ActionResult<Models.Action>> PostAction(Models.Action action)
        {
            _context.Action.Add(action);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAction", new { id = action.Id }, action);
        }

        // DELETE: api/Actions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.Action>> DeleteAction(int id)
        {
            var action = await _context.Action.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }

            _context.Action.Remove(action);
            await _context.SaveChangesAsync();

            return action;
        }

        private bool ActionExists(int id)
        {
            return _context.Action.Any(e => e.Id == id);
        }
    }
}
