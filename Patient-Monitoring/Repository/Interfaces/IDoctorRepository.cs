using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor_Detail?> GetByEmail(string email);

        Task AddDoctor(Doctor_Detail doctor);
    }
}
