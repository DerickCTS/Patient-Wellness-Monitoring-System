using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Patient_Monitoring.DTOs.Appointment;

namespace Patient_Monitoring.Services.Interfaces
{
    public interface IAppointmentService
    {
        // Patient Flow
        Task<ActionResult<IEnumerable<string>>> GetSpecializationsAsync();
        Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlotsBySpecializationAsync(string specialization);
        Task<IActionResult> BookSlotAsync(AppointmentBookingDto bookingDto);
        Task<ActionResult<object>> GetPatientAppointmentsByStatusAsync(string patientId);
        Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointmentsAsync(string patientId);

        // Doctor Flow
        Task<ActionResult<object>> GetDoctorMetricsAsync(string doctorId);
        Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovalsAsync(string doctorId);
        Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorScheduleAsync(string doctorId, DateTime date);
        Task<ActionResult<object>> GetWeeklyScheduleSummaryAsync(string doctorId);
        Task<IActionResult> ManuallyCreateSlotsAsync(ManualSlotCreationDto model);
        Task<IActionResult> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDto updateDto);
    }
}