using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interface;

namespace Patient_Monitoring.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }
        public async Task<Patient_Detail?> GetPatientDataByIdOrNameAsync(string patientId, string patientName)
        {
            return await _context.Patient_Details.Include(d => d.PatientDiagnoses).ThenInclude(pd => pd.Disease)
                .FirstOrDefaultAsync(p => p.PatientID == patientId || (p.FirstName + " " + p.LastName).Contains(patientName));
        }
    }
}
