using Patient_Monitoring.DTOs.WellnessPlan;

namespace Patient_Monitoring.Services.Interfaces
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
