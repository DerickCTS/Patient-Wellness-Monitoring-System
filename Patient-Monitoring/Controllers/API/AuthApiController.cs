using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
        }

        #region POST: Register New Patient
        [HttpPost]
        [Route("RegisterPatient")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterDTO registerDTO)
        {
            bool success = await _authService.RegisterPatient(registerDTO);

            if (!success)
            {
                return BadRequest(new { message = "Email already registered as patient" });
            }

            return Ok(new { message = "Registration successful" });
        }
        #endregion

        #region POST: Login User
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            var (success, message) = await _authService.Login(loginDTO);
            if (!success)
            {
                return Unauthorized(new { message });
            }
            return Ok(new { message = "Login successful" });
        }
        #endregion

        #region POST: Register New Doctor
        [HttpPost]
        [Route("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDTO registerDTO)
        {
            bool success = await _authService.RegisterDoctor(registerDTO);

            if (!success)
            {
                return BadRequest(new { message = "Email already registered as doctor" });
            }

            return Ok(new { message = "Registration successful" });
        }
        #endregion

    }
}
