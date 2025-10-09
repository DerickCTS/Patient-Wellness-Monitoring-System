using Patient_Monitoring.DTOs;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IPatientDashboardService
    {
        Task<PatientDashboardDTO> GetPatientDashboardDataAsync(string patientId);
        Task<PatientInfoDTO> GetPatientInfoAsync(string patientId);
        Task<PatientInfoDTO> UpdatePatientInfoAsync(string patientId, UpdatePatientInfoDTO updateDTO);
        Task<string> UploadProfileImageAsync(string patientId, string base64Image);
        Task<AssignedDoctorDTO> GetAssignedDoctorAsync(string patientId);
        Task<List<PrescriptionDTO>> GetActivePrescriptionsAsync(string patientId);
        Task<List<DailyTaskLogDTO>> GetRecentTaskLogsAsync(string patientId, int limit = 10);
    }
}
