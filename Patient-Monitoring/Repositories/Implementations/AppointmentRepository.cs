using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public AppointmentRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // ============================ GENERAL QUERIES ============================
        public async Task<List<string>> GetDistinctSpecializationsAsync()
        {
            return await _context.Doctors
                .Select(d => d.Specialization)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsBySpecializationAsync(string specialization)
        {
            return await _context.Doctors
                .Where(d => d.Specialization == specialization)
                .ToListAsync();
        }

        public async Task<List<AppointmentSlot>> GetDoctorAvailableSlotsAsync(string doctorId, DateTime cutoffDate)
        {
            return await _context.AppointmentSlots
                .Where(s => s.DoctorId == doctorId &&
                            s.IsBooked == false &&
                            s.StartDateTime >= DateTime.Now &&
                            s.StartDateTime <= cutoffDate)
                .OrderBy(s => s.StartDateTime)
                .Take(6)
                .ToListAsync();
        }

        public async Task<AppointmentSlot> GetSlotForBookingAsync(int slotId, string doctorId)
        {
            return await _context.AppointmentSlots
                .FirstOrDefaultAsync(s => s.SlotId == slotId && s.DoctorId == doctorId);
        }

        public async Task<Appointment> GetAppointmentByIdIncludeSlotAsync(string appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.AppointmentSlot)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves the highest sequential Appointment ID (e.g., A109) from the database.
        /// </summary>
        public async Task<string> GetLastSequentialAppointmentIdAsync()
        {
            // Step 1: Filter to get all AppointmentIDs that start with 'A'. (SQL translation is easy)
            var sequentialIds = await _context.Appointments
                .Select(a => a.AppointmentId)
                .Where(id => id.StartsWith("A"))
                .ToListAsync(); // <-- Query executed here, bringing IDs into memory

            // Step 2: Now that data is in memory, use C# logic to find the highest valid sequential ID.
            return sequentialIds
                .Where(id => id.Length > 1 && id.Skip(1).All(char.IsDigit))
                .OrderByDescending(id => int.Parse(id.Substring(1))) // Order by the numeric part
                .FirstOrDefault();
        }

        // ============================ APPOINTMENT MANAGEMENT ============================
        public void AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void UpdateAppointmentSlot(AppointmentSlot slot)
        {
            _context.AppointmentSlots.Update(slot);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        // ============================ METRICS/SCHEDULE QUERIES ============================
        public Task<int> CountAppointmentsAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return _context.Appointments.CountAsync(predicate);
        }

        public Task<int> CountAppointmentSlotsAsync(Expression<Func<AppointmentSlot, bool>> predicate)
        {
            return _context.AppointmentSlots.CountAsync(predicate);
        }

        public async Task<List<Appointment>> GetConfirmedAppointmentsForDoctor(string doctorId, DateTime startDate, DateTime endDate)
        {
            // Includes Pending for accurate slot counts in Weekly Summary
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate >= startDate &&
                            a.AppointmentDate < endDate &&
                            (a.Status == "Confirmed" || a.Status == "Pending Approval"))
                .ToListAsync();
        }

        // ============================ PATIENT/DOCTOR LISTS ============================

        /// <summary>
        /// Retrieves all slots for a doctor on a specific date, eagerly loading Appointment and Patient details for booked slots.
        /// </summary>
        public async Task<List<AppointmentSlot>> GetDoctorSlotsWithAppointmentDetailsAsync(string doctorId, DateTime date)
        {
            return await _context.AppointmentSlots
                .Where(s => s.DoctorId == doctorId && s.StartDateTime.Date == date.Date)
                .OrderBy(s => s.StartDateTime)
                // Assuming navigation properties:
                // AppointmentSlot has a property named 'Appointment' (1:0 or 1:1)
                // Appointment has a property named 'Patient' (Foreign Key)
                .Include(s => s.Appointment)
                    .ThenInclude(a => a.Patient)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPatientFutureAppointmentsAsync(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.Today)
                .Include(a => a.Doctor) // Assuming a navigation property to Doctor_Details
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPatientPastAppointmentsAsync(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId &&
                            (a.Status == "Confirmed" || a.Status == "Completed") &&
                            a.AppointmentDate < DateTime.Today)
                .Include(a => a.Doctor) // Assuming a navigation property
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPendingAppointmentsWithPatientDetailsAsync(string doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Status == "Pending Approval")
                .Include(a => a.Patient) // Assuming a navigation property to Patient_Details
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<AppointmentSlot>> GetDoctorSlotsByDateAsync(string doctorId, DateTime date)
        {
            // Note: This method is less useful for the Schedule API now.
            // Consider renaming it or replacing its usage with GetDoctorSlotsWithAppointmentDetailsAsync.
            return await _context.AppointmentSlots
                .Where(s => s.DoctorId == doctorId && s.StartDateTime.Date == date.Date)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        public void AddAppointmentSlots(IEnumerable<AppointmentSlot> slots)
        {
            _context.AppointmentSlots.AddRange(slots);
        }

        public Task<bool> AppointmentSlotExistsAsync(string doctorId, DateTime startDateTime)
        {
            return _context.AppointmentSlots
                .AnyAsync(s => s.DoctorId == doctorId && s.StartDateTime == startDateTime);
        }

        // ============================ TRANSACTION MANAGEMENT ============================
        public Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }
    }
}