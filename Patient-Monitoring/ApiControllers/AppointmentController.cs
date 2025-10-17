using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Patient_Monitoring.DTOs.Appointment;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // ====================================================================
        // 1. PATIENT ENDPOINTS
        // ====================================================================

        // GET /api/appointment/specializations
        [HttpGet("specializations")]
        public Task<ActionResult<IEnumerable<string>>> GetSpecializations()
        {
            return _appointmentService.GetSpecializationsAsync();
        }

        // GET /api/appointment/doctors/slots/specialization/{specialization}
        [HttpGet("doctors/slots/specialization/{specialization}")]
        public Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlots(string specialization)
        {
            return _appointmentService.GetDoctorsAndSlotsBySpecializationAsync(specialization);
        }

        // POST /api/appointment/book
        [HttpPost("book")]
        public Task<IActionResult> BookSlot([FromBody] AppointmentBookingDto bookingDto)
        {
            return _appointmentService.BookSlotAsync(bookingDto);
        }

        // GET /api/appointment/patient/{patientId}/status
        [HttpGet("patient/{patientId}/status")]
        public Task<ActionResult<object>> GetPatientAppointmentsByStatus(string patientId)
        {
            return _appointmentService.GetPatientAppointmentsByStatusAsync(patientId);
        }

        // GET /api/appointment/patient/{patientId}/history
        [HttpGet("patient/{patientId}/history")]
        public Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointments(string patientId)
        {
            return _appointmentService.GetPastAppointmentsAsync(patientId);
        }

        // ====================================================================
        // 2. DOCTOR ENDPOINTS
        // ====================================================================

        // GET /api/appointment/doctor/{doctorId}/metrics
        [HttpGet("doctor/{doctorId}/metrics")]
        public Task<ActionResult<object>> GetDoctorMetrics(string doctorId)
        {
            return _appointmentService.GetDoctorMetricsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/pending
        [HttpGet("doctor/{doctorId}/pending")]
        public Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovals(string doctorId)
        {
            return _appointmentService.GetPendingApprovalsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/schedule?date=yyyy-MM-dd
        [HttpGet("doctor/{doctorId}/schedule")]
        public Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorSchedule(string doctorId, [FromQuery] DateTime date)
        {
            return _appointmentService.GetDoctorScheduleAsync(doctorId, date);
        }

        // GET /api/appointment/doctor/{doctorId}/weekly-summary
        [HttpGet("doctor/{doctorId}/weekly-summary")]
        public Task<ActionResult<object>> GetWeeklyScheduleSummary(string doctorId)
        {
            return _appointmentService.GetWeeklyScheduleSummaryAsync(doctorId);
        }

        // POST /api/appointment/doctor/manage-slots/manual
        [HttpPost("doctor/manage-slots/manual")]
        public Task<IActionResult> ManuallyCreateSlots([FromBody] ManualSlotCreationDto model)
        {
            return _appointmentService.ManuallyCreateSlotsAsync(model);
        }

        // POST /api/appointment/status - DOCTOR'S ACTION: Approve or Reject
        [HttpPost("status")]
        public Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto updateDto)
        {
            return _appointmentService.UpdateAppointmentStatusAsync(updateDto);
        }

        // DEPRECATED ENDPOINTS (Kept for completeness, though they should be removed)
        [HttpGet("slots/{doctorId}")]
        public IActionResult GetAvailableSlots(string doctorId)
        {
            // Note: Service layer call removed, as it's deprecated
            return StatusCode(410, "This endpoint is deprecated. Use GET /api/appointment/doctors/slots/specialization/{specialization} instead.");
        }

        [HttpGet("pending/{doctorId}")]
        public IActionResult GetPendingAppointments(string doctorId)
        {
            // Note: Service layer call removed, as it's deprecated
            return StatusCode(410, "This endpoint is deprecated. Use GET /api/appointment/doctor/{doctorId}/pending instead.");
        }
    }
}