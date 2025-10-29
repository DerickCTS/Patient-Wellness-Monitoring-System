using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.PatientDashboard;
using Patient_Monitoring.Services.Interfaces;
using System.Security.Claims;
using System.Linq; // Required for FindFirstValue 

namespace Patient_Monitoring.Controllers
{
    // The [Authorize] attribute should be uncommented in a production environment
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Patient")] 
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDashboardDto>> GetDashboardData()
        {
            var patientIdResult = GetPatientIdFromClaims();
            if (patientIdResult.Result != null) return patientIdResult.Result;
            int patientId = patientIdResult.Value;

            var dashboardData = await _dashboardService.GetDashboardData(patientId);

            if (dashboardData == null)
            {
                return NotFound(new { message = $"Patient data not found for ID {patientId}." });
            }

            return Ok(dashboardData);
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

        // 5. WELLNESS PROGRESS: GET: api/PatientDashboard/Progress
        /// <summary>
        /// Retrieves the summary of the patient's current wellness plan progress.
        /// </summary>
        [HttpGet("Progress")]
        [ProducesResponseType(typeof(WellnessProgressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WellnessProgressDto>> GetWellnessProgress()
        {
            var patientIdResult = GetPatientIdFromClaims();
            if (patientIdResult.Result != null) return patientIdResult.Result;
            int patientId = patientIdResult.Value;

            var progress = await _dashboardService.GetWellnessProgress(patientId);

            // Returns a default/empty DTO if no plan is found, as service implementation does.
            return Ok(progress);
        }
    }
}

