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
        public async Task<Patient?> GetByEmail(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }

        #endregion


        #region Add New Patient to DB
        public async Task AddPatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
