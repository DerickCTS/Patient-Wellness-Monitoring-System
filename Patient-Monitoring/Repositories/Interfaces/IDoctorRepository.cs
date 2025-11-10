using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        //✅ Confirmed
        Task<Doctor?> GetDoctorByEmailAsync(string email);

        Task<bool> AddNewDoctorAsync(Doctor doctor);

        Task<DoctorProfileDto> GetDoctorProfileAsync(int doctorId);

        // ****************** Doctor Dashboard Stats ******************
        Task<int> GetTotalPatientsCountAsync(int doctorId);
        Task<int> GetTotalActivePrescriptionsCountAsync(int doctorId);
        Task<int> GetTotalWellnessPlansCountAsync(int doctorId);
        Task<int> GetTodaysAppointmentsCountAsync(int doctorId);
        Task<int> GetAppointmentsCompletedTodayAsync(int doctorId);

        // ****************** Weekly Appointment Summary ******************
        Task<List<Appointment>> GetWeeklyAppointmentSummaryAsync(int doctorId, DateTime startOfWeek);

        // ****************** Doctors Weekly Schedule ******************
        Task<List<DoctorAvailability>> GetWeeklyScheduleAsync(int doctorId);

        // ****************** Todays Appointment For Doctor ******************
        Task<List<Appointment>> GetTodaysAppointmentsAsync(int doctorId);

        // ****************** Recent Prescriptions ******************
        Task<List<Prescription>> GetRecentPrescriptionsAsync(int doctorId);
        
        // --- Raw Metrics for Breakdown Pop-ups (used by Service layer) ---
        //Task<(int Total, int NewThisMonth, int Active, int Inactive, int PreviousTotal)> GetPatientBreakdownMetrics(int doctorId, DateTime startOfCurrentMonth, DateTime startOfPreviousMonth);
        //Task<(int TotalActive, int NewThisWeek, int ExpiringSoon, int PreviousPeriodCount)> GetPrescriptionBreakdownMetrics(int doctorId, DateTime startOfCurrentPeriod, DateTime startOfPreviousPeriod);
        //Task<(int Total, int Active, int Pending, int Completed, int PreviousActive)> GetWellnessPlanMetrics(int doctorId, DateTime startOfPreviousMonth);

        // --- Chart and List Data ---
        //Task<Dictionary<DayOfWeek, Tuple<int, int>>> GetWeeklyAppointmentSummaryAsync(int doctorId, DateTime startOfWeek);
        
    }
}
