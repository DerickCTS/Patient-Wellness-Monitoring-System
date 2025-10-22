using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Doctor")]
public class DiagnosisController : ControllerBase
{
    //😍
    private readonly IDiagnosisService _diagnosisService;
    private readonly int doctorId;
    public DiagnosisController(IDiagnosisService diagnosisService)
    {
        _diagnosisService = diagnosisService;
        doctorId = GetCurrentDoctorId();
    }


    #region Initialize - Get Current Doctor Id from JWT Token
    private int GetCurrentDoctorId()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found in token.");
        }

        if (int.TryParse(userIdClaim.Value, out int doctorId))
        {
            return doctorId;
        } 
        else
        {
            throw new UnauthorizedAccessException("Invalid User ID claim value.");
        }

        // Use this to hard code a doctor id for testing purposes. But comment the above codes.
        //return 1;
    }
    #endregion


    #region Retrieve today's appointment details
    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysAppointments()
    {
        var appointments = await _diagnosisService.GetTodaysAppointmentsAsync(doctorId);
        return Ok(appointments);
    }
    #endregion


    #region Retrieve detailed appointment data by appointmentId
    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetPatientDiagnosisDetails(int appointmentId)
    {
        var details = await _diagnosisService.GetPatientDiagnosisDetailsAsync(appointmentId);
        if (details == null) return NotFound();
        return Ok(details);
    }
    #endregion


    #region Retrieve diseases for drop down
    [HttpGet("diseases")]
    public async Task<IActionResult> GetAllDiseases()
    {
        var diseases = await _diagnosisService.GetAllDiseasesAsync();
        return Ok(diseases);
    }
    #endregion


    #region Create New Diagnosis and Prescriptions
    [HttpPost("appointment/{appointmentId}/save")]
    public async Task<IActionResult> SaveDiagnosis(int appointmentId, [FromBody] SaveDiagnosisDto data)
    {
        var success = await _diagnosisService.SaveDiagnosisAndPrescriptionsAsync(appointmentId, doctorId, data);
        if (!success)
        {
            return BadRequest("Failed to save diagnosis and prescriptions.");
        }
        return Ok(new { message = "Diagnosis and prescriptions saved successfully." });
    }
    #endregion
}