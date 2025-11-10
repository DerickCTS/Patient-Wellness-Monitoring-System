using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.Appointment;
using Patient_Monitoring.DTOs; // Include base DTOs if necessary

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAppointmentService
    {
        // ====================================================================
        // 1. PATIENT ENDPOINTS
        // ====================================================================
        Task<ActionResult<IEnumerable<string>>> GetSpecializationsAsync();
        Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlotsBySpecializationAsync(string specialization);
        Task<IActionResult> BookSlotAsync(AppointmentBookingDto bookingDto);
        Task<ActionResult<object>> GetPatientAppointmentsByStatusAsync(int patientId);
        Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointmentsAsync(int patientId);

        // ====================================================================
        // 2. DOCTOR ENDPOINTS (Appointment Management & Metrics)
        // ====================================================================
        Task<ActionResult<object>> GetDoctorMetricsAsync(int doctorId);
        Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovalsAsync(int doctorId);
        Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorScheduleAsync(int doctorId, DateTime date);
        Task<ActionResult<object>> GetWeeklyScheduleSummaryAsync(int doctorId);
        Task<IActionResult> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDto updateDto);

        Task<List<UpcomingAppointmentDto>> GetUpcomingAppointmentDetailsByDate(DateTime date, int doctorId);

        // --- DEPRECATED: Manual slot creation removed ---
        // Task<IActionResult> ManuallyCreateSlotsAsync(ManualSlotCreationDto model);

        // ====================================================================
        // 3. DOCTOR SCHEDULE MANAGEMENT (AVAILABILITY AND TIME OFF)
        //    Using the renamed DTOs: DoctorAvailabilityDto and DoctorTimeOffDto
        // ====================================================================

        // Doctor Availability (Recurring Schedule)
        Task<IActionResult> SetDoctorAvailabilityAsync(DoctorAvailabilityDto model);
        Task<ActionResult<IEnumerable<DoctorAvailabilityDto>>> GetDoctorAvailabilityAsync(int doctorId);
        Task<IActionResult> DeleteDoctorAvailabilityAsync(int availabilityId);

        // Doctor Time Off (Non-Recurring Blocks)
        Task<IActionResult> SetDoctorTimeOffAsync(DoctorTimeOffDto model);
        Task<ActionResult<IEnumerable<DoctorTimeOffDto>>> GetDoctorTimeOffAsync(int doctorId);
        Task<IActionResult> DeleteDoctorTimeOffAsync(int timeOffId);
    }
}