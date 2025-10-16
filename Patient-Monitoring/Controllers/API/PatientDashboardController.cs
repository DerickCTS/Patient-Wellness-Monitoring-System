using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Services.Interfaces;
using System.Security.Claims;

namespace Patient_Monitoring.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensure user is authenticated
    public class PatientDashboardController : ControllerBase
    {
        private readonly IPatientDashboardService _dashboardService;
        private readonly ILogger<PatientDashboardController> _logger;

        public PatientDashboardController(
            IPatientDashboardService dashboardService,
            ILogger<PatientDashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get complete dashboard data for a patient
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Complete dashboard data including patient info, doctor, prescriptions, and task logs</returns>
        [HttpGet("{patientId}")]
        [ProducesResponseType(typeof(PatientDashboardDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardData(string patientId)
        {
            try
            {
                // Optional: Verify the authenticated user matches the requested patient
                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var dashboardData = await _dashboardService.GetPatientDashboardDataAsync(patientId);

                if (dashboardData == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting dashboard data for patient: {patientId}");
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard data" });
            }
        }

        /// <summary>
        /// Get patient information only
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Patient information</returns>
        [HttpGet("{patientId}/info")]
        [ProducesResponseType(typeof(PatientInfoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientInfo(string patientId)
        {
            try
            {
                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var patientInfo = await _dashboardService.GetPatientInfoAsync(patientId);

                if (patientInfo == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                return Ok(patientInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting patient info: {patientId}");
                return StatusCode(500, new { message = "An error occurred while retrieving patient information" });
            }
        }

        /// <summary>
        /// Update patient information
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <param name="updateDTO">Updated patient information</param>
        /// <returns>Updated patient information</returns>
        [HttpPut("{patientId}/info")]
        [ProducesResponseType(typeof(PatientInfoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePatientInfo(string patientId, [FromBody] UpdatePatientInfoDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var updatedInfo = await _dashboardService.UpdatePatientInfoAsync(patientId, updateDTO);

                if (updatedInfo == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                return Ok(updatedInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating patient info: {patientId}");
                return StatusCode(500, new { message = "An error occurred while updating patient information" });
            }
        }

        /// <summary>
        /// Upload patient profile image
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <param name="uploadDTO">Base64 encoded image</param>
        /// <returns>Image URL or base64 string</returns>
        [HttpPost("{patientId}/profile-image")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadProfileImage(string patientId, [FromBody] UploadProfileImageDTO uploadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                if (string.IsNullOrWhiteSpace(uploadDTO.Base64Image))
                {
                    return BadRequest(new { message = "Base64Image is required." });
                }

                var imageUrl = await _dashboardService.UploadProfileImageAsync(patientId, uploadDTO.Base64Image);

                if (imageUrl == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                return Ok(new { profileImage = imageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading profile image: {patientId}");
                return StatusCode(500, new { message = "An error occurred while uploading the profile image" });
            }
        }

        /// <summary>
        /// Get assigned doctor information
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>Assigned doctor details</returns>
        [HttpGet("{patientId}/doctor")]
        [ProducesResponseType(typeof(AssignedDoctorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignedDoctor(string patientId)
        {
            try
            {
                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var doctor = await _dashboardService.GetAssignedDoctorAsync(patientId);

                if (doctor == null)
                {
                    return NotFound(new { message = "No assigned doctor found" });
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting assigned doctor: {patientId}");
                return StatusCode(500, new { message = "An error occurred while retrieving doctor information" });
            }
        }

        /// <summary>
        /// Get active prescriptions
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>List of active prescriptions</returns>
        [HttpGet("{patientId}/prescriptions")]
        [ProducesResponseType(typeof(List<PrescriptionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActivePrescriptions(string patientId)
        {
            try
            {
                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var prescriptions = await _dashboardService.GetActivePrescriptionsAsync(patientId);
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting prescriptions: {patientId}");
                return StatusCode(500, new { message = "An error occurred while retrieving prescriptions" });
            }
        }

        /// <summary>
        /// Get recent task logs
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <param name="limit">Number of records to return (default: 10)</param>
        /// <returns>List of recent task logs</returns>
        [HttpGet("{patientId}/task-logs")]
        [ProducesResponseType(typeof(List<DailyTaskLogDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentTaskLogs(string patientId, [FromQuery] int limit = 10)
        {
            try
            {
                var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (authenticatedUserId != patientId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var taskLogs = await _dashboardService.GetRecentTaskLogsAsync(patientId, limit);
                return Ok(taskLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting task logs: {patientId}");
                return StatusCode(500, new { message = "An error occurred while retrieving task logs" });
            }
        }
    }
}
