using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Doctor")] // You would protect this for doctors only
public class DiagnosisApiController : ControllerBase
{
    private readonly IDiagnosisService _diagnosisService;

    public DiagnosisApiController(IDiagnosisService diagnosisService)
    {
        _diagnosisService = diagnosisService;
    }

    private string GetCurrentDoctorId()
    {
        // In a real app, get this from the JWT token claims
        return "doctor_id_from_dummy_data";
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysAppointments()
    {
        var doctorId = GetCurrentDoctorId();
        var appointments = await _diagnosisService.GetTodaysAppointmentsAsync(doctorId);
        return Ok(appointments);
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetPatientDiagnosisDetails(string appointmentId)
    {
        var details = await _diagnosisService.GetPatientDiagnosisDetailsAsync(appointmentId);
        if (details == null) return NotFound();
        return Ok(details);
    }

    [HttpGet("diseases")]
    public async Task<IActionResult> GetAllDiseases()
    {
        var diseases = await _diagnosisService.GetAllDiseasesAsync();
        return Ok(diseases);
    }

    [HttpPost("appointment/{appointmentId}/save")]
    public async Task<IActionResult> SaveDiagnosis(string appointmentId, [FromBody] SaveDiagnosisDto data)
    {
        var doctorId = GetCurrentDoctorId();
        var success = await _diagnosisService.SaveDiagnosisAndPrescriptionsAsync(appointmentId, doctorId, data);
        if (!success)
        {
            return BadRequest("Failed to save diagnosis and prescriptions.");
        }
        return Ok(new { message = "Diagnosis and prescriptions saved successfully." });
    }
}