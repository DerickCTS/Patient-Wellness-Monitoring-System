using Microsoft.AspNetCore.Mvc;

using Patient_Monitoring.Services.Interfaces;
using Patient_Monitoring.DTOs.WellnessPlan;


namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellnessPlanApiController : ControllerBase
    {
        private readonly IWellnessPlanService _service;

        public WellnessPlanApiController(IWellnessPlanService service)
        {
            _service = service;
        }


        #region Retrieve Template Cards
        /// GET: api/AssignedWellnessPlan/templates - Gets list of templates for the card view.
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = await _service.GetAvailableTemplateCards();
            return Ok(templates);
        }
        #endregion


        #region Retreive Template Data For Form Pre-population
        /// GET: api/AssignedWellnessPlan/templates/{planId} - Gets full template details for form prepopulation.
        [HttpGet("templates/{planId}")]
        public async Task<IActionResult> GetTemplateDetails(string planId)
        {
            var details = await _service.GetTemplateDetailsForFormAsync(planId);
            if (details == null)
            {
                return NotFound($"Template with ID {planId} not found.");
            }
            return Ok(details);
        }
        #endregion



        /// POST: api/AssignedWellnessPlan/assign - Assigns a new plan (Template or Scratch) to a patient.
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
            if (string.IsNullOrEmpty(request.PlanId))
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
                // Log exception
                return StatusCode(500, "An error occurred while assigning the plan.");
            }
        }
    }
}
