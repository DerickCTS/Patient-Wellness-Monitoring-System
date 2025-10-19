using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByEmail(string email);
        Task AddPatient(Patient patient);
    }
}
