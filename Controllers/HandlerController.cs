using System.Threading.Tasks;
using Automation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Automation.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HandlerController : ControllerBase
    {
        private readonly AutoContext _context;
        public HandlerController(AutoContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Handler>> Post()
        {
            var triggers = await _context.Trigger.Include(t => t.Actions).ThenInclude(a => a.MetaData)
                    .Include(t => t.Conditions).ThenInclude(c => c.MetaData).ToListAsync();
            var sortedTriggers = triggers.Where(t => t.IsNotActive == false).OrderBy(t => t.Position).ToList();
            SqlHelper help = new SqlHelper("khang-pc\\sqlexpress", "DataContext", true);
            foreach (var t in sortedTriggers)
            {
                Handler handle = new Handler(help);
                handle.ExecuteTrigger(t);

            }
            return Ok();
        }
    }
}