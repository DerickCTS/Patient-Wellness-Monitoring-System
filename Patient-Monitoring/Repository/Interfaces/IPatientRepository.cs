using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient_Detail?> GetByEmail(string email);
        Task AddPatient(Patient_Detail patient);
    }
}
