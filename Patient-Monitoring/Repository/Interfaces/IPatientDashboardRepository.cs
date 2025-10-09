using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientDashboardRepository
    {
        Task<Patient> GetPatientByIdAsync(string patientId);
        Task<bool> UpdatePatientAsync(Patient patient);
        
        Task<List<Prescription>> GetActivePrescriptionsAsync(string patientId);
        Task<List<DailyTaskLog>> GetRecentTaskLogsAsync(string patientId, int limit);
        //Task<int> GetDoctorTotalPatientsAsync(string doctorId);
        Task<int> GetPatientTotalVisitsAsync(string patientId, string doctorId);
        Task<DateTime?> GetPatientLastVisitAsync(string patientId, string doctorId);
        //Task<DoctorAvailability> GetDoctorNextAvailabilityAsync(string doctorId);
    }
}
