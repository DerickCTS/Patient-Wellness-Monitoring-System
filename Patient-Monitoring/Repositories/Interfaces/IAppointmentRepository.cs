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
        Task<List<AppointmentSlot>> GetDoctorAvailableSlotsAsync(int doctorId, DateTime cutoffDate);
        Task<AppointmentSlot> GetSlotForBookingAsync(int slotId, int doctorId);
        Task<Appointment> GetAppointmentByIdIncludeSlotAsync(int appointmentId);
        Task SaveChangesAsync();
        void SaveChanges();
        // Appointment Management
        Task AddAppointment(Appointment appointment);
        Task UpdateAppointmentSlot(AppointmentSlot slot);
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
    }
}