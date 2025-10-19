using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByEmail(string email);

        Task AddDoctor(Doctor doctor);
    }
}
