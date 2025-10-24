using Patient_Monitoring.DTOs.WellnessPlan;


namespace Patient_Monitoring.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<List<PatientSearchItemDto>> SearchPatientsAsync(string? patientName, int? patientId);
        Task<PatientFullDetailDto?> GetPatientDetailsAsync(int patientId);
        Task AssignPlanAsync(AssignPlanRequest request);
        Task<DiagnosisDetailsDto> GetDiagnosisDetailsAsync(int diagnosisId);

    }
}
