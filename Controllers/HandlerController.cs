using System.Threading.Tasks;
using Automation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Crm.Context;
using Microsoft.EntityFrameworkCore.Internal;
using Crm;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Automation.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HandlerController : ControllerBase
    {
        private readonly AutoContext _context;
        private CrmContext _crmContext;
        public HandlerController(AutoContext context, CrmContext crmContext)
        {
            string conn = @"Server=khang-pc\\sqlexpress;Database=DataContext;Trusted_connection=True;";
            _context = context;
            using (_crmContext = new CrmContext()) {
                _crmContext.Connection = conn;
            };
        }
        [HttpPost]
        public async Task<ActionResult<Handler>> Execute()
        {
            var triggers = await _context.Trigger.Include(t => t.Actions).ThenInclude(a => a.MetaData)
                    .Include(t => t.All).ThenInclude(c => c.MetaData)
                    .Include(t => t.Any).ThenInclude(c => c.MetaData).ToListAsync();
            var sortedTriggers = triggers.Where(t => t.IsNotActive == false).OrderBy(t => t.Position).ToList();
            return Ok();
        }

        [HttpGet("validate")]
        public async Task<ActionResult<Handler>> ValidateConnection([FromQuery] string sv, [FromQuery]string db, [FromQuery] bool trusted, [FromQuery] string userId, [FromQuery] string pwd)
        {
            if (trusted)
            {
                string conn = @"Server=" + sv + ";Database=" + db + ";Trusted_Connection=True;";
                
            }
            else
            {
                string conn = @"Server=" + sv + ";Database=" + db + ";User id=" + userId +";password=" + pwd;
            }
            return NotFound();
        }

    }
}