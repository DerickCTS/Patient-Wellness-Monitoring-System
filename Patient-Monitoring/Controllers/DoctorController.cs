
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly PatientMonitoringDbContext _context;

        public DoctorController(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctor_Detail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctor_Details()
        {
            return await _context.Doctor_Details.ToListAsync();
        }

        // GET: api/Doctor_Detail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor_Detail(string id)
        {
            var doctor_Detail = await _context.Doctor_Details.FindAsync(id);

            if (doctor_Detail == null)
            {
                return NotFound();
            }

            return doctor_Detail;
        }

        // PUT: api/Doctor_Detail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor_Detail(string id, Doctor doctor_Detail)
        {
            if (id != doctor_Detail.DoctorID)
            {
                return BadRequest();
            }

            _context.Entry(doctor_Detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Doctor_DetailExists(id))
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

        // POST: api/Doctor_Detail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor_Detail(Doctor doctor_Detail)
        {
            _context.Doctor_Details.Add(doctor_Detail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Doctor_DetailExists(doctor_Detail.DoctorID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDoctor_Detail", new { id = doctor_Detail.DoctorID }, doctor_Detail);
        }

        // DELETE: api/Doctor_Detail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor_Detail(string id)
        {
            var doctor_Detail = await _context.Doctor_Details.FindAsync(id);
            if (doctor_Detail == null)
            {
                return NotFound();
            }

            _context.Doctor_Details.Remove(doctor_Detail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Doctor_DetailExists(string id)
        {
            return _context.Doctor_Details.Any(e => e.DoctorID == id);
        }
    }
}
