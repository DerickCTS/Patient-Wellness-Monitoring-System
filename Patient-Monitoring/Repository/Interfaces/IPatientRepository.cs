using Patient_Monitoring.Models;
using Patient_Monitoring.DTOs;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByEmail(string email);
        Task AddPatient(Patient patient);
        Task<Patient> GetPatientByIdAsync(string patientId);
        Task UpdatePatientAsync(Patient patient);
        Task<PatientDashboardDTO> GetPatientDashboardDataAsync(string patientId);

        // Fix for CS1061: Add missing method signature
        Task<int> GetTotalPatientsCountAsync();
        Task<IEnumerable<PatientManagementDTO>> GetAllPatientsWithAssignedDoctorAsync();
        Task<IEnumerable<object>> GetPatientsWithoutMappingAsync();
    }
}
