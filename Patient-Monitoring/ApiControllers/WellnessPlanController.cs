
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellnessPlanController : ControllerBase
    {
        private readonly PatientMonitoringDbContext _context;

        public WellnessPlanController(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // GET: api/WellnessPlan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WellnessPlan>>> GetWellness_Plans()
        {
            return await _context.WellnessPlans.ToListAsync();
        }

        // GET: api/WellnessPlan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WellnessPlan>> GetWellness_Plan(string id)
        {
            var wellness_Plan = await _context.WellnessPlans.FindAsync(id);

            if (wellness_Plan == null)
            {
                return NotFound();
            }

            return wellness_Plan;
        }

        // PUT: api/WellnessPlan/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWellness_Plan(string id, WellnessPlan wellness_Plan)
        {
            if (id != wellness_Plan.PlanId)
            {
                return BadRequest();
            }

            _context.Entry(wellness_Plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Wellness_PlanExists(id))
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

        // POST: api/WellnessPlan
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WellnessPlan>> PostWellness_Plan(WellnessPlan wellness_Plan)
        {
            _context.WellnessPlans.Add(wellness_Plan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Wellness_PlanExists(wellness_Plan.PlanId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWellness_Plan", new { id = wellness_Plan.PlanId }, wellness_Plan);
        }

        // DELETE: api/WellnessPlan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWellness_Plan(string id)
        {
            var wellness_Plan = await _context.WellnessPlans.FindAsync(id);
            if (wellness_Plan == null)
            {
                return NotFound();
            }

            _context.WellnessPlans.Remove(wellness_Plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Wellness_PlanExists(string id)
        {
            return _context.WellnessPlans.Any(e => e.PlanId == id);
        }
    }
}
