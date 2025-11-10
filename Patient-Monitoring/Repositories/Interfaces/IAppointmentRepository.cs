using Microsoft.EntityFrameworkCore.Storage; // Added for IDbContextTransaction
using Patient_Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        // General Queries
        Task<List<string>> GetDistinctSpecializationsAsync();
        Task<List<Doctor>> GetDoctorsBySpecializationAsync(string specialization);
        Task<List<AppointmentSlot>> GetDoctorAvailableSlotsAsync(int doctorId, DateTime cutoffDate);
        Task<AppointmentSlot> GetSlotForBookingAsync(int slotId, int doctorId);
        Task<Appointment> GetAppointmentByIdIncludeSlotAsync(int appointmentId);
        Task SaveChangesAsync();
        void SaveChanges();
        // Appointment Management
        Task AddAppointment(Appointment appointment);
        void UpdateAppointmentSlot(AppointmentSlot slot);
        void UpdateAppointment(Appointment appointment);

        // --- NEW METHOD FOR SEQUENTIAL ID GENERATION ---
        Task<int> GetLastSequentialAppointmentIdAsync();
        // ---------------------------------------------

        // Doctor Metrics/Schedule
        Task<int> CountAppointmentsAsync(Expression<Func<Appointment, bool>> predicate);
        Task<int> CountAppointmentSlotsAsync(Expression<Func<AppointmentSlot, bool>> predicate);
        Task<List<Appointment>> GetConfirmedAppointmentsForDoctor(int doctorId, DateTime startDate, DateTime endDate);

        Task<List<AppointmentSlot>> GetDoctorSlotsWithAppointmentDetailsAsync(int doctorId, DateTime date);

        // Patient/Doctor Specific Lists  
        Task<List<Appointment>> GetPatientFutureAppointmentsAsync(int patientId);
        Task<List<Appointment>> GetPatientPastAppointmentsAsync(int patientId);
        Task<List<Appointment>> GetPendingAppointmentsWithPatientDetailsAsync(int doctorId);
        Task<List<AppointmentSlot>> GetDoctorSlotsByDateAsync(int doctorId, DateTime date);
        void AddAppointmentSlots(IEnumerable<AppointmentSlot> slots);
        Task<bool> AppointmentSlotExistsAsync(int doctorId, DateTime startDateTime);

        // Transaction Management
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<Appointment>> GetUpcomingAppointmentDetailsByDate(DateTime date, int doctorId);

        // NEW: DOCTOR SCHEDULE MANAGEMENT
        // ====================================================================

        // Doctor Availability (Recurring Schedule) CRUD
        Task AddDoctorAvailability(DoctorAvailability availability);
        void UpdateDoctorAvailability(DoctorAvailability availability);
        Task<DoctorAvailability> GetAvailabilityByIdAsync(int availabilityId);
        Task<List<DoctorAvailability>> GetDoctorAvailabilityAsync(int doctorId);
        Task DeleteDoctorAvailability(int availabilityId);

        // Doctor Time Off (Non-Recurring Blocks) CRUD
        Task AddDoctorTimeOff(DoctorTimeOff timeOff);
        void UpdateDoctorTimeOff(DoctorTimeOff timeOff);
        Task<DoctorTimeOff> GetTimeOffByIdAsync(int timeOffId);
        Task<List<DoctorTimeOff>> GetDoctorTimeOffAsync(int doctorId);
        Task DeleteDoctorTimeOff(int timeOffId);
    }
}