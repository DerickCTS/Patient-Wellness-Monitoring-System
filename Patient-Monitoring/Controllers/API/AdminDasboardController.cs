
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Services.Interfaces;


namespace Patient_Monitoring.Controllers.API
{
    // Ensure this controller is protected and only accessible by Admins
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // --- Dashboard Overview ---
        [HttpGet("overview")]
        public async Task<ActionResult<DashboardOverviewDTO>> GetDashboardOverview()
        {
            var overview = await _dashboardService.GetDashboardOverviewAsync();
            return Ok(overview);
        }

        // --- Patient Management ---
        [HttpGet("patients")]
        public async Task<ActionResult<IEnumerable<PatientManagementDTO>>> GetAllPatients()
        {
            var patients = await _dashboardService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpPost("patients")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<PatientManagementDTO>> AddPatient([FromBody] PatientRegisterDTO newPatient)
        {
            // Note: PatientRegisterDTO should be used here, but for simplicity, 
            // I'm using a placeholder DTO from your existing solution structure.
            var patient = await _dashboardService.AddPatientAsync(newPatient);
            if (patient == null) return BadRequest("Could not add patient.");
            return CreatedAtAction(nameof(GetAllPatients), new { id = patient.PatientId }, patient);
        }

        // --- Doctor Management ---
        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<DoctorManagementDTO>>> GetAllDoctors()
        {
            var doctors = await _dashboardService.GetAllDoctorsWithAssignmentsAsync();
            return Ok(doctors);
        }

        [HttpPost("doctors")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DoctorManagementDTO>> AddDoctor([FromBody] DoctorRegisterDTO newDoctor)
        {
            var doctor = await _dashboardService.AddDoctorAsync(newDoctor);
            if (doctor == null) return BadRequest("Could not add doctor.");
            return CreatedAtAction(nameof(GetAllDoctors), new { id = doctor.DoctorId }, doctor);
        }

        // --- Patient-Doctor Mapping ---
        [HttpGet("mapping/unmapped")]
        public async Task<ActionResult<IEnumerable<PatientDoctorMappingDTO>>> GetUnmappedPatients()
        {
            var patients = await _dashboardService.GetUnmappedPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("mapping/available-doctors")]
        public async Task<ActionResult<IEnumerable<DoctorManagementDTO>>> GetAvailableDoctors()
        {
            // Available doctors are those with < 10 patients
            var doctors = await _dashboardService.GetAvailableDoctorsForMappingAsync();
            return Ok(doctors);
        }

        [HttpPost("mapping/assign")]
        public async Task<IActionResult> AssignPatient([FromQuery] int patientId, [FromQuery] int doctorId)
        {
            var success = await _dashboardService.AssignPatientToDoctorAsync(patientId, doctorId);
            if (!success) return BadRequest("Assignment failed. Check patient/doctor existence or capacity.");
            return Ok(new { Message = "Patient assigned successfully." });
        }

        [HttpDelete("mapping/delete")]
        public async Task<IActionResult> DeleteMapping([FromQuery] int patientId)
        {
            var success = await _dashboardService.DeletePatientDoctorMappingAsync(patientId);
            if (!success) return NotFound("Mapping not found or deletion failed.");
            return NoContent();
        }

        // --- Appointments ---
        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointments()
        {
            var appointments = await _dashboardService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpDelete("appointments/{appointmentId}")]
        public async Task<IActionResult> DeleteCompletedAppointment(int appointmentId)
        {
            var success = await _dashboardService.DeleteAppointmentAsync(appointmentId);
            if (!success) return NotFound("Appointment not found or is not completed/deletable.");
            return NoContent();
        }
    }
}
