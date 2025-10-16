using Patient_Monitoring.DTOs;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardOverviewDTO> GetDashboardOverviewAsync();
        Task<IEnumerable<PatientManagementDTO>> GetAllPatientsAsync();
        Task<PatientManagementDTO> AddPatientAsync(PatientRegisterDTO newPatient);
        Task<IEnumerable<DoctorManagementDTO>> GetAllDoctorsWithAssignmentsAsync();
        Task<DoctorManagementDTO> AddDoctorAsync(DoctorRegisterDTO newDoctor);
        Task<IEnumerable<PatientDoctorMappingDTO>> GetUnmappedPatientsAsync();
        Task<IEnumerable<DoctorManagementDTO>> GetAvailableDoctorsForMappingAsync();
        Task<bool> AssignPatientToDoctorAsync(int patientId, int doctorId);
        Task<bool> DeletePatientDoctorMappingAsync(int patientId);
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync();
        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}
