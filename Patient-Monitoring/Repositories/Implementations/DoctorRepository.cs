using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;

namespace Patient_Monitoring.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public DoctorRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        //✅ Confirmed
        public async Task<Doctor?> GetDoctorByEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }


        public async Task<bool> AddNewDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
