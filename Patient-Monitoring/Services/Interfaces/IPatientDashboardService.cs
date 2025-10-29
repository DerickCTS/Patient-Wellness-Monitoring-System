using Patient_Monitoring.DTOs.PatientDashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patient_Monitoring.Services.Interfaces
{
    
    public interface IPatientDashboardService
    {
        Task<PatientDashboardDto> GetDashboardData(int patientId);

        // 2. New required method for Upcoming Appointments
        Task<List<AppointmentSummaryDto>> GetUpcomingAppointments(int patientId);

        // 3. New required method for Active Prescriptions
        Task<List<PrescriptionSummaryDto>> GetActivePrescriptions(int patientId);

        // 4. New required method for Recent Activities
        Task<List<TaskLogSummaryDto>> GetRecentActivities(int patientId);

        // 5. New required method for Wellness Progress
        Task<WellnessProgressDto> GetWellnessProgress(int patientId);
    }
}
