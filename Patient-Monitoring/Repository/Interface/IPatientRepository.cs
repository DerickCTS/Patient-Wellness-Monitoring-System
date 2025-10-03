using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interface
{
    public interface IPatientRepository
    {
        Task<Patient_Detail?> GetPatientDataByIdOrNameAsync(string patientId, string patientName);
    }
}
