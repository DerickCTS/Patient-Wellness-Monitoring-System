
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
        public async Task<ActionResult<IEnumerable<Wellness_Plan>>> GetWellness_Plans()
        {
            return await _context.Wellness_Plans.ToListAsync();
        }

        // GET: api/WellnessPlan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wellness_Plan>> GetWellness_Plan(string id)
        {
            var wellness_Plan = await _context.Wellness_Plans.FindAsync(id);

            if (wellness_Plan == null)
            {
                return NotFound();
            }

            return wellness_Plan;
        }

        // PUT: api/WellnessPlan/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWellness_Plan(string id, Wellness_Plan wellness_Plan)
        {
            if (id != wellness_Plan.PlanID)
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
        public async Task<ActionResult<Wellness_Plan>> PostWellness_Plan(Wellness_Plan wellness_Plan)
        {
            _context.Wellness_Plans.Add(wellness_Plan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Wellness_PlanExists(wellness_Plan.PlanID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWellness_Plan", new { id = wellness_Plan.PlanID }, wellness_Plan);
        }

        // DELETE: api/WellnessPlan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWellness_Plan(string id)
        {
            var wellness_Plan = await _context.Wellness_Plans.FindAsync(id);
            if (wellness_Plan == null)
            {
                return NotFound();
            }

            _context.Wellness_Plans.Remove(wellness_Plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Wellness_PlanExists(string id)
        {
            return _context.Wellness_Plans.Any(e => e.PlanID == id);
        }
    }
}
