//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Patient_Monitoring.ApiControllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DiagnosisApiController : ControllerBase
//    {
//        private readonly IAppointmentService _appointmentService;

//        public AppointmentController(IAppointmentService appointmentService)
//        {
//            _appointmentService = appointmentService;
//        }

//        /// <summary>
//        /// Get today's appointments for a specific doctor
//        /// </summary>
//        /// <param name="doctorId">Doctor ID</param>
//        /// <param name="date">Optional date (defaults to today)</param>
//        /// <param name="status">Appointment status (defaults to "Confirmed")</param>
//        /// <returns>List of appointments</returns>
//        [HttpGet("todays/{doctorId}")]
//        public async Task<IActionResult> GetTodaysAppointments(
//            string doctorId,
//            [FromQuery] DateTime? date = null,
//            [FromQuery] string status = "Confirmed")
//        {
//            try
//            {
//                var appointments = await _appointmentService.GetTodaysAppointmentsAsync(doctorId, date, status);
//                return Ok(new
//                {
//                    success = true,
//                    data = appointments,
//                    count = appointments.Count()
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    success = false,
//                    message = "Error retrieving appointments",
//                    error = ex.Message
//                });
//            }
//        }

//        /// <summary>
//        /// Get appointment details by ID
//        /// </summary>
//        /// <param name="appointmentId">Appointment ID</param>
//        /// <returns>Appointment details</returns>
//        [HttpGet("{appointmentId}")]
//        public async Task<IActionResult> GetAppointmentDetails(string appointmentId)
//        {
//            try
//            {
//                var appointment = await _appointmentService.GetAppointmentDetailsAsync(appointmentId);

//                if (appointment == null)
//                {
//                    return NotFound(new
//                    {
//                        success = false,
//                        message = "Appointment not found"
//                    });
//                }

//                return Ok(new
//                {
//                    success = true,
//                    data = appointment
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    success = false,
//                    message = "Error retrieving appointment details",
//                    error = ex.Message
//                });
//            }
//        }

//        /// <summary>
//        /// Update appointment status
//        /// </summary>
//        /// <param name="appointmentId">Appointment ID</param>
//        /// <param name="request">Status update request</param>
//        /// <returns>Update result</returns>
//        [HttpPatch("{appointmentId}/status")]
//        public async Task<IActionResult> UpdateAppointmentStatus(
//            string appointmentId,
//            [FromBody] UpdateStatusRequest request)
//        {
//            try
//            {
//                var result = await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, request.Status);

//                if (!result)
//                {
//                    return NotFound(new
//                    {
//                        success = false,
//                        message = "Appointment not found"
//                    });
//                }

//                return Ok(new
//                {
//                    success = true,
//                    message = "Appointment status updated successfully"
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    success = false,
//                    message = "Error updating appointment status",
//                    error = ex.Message
//                });
//            }
//        }
//    }

//    public class UpdateStatusRequest
//    {
//        public string Status { get; set; }
//    }
//}
//}
