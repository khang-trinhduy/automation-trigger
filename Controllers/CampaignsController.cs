using System.Threading.Tasks;
using Crm;
using Crm.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly CrmContext _context;
        public CampaignsController(CrmContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<Campaign>> Get()
        {
            var campaigns = await _context.Campaign.ToListAsync();
            return Ok(campaigns);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
            var campaign =await _context.Campaign.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return Ok(campaign);
        }

        [HttpPost]
        public async Task<ActionResult<Campaign>> CreateCampaign([FromBody] Campaign c)
        {
            if (c == null)
            {
                return NotFound();
            }
            _context.Campaign.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCampaign", new {id = c.Id}, c);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Campaign>> UpdateCampaign(int id, [FromBody] Campaign campaign)
        {
            if (campaign == null || id != campaign.Id)
            {
                return BadRequest();
            }
            _context.Entry(campaign).State = EntityState.Modified;
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
        public async Task<ActionResult<Campaign>> DeleteCampaign(int id)
        {
            var campaign = await _context.Campaign.FindAsync(id);
            if(campaign == null)
            {
                return  NotFound();
            }

            _context.Campaign.Remove(campaign);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }
            return campaign;
        }
    }
}