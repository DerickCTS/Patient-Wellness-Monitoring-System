using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.Notification;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(INotificationService service) : ControllerBase
    {
        private readonly INotificationService _service = service;


        #region Create and schedule a new notification.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationDTO dto)
        {
            await _service.ScheduleNotificationAsync(dto);
            return Ok(new { message = "Notification scheduled." });
        }
        #endregion


        #region Get all notifications for a specific patient.
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var notifications = await _service.GetNotificationsAsync(patientId);
            return Ok(notifications);
        }
        #endregion


        #region Get a single notification by ID (used for "View Details")
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            return notification == null ? NotFound() : Ok(notification);
        }
        #endregion


        #region Mark a notification as read (used for appointments).
        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read." });
        }
        #endregion


        #region Mark a medication reminder as taken.
        [HttpPut("mark-taken/{id}")]
        public async Task<IActionResult> MarkAsTaken(int id)
        {
            await _service.MarkAsTakenAsync(id);
            return Ok(new { message = "Medication marked as taken." });
        }
        #endregion


        #region Mark a wellness activity as done.
        /// Derick - The same is being tracked in WellnessService. Is this redundant? Or I could trigger
        /// mark as done in WellnessService from here?
        [HttpPut("mark-done/{id}")]
        public async Task<IActionResult> MarkAsDone(int id)
        {
            await _service.MarkAsDoneAsync(id);
            return Ok(new { message = "Wellness activity marked as done." });
        }
        #endregion


        #region Delete a notification by ID.
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
        #endregion
    }
}
