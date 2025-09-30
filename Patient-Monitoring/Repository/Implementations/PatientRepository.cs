using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;

namespace Patient_Monitoring.Repository.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }


        #region Get Patient By Email
        public async Task<Patient_Detail?> GetByEmail(string email)
        {
            return await _context.Patient_Details.FirstOrDefaultAsync(p => p.Email == email);
        }

        #endregion


        #region Add New Patient to DB
        public async Task AddPatient(Patient_Detail patient)
        {
            await _context.Patient_Details.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
