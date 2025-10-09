using Patient_Monitoring.Models;
using Patient_Monitoring.DTOs;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByEmail(string email);
        Task AddPatient(Patient patient);
        // CRUD operations (assumed to exist)
        Task<Patient> GetPatientByIdAsync(string patientId);
        Task UpdatePatientAsync(Patient patient);

        // --- New method for Dashboard Data Retrieval ---
        Task<PatientDashboardDTO> GetPatientDashboardDataAsync(string patientId);

        // --- New methods for Modal Updates ---
        //Task<bool> UpdatePatientContactInfoAsync(string patientId, UpdateContactInfoDto updateDto);
        //Task<bool> UpdatePatientPasswordAsync(string patientId, ChangePasswordDTO passwordDto);
    }
}
