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

        public async Task<List<AppointmentSlot>> GetDoctorAvailableSlotsAsync(int doctorId, DateTime cutoffDate)
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

        public async Task<AppointmentSlot> GetSlotForBookingAsync(int slotId, int doctorId)
        {
            return await _context.AppointmentSlots
                .FirstOrDefaultAsync(s => s.SlotId == slotId && s.DoctorId == doctorId);
        }

        public async Task<Appointment> GetAppointmentByIdIncludeSlotAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.AppointmentSlot)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves the highest sequential Appointment ID (e.g., A109) from the database.
        /// </summary>
        public async Task<int> GetLastSequentialAppointmentIdAsync()
        {
            var maxId = await _context.Appointments
                             .MaxAsync(a => a.AppointmentId);

            return maxId;
        }

        // ============================ APPOINTMENT MANAGEMENT ============================
        public async Task AddAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        /// <summary>
        /// Updates an appointment slot state without calling SaveChanges,
        /// relying on the calling service layer's transaction to commit.
        /// </summary>
        public void UpdateAppointmentSlot(AppointmentSlot slot) // MODIFIED: Changed to void, removed SaveChangesAsync
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

        public async Task<List<Appointment>> GetConfirmedAppointmentsForDoctor(int doctorId, DateTime startDate, DateTime endDate)
        {
            // Includes Pending for accurate slot counts in Weekly Summary
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate >= startDate &&
                            a.AppointmentDate < endDate &&
                            (a.Status == "Confirmed" || a.Status == "Pending Approval"))
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentDetailsByDate(DateTime date, int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.AppointmentSlot)
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date)
                .ToListAsync();
        }

        // ============================ PATIENT/DOCTOR LISTS ============================

        /// <summary>
        /// Retrieves all slots for a doctor on a specific date, eagerly loading Appointment and Patient details for booked slots.
        /// </summary>
        public async Task<List<AppointmentSlot>> GetDoctorSlotsWithAppointmentDetailsAsync(int doctorId, DateTime date)
        {
            return await _context.AppointmentSlots
                .Where(s => s.DoctorId == doctorId && s.StartDateTime.Date == date.Date)
                .OrderBy(s => s.StartDateTime)
                .Include(s => s.Appointment)
                    .ThenInclude(a => a.Patient)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPatientFutureAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.Today)
                .Include(a => a.Doctor)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPatientPastAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId &&
                            (a.Status == "Confirmed") &&
                            a.AppointmentDate < DateTime.Today)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPendingAppointmentsWithPatientDetailsAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Status == "Pending Approval")
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<AppointmentSlot>> GetDoctorSlotsByDateAsync(int doctorId, DateTime date)
        {
            return await _context.AppointmentSlots
                .Where(s => s.DoctorId == doctorId && s.StartDateTime.Date == date.Date)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        public void AddAppointmentSlots(IEnumerable<AppointmentSlot> slots)
        {
            _context.AppointmentSlots.AddRange(slots);
        }

        public Task<bool> AppointmentSlotExistsAsync(int doctorId, DateTime startDateTime)
        {
            return _context.AppointmentSlots
                .AnyAsync(s => s.DoctorId == doctorId && s.StartDateTime == startDateTime);
        }

        // ============================ TRANSACTION MANAGEMENT ============================
        public Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        // ====================================================================
        // NEW: DOCTOR SCHEDULE MANAGEMENT IMPLEMENTATIONS
        // ====================================================================

        // --- DOCTOR AVAILABILITY (Recurring Schedule) CRUD ---

        public async Task AddDoctorAvailability(DoctorAvailability availability)
        {
            await _context.DoctorAvailabilities.AddAsync(availability);
        }

        public void UpdateDoctorAvailability(DoctorAvailability availability)
        {
            _context.DoctorAvailabilities.Update(availability);
        }

        public Task<DoctorAvailability> GetAvailabilityByIdAsync(int availabilityId)
        {
            return _context.DoctorAvailabilities.FirstOrDefaultAsync(a => a.AvailabilityId == availabilityId);
        }

        public async Task<List<DoctorAvailability>> GetDoctorAvailabilityAsync(int doctorId)
        {
            return await _context.DoctorAvailabilities
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task DeleteDoctorAvailability(int availabilityId)
        {
            var availability = await _context.DoctorAvailabilities.FindAsync(availabilityId);
            if (availability != null)
            {
                _context.DoctorAvailabilities.Remove(availability);
            }
        }

        // --- DOCTOR TIME OFF (Non-Recurring Blocks) CRUD ---

        public async Task AddDoctorTimeOff(DoctorTimeOff timeOff)
        {
            await _context.DoctorTimeOffs.AddAsync(timeOff);
        }

        public void UpdateDoctorTimeOff(DoctorTimeOff timeOff)
        {
            _context.DoctorTimeOffs.Update(timeOff);
        }

        public Task<DoctorTimeOff> GetTimeOffByIdAsync(int timeOffId)
        {
            return _context.DoctorTimeOffs.FirstOrDefaultAsync(t => t.TimeOffId == timeOffId);
        }

        public async Task<List<DoctorTimeOff>> GetDoctorTimeOffAsync(int doctorId)
        {
            return await _context.DoctorTimeOffs
                .Where(t => t.DoctorId == doctorId)
                .OrderBy(t => t.StartDateTime)
                .ToListAsync();
        }

        public async Task DeleteDoctorTimeOff(int timeOffId)
        {
            var timeOff = await _context.DoctorTimeOffs.FindAsync(timeOffId);
            if (timeOff != null)
            {
                _context.DoctorTimeOffs.Remove(timeOff);
            }
        }
    }
}
