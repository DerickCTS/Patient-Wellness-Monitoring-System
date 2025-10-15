using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interface;
using System.Net;

namespace Patient_Monitoring.Controllers.API;

[Route("api/[controller]")]
[ApiController]

public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;


    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    [HttpGet("patient/search")]
    public async Task<ActionResult<List<PatientSearchItemDto>>> SearchPatients([FromQuery] string? patientName, [FromQuery] string? patientId)
    {
        if (string.IsNullOrWhiteSpace(patientName) && string.IsNullOrEmpty(patientId))
        {
            return Ok(new List<PatientSearchItemDto>());
        }

        var searchResults = await _doctorService.SearchPatientsAsync(patientName, patientId);

        return Ok(searchResults);
    }
    [HttpGet("patients/{patientId}")]
    public async Task<ActionResult<PatientFullDetailDto>> GetPatientDetails(string patientId)
    {
        var patientDetails = await _doctorService.GetPatientDetailsAsync(patientId);

        if (patientDetails == null)
        {
            return NotFound($"Patient with ID '{patientId}' not found.");
        }

        return Ok(patientDetails);
    }
    [HttpGet("is-assigned/{patientId}/{doctorId}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> IsDoctorAssigned(string patientId, string doctorId)
    {
        // 1. Validate inputs (basic check)
        if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(doctorId))
        {
            return BadRequest("Both Patient ID and Doctor ID must be provided.");
        }
        bool isAssigned = await _doctorService.IsDoctorAssignedToPatient(patientId, doctorId);

        // 3. Return the result
        if (isAssigned)
        {
            // The doctor IS the personalized doctor for this patient.
            return Ok(new
            {
                Status = "Assigned",
                IsPersonalizedDoctor = true,
                Message = $"Doctor {doctorId} is the personalized doctor for Patient {patientId}."
            });
        }
        else
        {
            // The doctor is NOT the personalized doctor for this patient.
            return Ok(new
            {
                Status = "Not Assigned",
                IsPersonalizedDoctor = false,
                Message = $"Doctor {doctorId} is NOT the personalized doctor for Patient {patientId}."
            });
        }
    }
    [HttpPost("planid/{PlanId}")] // <-- The Correct Method
    public async Task<IActionResult> AssignPlan([FromRoute] int PlanId, [FromBody] AssignPlanRequest request)
    {
        // if (!ModelState.IsValid) { ... } // Validation logic remains

        // request.PlanId = PlanId; // Often you inject the route ID into the body model

        try
        {
            await _doctorService.AssignPlanAsync(request);

            // 201 Created is perfect for a successful non-idempotent creation (Source 1.7)
            return StatusCode(201);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred while assigning the plan.");
        }
    }
    [HttpGet("{diagnosisId}/details")]
    [ProducesResponseType(typeof(DiagnosisDetailDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetDiagnosisDetails(string diagnosisId)
    {
        try
        {
            var detailsDto = await _doctorService.GetDiagnosisDetailsAsync(diagnosisId);

            // Returns the 200 OK status with the DTO object
            return Ok(detailsDto);
        }
        catch (KeyNotFoundException)
        {
            // Returns a 404 Not Found status
            return NotFound($"Diagnosis with ID '{diagnosisId}' not found.");
        }
        catch (InvalidOperationException ex)
        {
            // Returns a 500 Internal Server Error for data issues
            return StatusCode(500, $"Data error: {ex.Message}");
        }
    }
}
//    private readonly IWellnessPlanService _wellnessPlanService;


//    public DoctorController(IWellnessPlanService wellnessPlanService)
//    {
//        _wellnessPlanService = wellnessPlanService;
//    }

//    [HttpPost("assign")]
//    public async Task<IActionResult> AssignPlan([FromBody] AssignPlanRequest request)
//    {
//        if (!ModelState.IsValid)
//            return BadRequest(ModelState);

//        try
//        {
//            await _wellnessPlanService.AssignPlanAsync(request);
//            return Ok(new { message = "Plan assigned successfully." });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { error = ex.Message });
//        }
//    }
//    [HttpGet("search")]
//    public async Task<IActionResult> SearchPatientPlans([FromQuery] string query)
//    {
//        if (string.IsNullOrWhiteSpace(query))
//            return BadRequest(new { message = "Search query cannot be empty." });

//        try
//        {
//            var result = await _wellnessPlanService.SearchPatientPlansAsync(query);
//            if (result == null || !result.Any())
//                return NotFound(new { message = "No matching patients or plans found." });

//            return Ok(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { error = ex.Message });
//        }
//    }

//    #region GET: Retrieving Patient Details
//    // GET: api/AssignedWellnessPlan/GetPatientDetails?patientId=...&patientName=...
//    [HttpGet("GetPatientDetails")]
//    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientWellnessDTO))]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    public async Task<ActionResult<PatientWellnessDTO>> GetPatientDetails(string patientId, string patientName)
//    {
//        var result = await _wellnessPlanService.GetPatientDetailsAsync(patientId, patientName);

//        if (result == null)
//        {
//            // Patient was not found by the service
//            return NotFound("Patient not found.");
//        }

//        // Return the full DTO containing all mapped data
//        return Ok(result);
//    }
//    #endregion
//}