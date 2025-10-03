using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interface;

namespace Patient_Monitoring.Controllers;

[Route("api/[controller]")]
[ApiController]
/// <summary>
/// Controller responsible for managing API requests related to patient wellness plans.
/// It delegates all business logic to the IWellnessPlanService.
/// </summary>
public class WellnessPlanController : ControllerBase
{
    private readonly IWellnessPlanService _service;

    /// <summary>
    /// Initializes a new instance of the controller, injecting the required service.
    /// </summary>
    public WellnessPlanController(IWellnessPlanService service)
    {
        _service = service;
    }

    #region GET: Retrieving Patient Details
    // GET: api/WellnessPlan/GetPatientDetails?patientId=...&patientName=...
    [HttpGet("GetPatientDetails")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientWellnessDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientWellnessDTO>> GetPatientDetails(string patientId, string patientName)
    {
        var result = await _service.GetPatientDetailsAsync(patientId, patientName);

        if (result == null)
        {
            // Patient was not found by the service
            return NotFound("Patient not found.");
        }

        // Return the full DTO containing all mapped data
        return Ok(result);
    }
    #endregion

    // POST: api/WellnessPlanManagement/AssignPlan
    /// <summary>
    /// Assigns a new or existing wellness plan to a specified patient.
    /// </summary>
    /// <param name="patientId">The ID of the patient receiving the plan.</param>
    /// <param name="newPlan">The details of the plan to assign (used to create or link).</param>
    //[HttpPost("AssignPlan")]
    ////[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlanAssignment))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status409Conflict)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult> AssignPlan(string patientId, [FromBody] Wellness_Plan newPlan)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Delegate assignment and business validation to the service
    //    var (assignedPlanDto, errorMessage) = await _service.AssignPlanAsync(patientId, newPlan);

    //    if (errorMessage != null)
    //    {
    //        // Handle specific business errors returned by the service
    //        if (errorMessage.Contains("Patient not found"))
    //        {
    //            return NotFound(errorMessage);
    //        }
    //        if (errorMessage.Contains("already assigned"))
    //        {
    //            return Conflict(errorMessage);
    //        }
    //        // Catch-all for other service errors
    //        return BadRequest(errorMessage);
    //    }

    //    // Plan successfully assigned and possibly created
    //    // Return 201 Created status with the newly assigned plan DTO
    //    return CreatedAtAction(nameof(GetPatientDetails), new { patientId = patientId, patientName = string.Empty }, assignedPlanDto);
    //}
}