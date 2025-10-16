using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
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

        // Dashboard Overview: Total doctor count
        public async Task<int> GetTotalDoctorsCountAsync()
        {
            return await _context.Doctors.CountAsync();
        }

        // Replace all occurrences of 'DoctorID' with 'DoctorId' to match the property name in the Doctor class

        public async Task<IEnumerable<DoctorManagementDTO>> GetAllDoctorsWithAssignmentCountAsync(int maxCapacity)
        {
            var doctors = await _context.Doctors
                .Select(d => new DoctorManagementDTO
                {
                    DoctorId = Convert.ToInt32(d.DoctorId), // Required member set
                    FullName = $"{d.FirstName} {d.LastName}", // Required member set
                    Email = d.Email, // Required member set
                    Specialization = d.Specialization, // Required member set
                    PatientsAssignedCount = d.AssignedPatient != null ? 1 : 0,
                    MaxCapacity = maxCapacity
                })
                .OrderBy(d => d.FullName)
                .ToListAsync();

            return doctors;
        }
        // Implementation for IDoctorRepository.AddDoctorAsync
        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
