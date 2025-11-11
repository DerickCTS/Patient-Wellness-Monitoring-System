using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        //✅ Confirmed
        Task<Doctor?> GetDoctorByEmailAsync(string email);

        Task<bool> AddNewDoctorAsync(Doctor doctor);


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

        // ******************* Doctor Profile Data ********************
        Task<Doctor> GetDoctorProfileAsync(int doctorId);
    }
}
