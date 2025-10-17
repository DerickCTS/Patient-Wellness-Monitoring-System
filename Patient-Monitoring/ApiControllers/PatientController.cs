
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientController(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // GET: api/Patient_Detail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatient_Details()
        {
            return await _context.Patients.ToListAsync();
        }

        // GET: api/Patient_Detail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient_Detail(string id)
        {
            var patient_Detail = await _context.Patients.FindAsync(id);

            if (patient_Detail == null)
            {
                return NotFound();
            }

            return patient_Detail;
        }

        // PUT: api/Patient_Detail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient_Detail(string id, Patient patient_Detail)
        {
            if (id != patient_Detail.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(patient_Detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Patient_DetailExists(id))
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

        // POST: api/Patient_Detail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient_Detail(Patient patient_Detail)
        {
            _context.Patients.Add(patient_Detail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Patient_DetailExists(patient_Detail.PatientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPatient_Detail", new { id = patient_Detail.PatientId }, patient_Detail);
        }

        // DELETE: api/Patient_Detail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient_Detail(string id)
        {
            var patient_Detail = await _context.Patients.FindAsync(id);
            if (patient_Detail == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient_Detail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Patient_DetailExists(string id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
