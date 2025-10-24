using Microsoft.AspNetCore.Mvc;

using Patient_Monitoring.Services.Interfaces;
using Patient_Monitoring.DTOs.WellnessPlan;
using Microsoft.AspNetCore.Authorization;


namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class WellnessPlanController : ControllerBase
    {
        private readonly IWellnessPlanService _service;

        public WellnessPlanController(IWellnessPlanService service)
        {
            _service = service;
        }


        #region Retrieve Template Cards
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = await _service.GetAvailableTemplateCards();
            return Ok(templates);
        }
        #endregion


        #region Retreive Template Data For Form Pre-population
        [HttpGet("templates/{planId}")]
        public async Task<IActionResult> GetTemplateDetails(int planId)
        {
            var details = await _service.GetTemplateDetailsForFormAsync(planId);
            if (details == null)
            {
                return NotFound($"Template with ID {planId} not found.");
            }
            return Ok(details);
        }
        #endregion


        #region Assigns a new plan (Template or Scratch) to a patient.
                [HttpPost("assign")]
                public async Task<IActionResult> AssignPlan([FromBody] AssignPlanRequestDto request)
                {
                    // Ensuring Frequency Count is > 0 & Start Date > EndDate
                    if (request.FrequencyCount <= 0 || request.StartDate >= request.EndDate)
                    {
                        return BadRequest("Invalid frequency or dates.");
                    }

                    // The first if is checks if the plan id is null or empty. If it is null, it means the plan was 
                    // created from scratch, so the plan name, goal, and details are required. Otherwise it is a template.
                    if (request.PlanId == null)
                    {
                        if (string.IsNullOrEmpty(request.PlanName) || string.IsNullOrEmpty(request.Goal) || !request.Details.Any())
                        {
                            return BadRequest("Plan Name, Goal, and details are required for 'Create from Scratch'.");
                        }
                    }

                    try
                    {
                        var assignmentId = await _service.AssignNewPlanAsync(request);
                        return CreatedAtAction(nameof(AssignPlan), new { id = assignmentId }, new { message = "Plan assigned successfully." });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "An error occurred while assigning the plan.");
                    }
                }
                #endregion
    }
}
