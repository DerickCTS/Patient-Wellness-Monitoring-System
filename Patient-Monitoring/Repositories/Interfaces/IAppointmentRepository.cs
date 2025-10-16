using Patient_Monitoring.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage; // Added for IDbContextTransaction

namespace Patient_Monitoring.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        // General Queries
        Task<List<string>> GetDistinctSpecializationsAsync();
        Task<List<Doctor>> GetDoctorsBySpecializationAsync(string specialization);
        Task<List<AppointmentSlot>> GetDoctorAvailableSlotsAsync(string doctorId, DateTime cutoffDate);
        Task<AppointmentSlot> GetSlotForBookingAsync(int slotId, string doctorId);
        Task<Appointment> GetAppointmentByIdIncludeSlotAsync(string appointmentId);
        Task SaveChangesAsync();

        // Appointment Management
        void AddAppointment(Appointment appointment);
        void UpdateAppointmentSlot(AppointmentSlot slot);
        void UpdateAppointment(Appointment appointment);

        // --- NEW METHOD FOR SEQUENTIAL ID GENERATION ---
        Task<string> GetLastSequentialAppointmentIdAsync();
        // ---------------------------------------------

        // Doctor Metrics/Schedule
        Task<int> CountAppointmentsAsync(Expression<Func<Appointment, bool>> predicate);
        Task<int> CountAppointmentSlotsAsync(Expression<Func<AppointmentSlot, bool>> predicate);
        Task<List<Appointment>> GetConfirmedAppointmentsForDoctor(string doctorId, DateTime startDate, DateTime endDate);

        Task<List<AppointmentSlot>> GetDoctorSlotsWithAppointmentDetailsAsync(string doctorId, DateTime date);

        // Patient/Doctor Specific Lists  
        Task<List<Appointment>> GetPatientFutureAppointmentsAsync(string patientId);
        Task<List<Appointment>> GetPatientPastAppointmentsAsync(string patientId);
        Task<List<Appointment>> GetPendingAppointmentsWithPatientDetailsAsync(string doctorId);
        Task<List<AppointmentSlot>> GetDoctorSlotsByDateAsync(string doctorId, DateTime date);
        void AddAppointmentSlots(IEnumerable<AppointmentSlot> slots);
        Task<bool> AppointmentSlotExistsAsync(string doctorId, DateTime startDateTime);

        // Transaction Management
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}