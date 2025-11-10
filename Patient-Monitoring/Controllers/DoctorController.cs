using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Services.Interfaces;
using System.Net;

namespace Patient_Monitoring.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "Doctor")]
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


    #region DoctorDashboard

    [HttpGet("dashboard/profile")]

    public async Task<IActionResult> GetDoctorProfile()
    {
        // FIX: The compiler error 'The name 'CurrentDoctorId' does not exist in the current context' is resolved by the constant above.
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var profile = await _doctorService.GetDoctorProfileAsync(CurrentDoctorId);
        if (profile == null) return NotFound("Doctor profile not found.");
        return Ok(profile);
    }

    // --- 2. Dashboard Overview Endpoints ---

    [HttpGet("dashboard/stats")]
    public async Task<IActionResult> GetDashboardStats()
    {

        int doctorId = 2;
        var dashboardData = await _doctorService.GetDashboardStatsAsync(doctorId);

        return Ok(dashboardData);
    }

    [HttpGet("dashboard/weekly-appointments")]
    public async Task<IActionResult> GetWeeklyAppointments()
    {
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var weeklyData = await _doctorService.GetWeeklyAppointmentDataAsync(CurrentDoctorId);
        return Ok(weeklyData);
    }

    [HttpGet("dashboard/recent-prescriptions")]
    public async Task<IActionResult> GetRecentPrescriptions()
    {
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var prescriptions = await _doctorService.GetRecentPrescriptionsAsync(CurrentDoctorId);
        return Ok(prescriptions);
    }

    // --- 3. Detailed Pop-up Breakdown Endpoints ---

    [HttpGet("dashboard/patients/breakdown")]
    public async Task<IActionResult> GetPatientsBreakdown()
    {
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var breakdown = await _doctorService.GetPatientsBreakdownAsync(CurrentDoctorId);
        return Ok(breakdown);
    }

    [HttpGet("dashboard/prescriptions/breakdown")]
    public async Task<IActionResult> GetPrescriptionsBreakdown()
    {
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var breakdown = await _doctorService.GetPrescriptionsBreakdownAsync(CurrentDoctorId);
        return Ok(breakdown);
    }

    [HttpGet("dashboard/wellness-plans/breakdown")]
    public async Task<IActionResult> GetWellnessPlansBreakdown()
    {
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var breakdown = await _doctorService.GetWellnessPlansBreakdownAsync(CurrentDoctorId);
        return Ok(breakdown);
    }
    [HttpGet("dashboard/appointments")]
    public async Task<ActionResult<Tuple<int, int>>> GetAppointmentsCompletedToday()
    {
        // The service method signature is: Task<Tuple<int, int>> GetAppointmentsCompletedTodayAsync(int doctorId, DateTime date)
        var CurrentDoctorId = 2; // Replace with actual logic to get current doctor's ID
        var result = await _doctorService.GetAppointmentsCompletedTodayAsync(CurrentDoctorId, DateTime.Today);
        return Ok(result);
    }

    #endregion
}
