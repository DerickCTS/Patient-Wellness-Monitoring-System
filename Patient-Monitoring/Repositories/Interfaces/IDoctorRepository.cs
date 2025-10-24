using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        //✅ Confirmed
        Task<Doctor?> GetDoctorByEmailAsync(string email);

        Task<bool> AddNewDoctorAsync(Doctor doctor);
    }
}
