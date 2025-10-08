using Patient_Monitoring.Models;
using Patient_Monitoring.DTOs;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient_Detail?> GetByEmail(string email);
        Task AddPatient(Patient_Detail patient);
        // CRUD operations (assumed to exist)
        Task<Patient_Detail> GetPatientByIdAsync(string patientId);
        Task UpdatePatientAsync(Patient_Detail patient);

        // --- New method for Dashboard Data Retrieval ---
        Task<PatientDashboardDto> GetPatientDashboardDataAsync(string patientId);

        // --- New methods for Modal Updates ---
        Task<bool> UpdatePatientContactInfoAsync(string patientId, UpdateContactInfoDto updateDto);
        Task<bool> UpdatePatientPasswordAsync(string patientId, ChangePasswordDto passwordDto);
    }
}
