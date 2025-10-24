using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Patient_Monitoring.DTOs.Appointment;
using Microsoft.AspNetCore.Authorization;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        // GET /api/appointment/specializations
        [Authorize(Roles = "Patient")]
        [HttpGet("specializations")]
        public Task<ActionResult<IEnumerable<string>>> GetSpecializations()
        {
            return _appointmentService.GetSpecializationsAsync();
        }

        // GET /api/appointment/doctors/slots/specialization/{specialization}
        [HttpGet("doctors/slots/specialization/{specialization}")]
        [Authorize(Roles = "Patient")]
        public Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlots(string specialization)
        {
            return _appointmentService.GetDoctorsAndSlotsBySpecializationAsync(specialization);
        }

        // POST /api/appointment/book
        [HttpPost("book")]
        [Authorize(Roles = "Patient")]
        public Task<IActionResult> BookSlot([FromBody] AppointmentBookingDto bookingDto)
        {
            return _appointmentService.BookSlotAsync(bookingDto);
        }

        // GET /api/appointment/patient/{patientId}/status
        [HttpGet("patient/{patientId}/status")]
        [Authorize(Roles = "Patient")]
        public Task<ActionResult<object>> GetPatientAppointmentsByStatus(int patientId)
        {
            return _appointmentService.GetPatientAppointmentsByStatusAsync(patientId);
        }

        // GET /api/appointment/patient/{patientId}/history
        [HttpGet("patient/{patientId}/history")]
        [Authorize(Roles = "Patient")]
        public Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointments(int patientId)
        {
            return _appointmentService.GetPastAppointmentsAsync(patientId);
        }

        // ====================================================================
        // 2. DOCTOR ENDPOINTS
        // ====================================================================

        // GET /api/appointment/doctor/{doctorId}/metrics
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}/metrics")]
        public Task<ActionResult<object>> GetDoctorMetrics(int doctorId)
        {
            return _appointmentService.GetDoctorMetricsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/pending
        [HttpGet("doctor/{doctorId}/pending")]
        [Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovals(int doctorId)
        {
            return _appointmentService.GetPendingApprovalsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/schedule?date=yyyy-MM-dd
        [HttpGet("doctor/{doctorId}/schedule")]
        [Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorSchedule(int doctorId, [FromQuery] DateTime date)
        {
            return _appointmentService.GetDoctorScheduleAsync(doctorId, date);
        }

        // GET /api/appointment/doctor/{doctorId}/weekly-summary
        [HttpGet("doctor/{doctorId}/weekly-summary")]
        [Authorize(Roles = "Doctor")]
        public Task<ActionResult<object>> GetWeeklyScheduleSummary(int doctorId)
        {
            return _appointmentService.GetWeeklyScheduleSummaryAsync(doctorId);
        }

        // POST /api/appointment/doctor/manage-slots/manual
        [HttpPost("doctor/manage-slots/manual")]
        [Authorize(Roles = "Doctor")]
        public Task<IActionResult> ManuallyCreateSlots([FromBody] ManualSlotCreationDto model)
        {
            return _appointmentService.ManuallyCreateSlotsAsync(model);
        }

        // POST /api/appointment/status - DOCTOR'S ACTION: Approve or Reject
        [HttpPost("status")]
        [Authorize(Roles = "Doctor")]
        public Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto updateDto)
        {
            return _appointmentService.UpdateAppointmentStatusAsync(updateDto);
        }

        // DEPRECATED ENDPOINTS (Kept for completeness, though they should be removed)
        [HttpGet("slots/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public IActionResult GetAvailableSlots(string doctorId)
        {
            // Note: Service layer call removed, as it's deprecated
            return StatusCode(410, "This endpoint is deprecated. Use GET /api/appointment/doctors/slots/specialization/{specialization} instead.");
        }

        [HttpGet("pending/{doctorId}")]
        [Authorize(Roles = "Doctor")]
        public IActionResult GetPendingAppointments(string doctorId)
        {
            // Note: Service layer call removed, as it's deprecated
            return StatusCode(410, "This endpoint is deprecated. Use GET /api/appointment/doctor/{doctorId}/pending instead.");
        }
    }
}