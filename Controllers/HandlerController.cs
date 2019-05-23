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
                object [] p = t.GetParams();
                Type objType = Type.GetType("Crm." + t.Table);
                if (t.Table.ToLower() == "contact")
                {
                    foreach (var action in t.Actions)
                    {
                        var contacts = _crmContext.Contact.Where(expression, p).ToList();
                        // Type contactType = contact.GetType();
                        if (action.Type.ToLower() == "update")
                        {
                            if (contacts != null)
                            {
                                foreach (var contact in contacts)
                                {
                                    if (action.MetaData == null)
                                    {
                                        continue;
                                    }

                                    string[] values = action.Value.Split("_");
                                    string[] fields = action.MetaData.Field.Split("_");
                                    if (values.Length != fields.Length)
                                    {
                                        throw new Exception(nameof(action));
                                    }
                                    int count = 0;
                                    foreach (var field in fields)
                                    {
                                        foreach (var prop in contact.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                                        {
                                            if (prop.Name.ToLower() == field.ToLower())
                                            {
                                                // FieldInfo fieldInfo = contactType.GetFieldInfo(prop.Name);
                                                // fieldInfo.SetValue(null, action.Value);
                                                var propType = prop.PropertyType;
                                                var targetType = propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) ? Nullable.GetUnderlyingType(propType) : propType;
                                                try
                                                {
                                                    var new_val = Convert.ChangeType(values[count], targetType, null);
                                                    prop.SetValue(contact, new_val);
                                                    
                                                }
                                                catch (InvalidCastException e)
                                                {
                                                    throw new InvalidCastException(nameof(action.Value));
                                                    
                                                }
                                            }
                                            else
                                                continue;
                                        }
                                        count++;

                                    }

                                    _crmContext.Entry(contact).State = EntityState.Modified;
                                    
                                }

                            }
                        }
                        else if (action.Type.ToLower() == "delete")
                        {
                            if (contacts != null)
                            {
                                //TODO delete action
                                foreach (var contact in contacts)
                                {
                                    _crmContext.Contact.Remove(contact);
                                    
                                }
                            }
                        }
                        else if (action.Type.ToLower() == "create")
                        {
                            Contact template = new Contact();
                            //TODO create action
                            if (action.MetaData == null)
                            {
                                continue;
                            }

                            string[] values = action.Value.Split("_");
                            string[] fields = action.MetaData.Field.Split("_");
                            if (values.Length != fields.Length)
                            {
                                throw new Exception(nameof(action));
                            }
                            int count = 0;
                            foreach (var field in fields)
                            {
                                foreach (var prop in template.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                                {
                                    if (prop.Name == field)
                                    {
                                        // FieldInfo fieldInfo = contactType.GetFieldInfo(prop.Name);
                                        // fieldInfo.SetValue(null, action.Value);
                                        prop.SetValue(template, values[count]);
                                    }
                                    else
                                        continue;
                                }
                                count++;

                            }
                            _crmContext.Contact.Add(template);
                        }
                    }
                    try
                    {
                        await _crmContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                    //TODO do action depend on prop, val
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
                string conn = @"Server=" + sv + ";Database=" + db + ";User id=" + userId + ";password=" + pwd;
            }
            return NotFound();
        }



    }
}