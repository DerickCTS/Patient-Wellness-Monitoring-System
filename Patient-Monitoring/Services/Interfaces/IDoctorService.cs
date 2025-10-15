using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services.Interface
{
    public interface IDoctorService
    {
        public Task<List<PatientSearchItemDto>> SearchPatientsAsync(string? patientName, string? patientId);
        Task<PatientFullDetailDto?> GetPatientDetailsAsync(string patientId);
        Task<bool> IsDoctorAssignedToPatient(string patientId, string doctorId);
        Task AssignPlanAsync(AssignPlanRequest request);
        Task<DiagnosisDetailsDto> GetDiagnosisDetailsAsync(string diagnosisId);

    }
}
