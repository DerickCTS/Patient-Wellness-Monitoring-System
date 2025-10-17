using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // You would add this to protect the endpoint
public class ProgressApiController : ControllerBase
{
    private readonly IProgressService _progressService;
    private readonly int patientId;
    public ProgressApiController(IProgressService progressService)
    {
        _progressService = progressService;
        patientId = GetCurrentPatientId();
    }

    private int GetCurrentPatientId()
    {
        //var patientId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

        return 15;
    }
    #region Retrieve Plan Task Cards
    [HttpGet("plans")]
    public async Task<IActionResult> GetAssignedPlanCards(
        [FromQuery] string status = "All",
        [FromQuery] string category = "All",
        [FromQuery] string date = "Today")
    {
        var cards = await _progressService.GetAssignedPlanCardsAsync(patientId, status, category, date);
        return Ok(cards);
    }
    #endregion


    #region Retrieve Plan Card Details
    [HttpGet("plans/{assignmentId}/details")]
    public async Task<IActionResult> GetPlanDetails(int assignmentId)
    {
        var details = await _progressService.GetPlanDetailsAsync(assignmentId);
        if (details == null)
        {
            return NotFound();
        }
        return Ok(details);
    }
    #endregion


    #region Updating Plan Task Status
    [HttpPatch("tasks/{taskLogId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int taskLogId, [FromBody] UpdateTaskStatusDto updateDto)
    {
        var success = await _progressService.UpdateTaskStatusAsync(taskLogId, updateDto);
        if (!success)
        {
            return NotFound("Task log not found or failed to update.");
        }
        return Ok("Status updated successfully");
    }
    #endregion


    #region Retrive Dashboard Details
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardData()
    {
        var dashboardData = await _progressService.GetDashboardDataAsync(patientId);
        return Ok(dashboardData);
    }
    #endregion


    #region Retrieve Activity Calendar Data
    [HttpGet("dashboard/{year}")]
    public async Task<IActionResult> GetActivityCalendarAsync(int year)
    {
        var calendarData = await _progressService.GetActivityCalendarAsync(patientId, year);
        return Ok(calendarData);
    }
    #endregion
}