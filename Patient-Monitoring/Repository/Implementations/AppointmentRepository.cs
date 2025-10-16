using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repository.Implementations
{
    // Concrete implementation of IAppointmentRepository
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public AppointmentRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // Dashboard Overview: Count of scheduled appointments
        public async Task<int> GetScheduledAppointmentsCountAsync()
        {
            // Assuming "Pending" or any future date means "scheduled"
            return await _context.Appointments
                                 .CountAsync(a => a.Status != "Completed");
        }

        // Appointments Management Page: Get all appointments with details
        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsDetailsAsync()
        {
            // We join explicitly to AppointmentSlot since the navigation property might be missing or named differently.
            var appointmentsQuery = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                // Use the explicit join method to link the tables
                .Join(
                    _context.AppointmentSlots, // The table to join with
                    appointment => appointment.SlotId, // FK on Appointment
                    slot => slot.SlotId, // PK on AppointmentSlot
                    (appointment, slot) => new { Appointment = appointment, Slot = slot } // Result object
                );

            return await appointmentsQuery
                .Select(data => new AppointmentDTO
                {
                    // Change this line in GetAllAppointmentsDetailsAsync:
                    AppointmentId = int.Parse(data.Appointment.AppointmentId),
                    DoctorName = $"{data.Appointment.Doctor!.FirstName} {data.Appointment.Doctor.LastName}",
                    PatientName = $"{data.Appointment.Patient!.FirstName} {data.Appointment.Patient.LastName}",
                    Specialization = data.Appointment.Doctor.Specialization,

                    // Convert DateTime to DateOnly
                    Date = DateOnly.FromDateTime(data.Appointment.AppointmentDate),

                    // Combine StartDateTime and EndDateTime from the joined Slot object
                    // TimeOfDay is used to extract only the time portion
                    TimeSlot = $"{data.Slot.StartDateTime.TimeOfDay.ToString(@"hh\:mm")} - {data.Slot.EndDateTime.TimeOfDay.ToString(@"hh\:mm")}",

                    Status = data.Appointment.Status
                })
                .ToListAsync();
        }

        // Delete Logic: Retrieve specific appointment
        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }

        // Delete Logic: Perform deletion
        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}