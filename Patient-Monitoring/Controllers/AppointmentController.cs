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
    //[Authorize]
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
        //[Authorize(Roles = "Patient")]
        [HttpGet("specializations")]
        public Task<ActionResult<IEnumerable<string>>> GetSpecializations()
        {
            return _appointmentService.GetSpecializationsAsync();
        }

        // GET /api/appointment/doctors/slots/specialization/{specialization}
        [HttpGet("doctors/slots/specialization/{specialization}")]
        //[Authorize(Roles = "Patient")]
        public Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlots(string specialization)
        {
            return _appointmentService.GetDoctorsAndSlotsBySpecializationAsync(specialization);
        }

        [HttpGet("doctor/upcoming-appointments")]
        public async Task<ActionResult<List<UpcomingAppointmentDto>>> GetUpcomingAppointmentDetailsByDate([FromQuery] DateTime date, [FromQuery] int doctorId) {
            var items = await _appointmentService.GetUpcomingAppointmentDetailsByDate(date, doctorId);
            
            if(items.Count == 0)
            {
                return Ok("No upcoming appointments found for the specified date.");
            }
            return Ok(items);
        }

        // POST /api/appointment/book
        [HttpPost("book")]
        //[Authorize(Roles = "Patient")]
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
        //[Authorize(Roles = "Patient")]
        public Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointments(int patientId)
        {
            return _appointmentService.GetPastAppointmentsAsync(patientId);
        }

        // ====================================================================
        // 2. DOCTOR ENDPOINTS
        // ====================================================================

        // GET /api/appointment/doctor/{doctorId}/metrics
        //[Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}/metrics")]
        public Task<ActionResult<object>> GetDoctorMetrics(int doctorId)
        {
            return _appointmentService.GetDoctorMetricsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/pending
        [HttpGet("doctor/{doctorId}/pending")]
        //[Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovals(int doctorId)
        {
            return _appointmentService.GetPendingApprovalsAsync(doctorId);
        }

        // GET /api/appointment/doctor/{doctorId}/schedule?date=yyyy-MM-dd
        [HttpGet("doctor/{doctorId}/schedule")]
        //[Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorSchedule(int doctorId, [FromQuery] DateTime date)
        {
            return _appointmentService.GetDoctorScheduleAsync(doctorId, date);
        }

        // GET /api/appointment/doctor/{doctorId}/weekly-summary
        [HttpGet("doctor/{doctorId}/weekly-summary")]
        //[Authorize(Roles = "Doctor")]
        public Task<ActionResult<object>> GetWeeklyScheduleSummary(int doctorId)
        {
            return _appointmentService.GetWeeklyScheduleSummaryAsync(doctorId);
        }


        // POST /api/appointment/status - DOCTOR'S ACTION: Approve or Reject
        [HttpPost("status")]
        //[Authorize(Roles = "Doctor")]
        public Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto updateDto)
        {
            return _appointmentService.UpdateAppointmentStatusAsync(updateDto);
        }

        // --- DOCTOR AVAILABILITY (RECURRING SCHEDULE) ---

        // POST /api/appointment/doctor/availability
        [HttpPost("doctor/availability")]
        //[Authorize(Roles = "Doctor")]
        public Task<IActionResult> SetAvailability([FromBody] DoctorAvailabilityDto model)
        {
            // Handles both creation and update based on AvailabilityId being present or null
            return _appointmentService.SetDoctorAvailabilityAsync(model);
        }

        // GET /api/appointment/doctor/{doctorId}/availability
        [HttpGet("doctor/{doctorId}/availability")]
        //[Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<DoctorAvailabilityDto>>> GetAvailability(int doctorId)
        {
            return _appointmentService.GetDoctorAvailabilityAsync(doctorId);
        }

        // DELETE /api/appointment/doctor/availability/{availabilityId}
        [HttpDelete("doctor/availability/{availabilityId}")]
        //[Authorize(Roles = "Doctor")]
        public Task<IActionResult> DeleteAvailability(int availabilityId)
        {
            return _appointmentService.DeleteDoctorAvailabilityAsync(availabilityId);
        }

        // --- DOCTOR TIME OFF (SPECIFIC BLOCKS) ---

        // POST /api/appointment/doctor/time-off
        [HttpPost("doctor/time-off")]
        //[Authorize(Roles = "Doctor")]
        public Task<IActionResult> SetTimeOff([FromBody] DoctorTimeOffDto model)
        {
            // Handles both creation and update based on TimeOffId being present or null
            return _appointmentService.SetDoctorTimeOffAsync(model);
        }

        // GET /api/appointment/doctor/{doctorId}/time-off
        [HttpGet("doctor/{doctorId}/time-off")]
        //[Authorize(Roles = "Doctor")]
        public Task<ActionResult<IEnumerable<DoctorTimeOffDto>>> GetTimeOff(int doctorId)
        {
            return _appointmentService.GetDoctorTimeOffAsync(doctorId);
        }

        // DELETE /api/appointment/doctor/time-off/{timeOffId}
        [HttpDelete("doctor/time-off/{timeOffId}")]
        //[Authorize(Roles = "Doctor")]
        public Task<IActionResult> DeleteTimeOff(int timeOffId)
        {
            return _appointmentService.DeleteDoctorTimeOffAsync(timeOffId);
        }
    }
}