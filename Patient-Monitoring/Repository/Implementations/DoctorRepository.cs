using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;

namespace Patient_Monitoring.Repository.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public DoctorRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        #region Get Doctor by Email
        public async Task<Doctor?> GetByEmail(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }

        #endregion

        #region Add New Doctor to DB
        public async Task AddDoctor(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
