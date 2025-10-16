using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByEmail(string email);

        Task AddDoctor(Doctor doctor);
    
    public async Task<int> GetTotalDoctorsCountAsync()
        {
            // Replace with your actual data source logic
            // Example for Entity Framework:
            // return await _context.Doctors.CountAsync();

            // Placeholder implementation
            return await Task.FromResult(0);
        }

        Task<IEnumerable<DoctorManagementDTO>> GetAllDoctorsWithAssignmentCountAsync(int MAX_PATIENT_CAPACITY);
        Task AddDoctorAsync(Doctor doctor);
    }
}
