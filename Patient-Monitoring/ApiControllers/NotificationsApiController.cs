using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.Notification;
using Patient_Monitoring.Services.Interface;

namespace Patient_Monitoring.Controllers
{
    /// <summary>
    /// API controller for managing patient notifications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationService service) : ControllerBase
    {
        private readonly INotificationService _service = service;

        /// <summary>
        /// Create and schedule a new notification.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationDTO dto)
        {
            await _service.ScheduleNotificationAsync(dto);
            return Ok(new { message = "Notification scheduled." });
        }

        /// <summary>
        /// Get all notifications for a specific patient.
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(string patientId)
        {
            var notifications = await _service.GetNotificationsAsync(patientId);
            return Ok(notifications);
        }

        /// <summary>
        /// Get a single notification by ID (used for "View Details").
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            return notification == null ? NotFound() : Ok(notification);
        }

        /// <summary>
        /// Mark a notification as read (used for appointments).
        /// </summary>
        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read." });
        }

        /// <summary>
        /// Mark a medication reminder as taken.
        /// </summary>
        [HttpPut("mark-taken/{id}")]
        public async Task<IActionResult> MarkAsTaken(int id)
        {
            await _service.MarkAsTakenAsync(id);
            return Ok(new { message = "Medication marked as taken." });
        }

        /// <summary>
        /// Mark a wellness activity as done.
        /// </summary>
        [HttpPut("mark-done/{id}")]
        public async Task<IActionResult> MarkAsDone(int id)
        {
            await _service.MarkAsDoneAsync(id);
            return Ok(new { message = "Wellness activity marked as done." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteNotificationAsync(id);
            if (!success)
            {
                return NotFound($"Notification with ID {id} not found.");
            }

            return Ok($"Notification with ID {id} deleted successfully.");
        }
    }
}
