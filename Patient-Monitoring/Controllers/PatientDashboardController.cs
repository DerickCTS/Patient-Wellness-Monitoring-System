using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.PatientDashboard;
using System.Security.Claims;
using System.Linq;
using Patient_Monitoring.Services.Interfaces; 
using System.Threading.Tasks; 
using System.Collections.Generic; 

namespace Patient_Monitoring.Controllers
{
    // The [Authorize] attribute should be uncommented in a production environment
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Patient")] 
    public class PatientDashboardController : ControllerBase
    {
        private readonly IPatientDashboardService _dashboardService;

        public PatientDashboardController(IPatientDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Helper method to retrieve and validate the Patient ID from claims
        private ActionResult<int> GetPatientIdFromClaims()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idClaim))
            {
                return Unauthorized("User ID claim missing.");
            }

            if (!int.TryParse(idClaim, out int patientId))
            {
                return Unauthorized("Invalid user ID format.");
            }

            return patientId;
        }

        // 1. MONOLITHIC ENDPOINT: GET: api/PatientDashboard/Data
        /// <summary>
        /// Retrieves the comprehensive dashboard data for the authenticated patient.
        /// </summary>
        /// <returns>A PatientDashboardDTO object.</returns>
        [HttpGet("Data")]
        [ProducesResponseType(typeof(PatientDashboardDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDashboardDto>> GetDashboardData()
        {
            // Commented out for testing without authentication
            //var patientIdResult = GetPatientIdFromClaims();
            //if (patientIdResult.Result != null) return patientIdResult.Result;
            //int patientId = patientIdResult.Value;

            int patientId = 3;
            var dashboardData = await _dashboardService.GetDashboardData(patientId);

            if (dashboardData == null)
            {
                return NotFound(new { message = $"Patient data not found for ID {patientId}." });
            }

            return Ok(dashboardData);
        }

        [HttpPost("Profile")]
        /// <summary>
        /// Allows the authenticated patient to update their profile details.
        /// </summary>
        /// <param name="updateDto">The updated profile data.</param>
        /// <returns>200 OK on success, 400 on validation error, 404 if patient not found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePatientProfile([FromBody] UpdatePatientProfileDto updateDto)
        {
            // 1. Validate DTO model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. Get Patient ID from claims
            //var patientIdResult = GetPatientIdFromClaims();
            //if (patientIdResult.Result != null) return patientIdResult.Result;
            //int patientId = patientIdResult.Value;

            int patientId = 3;

            // 3. Call Service
            var success = await _dashboardService.UpdatePatientProfile(patientId, updateDto);

            if (!success)
            {
                // This means the service layer returned false, either because the patient ID was invalid 
                // or a database error occurred during the update.
                return NotFound(new { message = $"Profile update failed. Patient not found or a database error occurred for ID {patientId}." });
            }

            return Ok(new { message = "Profile updated successfully." });
        }

        // 2. UPCOMING APPOINTMENTS: GET: api/PatientDashboard/Appointments
        /// <summary>
        /// Retrieves a list of upcoming appointments for the authenticated patient.
        /// </summary>
        [HttpGet("Appointments")]
        [ProducesResponseType(typeof(List<AppointmentSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<AppointmentSummaryDto>>> GetUpcomingAppointments()
        {
            var patientIdResult = GetPatientIdFromClaims();
            if (patientIdResult.Result != null) return patientIdResult.Result;
            int patientId = patientIdResult.Value;

            var appointments = await _dashboardService.GetUpcomingAppointments(patientId);

            // Always return an empty list if none are found, rather than 404
            return Ok(appointments);
        }

        // 3. ACTIVE PRESCRIPTIONS: GET: api/PatientDashboard/Prescriptions
        /// <summary>
        /// Retrieves a list of active prescriptions for the authenticated patient.
        /// </summary>
        [HttpGet("Prescriptions")]
        [ProducesResponseType(typeof(List<PrescriptionSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PrescriptionSummaryDto>>> GetActivePrescriptions()
        {
            var patientIdResult = GetPatientIdFromClaims();
            if (patientIdResult.Result != null) return patientIdResult.Result;
            int patientId = patientIdResult.Value;

            var prescriptions = await _dashboardService.GetActivePrescriptions(patientId);

            return Ok(prescriptions);
        }

        // 4. RECENT ACTIVITIES: GET: api/PatientDashboard/Activities
        /// <summary>
        /// Retrieves a list of recently completed task logs/activities for the authenticated patient.
        /// </summary>
        [HttpGet("Activities")]
        [ProducesResponseType(typeof(List<TaskLogSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<TaskLogSummaryDto>>> GetRecentActivities()
        {
            var patientIdResult = GetPatientIdFromClaims();
            if (patientIdResult.Result != null) return patientIdResult.Result;
            int patientId = patientIdResult.Value;

            var activities = await _dashboardService.GetRecentActivities(patientId);

            return Ok(activities);
        }

        
    }
}

