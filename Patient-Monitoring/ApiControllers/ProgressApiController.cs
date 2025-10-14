using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // You would add this to protect the endpoint
public class ProgressApiController : ControllerBase
{
    private readonly IProgressService _progressService;
    private readonly string patientId;
    public ProgressApiController(IProgressService progressService)
    {
        _progressService = progressService;
        patientId = GetCurrentPatientId();
    }

    private string GetCurrentPatientId()
    {
        var patientId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(patientId))
        {
            throw new InvalidOperationException("Patient ID claim (sub) not found in token.");
        }

        return patientId;
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetAssignedPlanCards(
        [FromQuery] string status = "All",
        [FromQuery] string category = "All",
        [FromQuery] string date = "This Week")
    {
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