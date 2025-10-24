using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Services.Interfaces;
using System.Net;

namespace Patient_Monitoring.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    #region Search For A Patient By Name or Id
    [HttpGet("patient/search")]
    public async Task<ActionResult<List<PatientSearchItemDto>>> SearchPatients([FromQuery] string? patientName, [FromQuery] int? patientId)
    {
        if (string.IsNullOrWhiteSpace(patientName) && patientId == null)
        {
            return Ok(new List<PatientSearchItemDto>());
        }

        var searchResults = await _doctorService.SearchPatientsAsync(patientName, patientId);

        return Ok(searchResults);
    }
    #endregion


    #region Get Patient Details
    [HttpGet("patients/{patientId}")]
    public async Task<ActionResult<PatientFullDetailDto>> GetPatientDetails(int patientId)
    {
        var patientDetails = await _doctorService.GetPatientDetailsAsync(patientId);

        if (patientDetails == null)
        {
            return NotFound($"Patient with ID '{patientId}' not found.");
        }

        return Ok(patientDetails);
    }
    #endregion


    #region Get Diagnosis and Disease Details
    [HttpGet("{diagnosisId}/details")]
    public async Task<IActionResult> GetDiagnosisDetails(int diagnosisId)
    {
        try
        {
            var detailsDto = await _doctorService.GetDiagnosisDetailsAsync(diagnosisId);
            return Ok(detailsDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Diagnosis with ID '{diagnosisId}' not found.");
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, $"Data error: {ex.Message}");
        }
    }
    #endregion
}
