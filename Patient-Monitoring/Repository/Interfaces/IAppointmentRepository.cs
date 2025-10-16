using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Gets the count of scheduled (non-completed) appointments for the dashboard overview.
        /// </summary>
        Task<int> GetScheduledAppointmentsCountAsync();

        /// <summary>
        /// Retrieves all appointments, joining with Doctor and Patient details, projected as DTOs.
        /// </summary>
        Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsDetailsAsync();

        /// <summary>
        /// Retrieves a single Appointment model by ID.
        /// </summary>
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);

        /// <summary>
        /// Deletes an appointment by ID.
        /// </summary>
        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}
