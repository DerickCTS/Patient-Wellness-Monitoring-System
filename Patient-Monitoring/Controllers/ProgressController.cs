using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // You would add this to protect the endpoint
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;
    private readonly int patientId;
    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
        patientId = GetCurrentPatientId();
    }

    #region Initialize - Get Current Patient Id from JWT Token
    private int GetCurrentPatientId()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found in token.");
        }

        if (int.TryParse(userIdClaim.Value, out int patientId))
        {
            return patientId;
        }
        else
        {
            throw new UnauthorizedAccessException("Invalid User ID claim value.");
        }

        // Use this to hard code a doctor id for testing purposes. But comment the above codes.
        //return 1;
    }
    #endregion


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