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
        public async Task<ActionResult<IEnumerable<Patient_Diagnosis>>> GetPatient_Diagnoses()
        {
            return await _context.Patient_Diagnoses.ToListAsync();
        }

        // GET: api/Diagnosis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient_Diagnosis>> GetPatient_Diagnosis(string id)
        {
            var patient_Diagnosis = await _context.Patient_Diagnoses.FindAsync(id);

            if (patient_Diagnosis == null)
            {
                return NotFound();
            }

            return patient_Diagnosis;
        }

        // PUT: api/Diagnosis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient_Diagnosis(string id, Patient_Diagnosis patient_Diagnosis)
        {
            if (id != patient_Diagnosis.DiagnosisID)
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
        public async Task<ActionResult<Patient_Diagnosis>> PostPatient_Diagnosis(Patient_Diagnosis patient_Diagnosis)
        {
            _context.Patient_Diagnoses.Add(patient_Diagnosis);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Patient_DiagnosisExists(patient_Diagnosis.DiagnosisID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPatient_Diagnosis", new { id = patient_Diagnosis.DiagnosisID }, patient_Diagnosis);
        }

        // DELETE: api/Diagnosis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient_Diagnosis(string id)
        {
            var patient_Diagnosis = await _context.Patient_Diagnoses.FindAsync(id);
            if (patient_Diagnosis == null)
            {
                return NotFound();
            }

            _context.Patient_Diagnoses.Remove(patient_Diagnosis);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Patient_DiagnosisExists(string id)
        {
            return _context.Patient_Diagnoses.Any(e => e.DiagnosisID == id);
        }
    }
}
