using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // You would add this to protect the endpoint
public class ProgressApiController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressApiController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    private string GetCurrentPatientId()
    {
        // In a real app, you would get this from the user's token claims
        // For example: return User.FindFirstValue(ClaimTypes.NameIdentifier);
        return "patient_id_from_dummy_data"; // Replace with a real ID from your seeded data
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetAssignedPlanCards(
        [FromQuery] string status = "All",
        [FromQuery] string category = "All",
        [FromQuery] string date = "This Week")
    {
        var patientId = GetCurrentPatientId();
        var cards = await _progressService.GetAssignedPlanCardsAsync(patientId, status, category, date);
        return Ok(cards);
    }

    [HttpGet("plans/{assignmentId}/details")]
    public async Task<IActionResult> GetPlanDetails(string assignmentId)
    {
        var details = await _progressService.GetPlanDetailsAsync(assignmentId);
        if (details == null)
        {
            return NotFound();
        }
        return Ok(details);
    }

    [HttpPatch("tasks/{taskLogId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(string taskLogId, [FromBody] UpdateTaskStatusDto updateDto)
    {
        var success = await _progressService.UpdateTaskStatusAsync(taskLogId, updateDto);
        if (!success)
        {
            return NotFound("Task log not found or failed to update.");
        }
        return NoContent(); // Success, no content to return
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardData()
    {
        var patientId = GetCurrentPatientId();
        var dashboardData = await _progressService.GetDashboardDataAsync(patientId);
        return Ok(dashboardData);
    }
}