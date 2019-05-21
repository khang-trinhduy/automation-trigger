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
using System.Linq.Dynamic.Core;
using System.Reflection;

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
            _context = context;
            _crmContext = crmContext;
        }
        [HttpPost]
        public async Task<ActionResult<Handler>> Execute()
        {
            var triggers = await _context.Trigger.Include(t => t.Actions).ThenInclude(a => a.MetaData)
                    .Include(t => t.All).ThenInclude(c => c.MetaData)
                    .Include(t => t.Any).ThenInclude(c => c.MetaData).ToListAsync();
            var sortedTriggers = triggers.Where(t => t.IsNotActive == false).OrderBy(t => t.Position).ToList();
            foreach (var t in sortedTriggers)
            {
                //TODO call linq expression on trigger.cs
                string expression = t.GetExpression();
                if (t.Table.ToLower() == "contact")
                {
                    var contact = _crmContext.Contact.Where(expression).First();
                    Type contactType = contact.GetType();
                    if (contactType != null)
                    {
                        foreach (var action in t.Actions)
                        {
                            if (action.Type.ToLower() == "update")
                            {
                                if (action.MetaData == null)
                                {
                                    continue;
                                }
                                switch (action.Type.ToLower())
                                {
                                    case "update":
                                        foreach (var prop in contactType.GetProperties())
                                        {
                                            if (prop.Name == action.MetaData.Field)
                                            {
                                                FieldInfo fieldInfo = contactType.GetFieldInfo(prop.Name);
                                                fieldInfo.SetValue(null, action.Value);
                                                break;
                                            }
                                            else
                                                continue;
                                        }
                                        break;
                                }
                                
                            }
                            else if(action.Type.ToLower() == "delete")
                            {
                                //TODO delete action
                            }
                        }
                        //TODO do action depend on prop, val
                    }
                }
            }
            
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