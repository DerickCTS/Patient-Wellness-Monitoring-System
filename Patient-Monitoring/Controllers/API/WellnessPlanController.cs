using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Patient_Monitoring.Services.Interface;


namespace Patient_Monitoring.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellnessPlanController : ControllerBase
    {
        private readonly IWellnessPlanService _service;

        public WellnessPlanController(IWellnessPlanService service)
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

        // --- Assignment Endpoint (Handles both flows) ---

        /// <summary>
        /// POST: api/AssignedWellnessPlan/assign - Assigns a new plan (Template or Scratch) to a patient.
        /// </summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPlan([FromBody] AssignPlanRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Basic validation check for assignment details
            if (request.FrequencyCount <= 0 || request.StartDate >= request.EndDate)
            {
                return BadRequest("Invalid frequency or dates.");
            }

            // Validation based on flow
            if (string.IsNullOrEmpty(request.PlanId))
            {
                // Create from Scratch flow: requires name, goal, and details
                if (string.IsNullOrEmpty(request.PlanName) || string.IsNullOrEmpty(request.Goal) || !request.Details.Any())
                {
                    return BadRequest("Plan Name, Goal, and details are required for 'Create from Scratch'.");
                }
            }
            else
            {
                // Use Template flow: does not require PlanName/Goal/ImageUrl
                request.PlanName = null;
                request.Goal = null;
                request.ImageUrl = null;
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
