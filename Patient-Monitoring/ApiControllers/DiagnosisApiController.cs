using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Doctor")] // You would protect this for doctors only
public class DiagnosisApiController : ControllerBase
{
    private readonly IDiagnosisService _diagnosisService;
    private readonly int doctorId;
    public DiagnosisApiController(IDiagnosisService diagnosisService)
    {
        _diagnosisService = diagnosisService;
        doctorId = GetCurrentDoctorId();
    }

    private int GetCurrentDoctorId()
    {
        // In a real app, get this from the JWT token claims
        return 1;
    }

    #region Retrieve today's appointment details
    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysAppointments()
    {
        var appointments = await _diagnosisService.GetTodaysAppointmentsAsync(doctorId);
        return Ok(appointments);
    }
    #endregion


    #region Retrieve deatiled appointment data by appointmentId
    //[HttpGet("appointment/{appointmentId}")]
    //public async Task<IActionResult> GetPatientDiagnosisDetails(int appointmentId)
    //{
    //    var details = await _diagnosisService.GetPatientDiagnosisDetailsAsync(appointmentId);
    //    if (details == null) return NotFound();
    //    return Ok(details);
    //}
    #endregion
    

    #region Retrieve diseases for drop down
    [HttpGet("diseases")]
    public async Task<IActionResult> GetAllDiseases()
    {
        var diseases = await _diagnosisService.GetAllDiseasesAsync();
        return Ok(diseases);
    }
    #endregion


    #region Create New Diagnosis
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