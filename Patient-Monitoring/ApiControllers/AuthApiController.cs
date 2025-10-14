using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.DTOs.Authentication;
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
            (bool success, string message) = await _authService.RegisterPatient(registerDTO);

            if (!success)
            {
                return BadRequest(new { Message = message });
            }

            return Ok(new { Message = message });
        }
        #endregion

        #region POST: Login User
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            var (success, message, token, refreshToken) = await _authService.Login(loginDTO);

            if (!success)
            {
                return Unauthorized(new { message });
            }

            Response.Cookies.Append("AuthToken", token!, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });
            Response.Cookies.Append("RefreshToken", refreshToken!, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });

            return Ok(new { message = "Login successful" });
        }
        #endregion

        #region POST: Register New Doctor
        [HttpPost]
        [Route("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDTO registerDTO)
        {
            (bool success, string Message) = await _authService.RegisterDoctor(registerDTO);

            if (!success)
            {
                return BadRequest(new { message = Message });
            }

            return Ok(new { message = Message });
        }
        #endregion

    }
}
