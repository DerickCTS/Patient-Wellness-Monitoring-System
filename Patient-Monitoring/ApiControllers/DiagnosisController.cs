using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly PatientMonitoringDbContext _context;

        public DiagnosisController(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // GET: api/Diagnosis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetPatient_Diagnoses()
        {
            return await _context.Diagnoses.ToListAsync();
        }

        // GET: api/Diagnosis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetPatient_Diagnosis(string id)
        {
            var patient_Diagnosis = await _context.Diagnoses.FindAsync(id);

            if (patient_Diagnosis == null)
            {
                return NotFound();
            }

            return patient_Diagnosis;
        }

        // PUT: api/Diagnosis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient_Diagnosis(string id, Diagnosis patient_Diagnosis)
        {
            if (id != patient_Diagnosis.DiagnosisId)
            {
                return BadRequest();
            }

            _context.Entry(patient_Diagnosis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Patient_DiagnosisExists(id))
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

        // POST: api/Diagnosis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Diagnosis>> PostPatient_Diagnosis(Diagnosis patient_Diagnosis)
        {
            _context.Diagnoses.Add(patient_Diagnosis);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Patient_DiagnosisExists(patient_Diagnosis.DiagnosisId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPatient_Diagnosis", new { id = patient_Diagnosis.DiagnosisId }, patient_Diagnosis);
        }

        // DELETE: api/Diagnosis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient_Diagnosis(string id)
        {
            var patient_Diagnosis = await _context.Diagnoses.FindAsync(id);
            if (patient_Diagnosis == null)
            {
                return NotFound();
            }

            _context.Diagnoses.Remove(patient_Diagnosis);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Patient_DiagnosisExists(string id)
        {
            return _context.Diagnoses.Any(e => e.DiagnosisId == id);
        }
    }
}
