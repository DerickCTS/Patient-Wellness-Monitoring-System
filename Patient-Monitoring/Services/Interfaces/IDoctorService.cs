using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.DTOs.WellnessPlan;


namespace Patient_Monitoring.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<List<PatientSearchItemDto>> SearchPatientsAsync(string? patientName, int? patientId);
        public Task<PatientFullDetailDto?> GetPatientDetailsAsync(int patientId);
        public Task AssignPlanAsync(AssignPlanRequest request);
        public Task<DiagnosisDetailsDto> GetDiagnosisDetailsAsync(int diagnosisId);

        #region DoctorDashboard

        public Task<DoctorDashboardDto> GetDashboardStatsAsync(int doctorId);
        
        public Task<DoctorProfileDto> GetDoctorProfileAsync(int doctorId);

        #endregion

    }
}
