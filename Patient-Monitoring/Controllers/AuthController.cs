using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services;
using Patient_Monitoring.Utils;
using Patient_Monitoring.Data;

namespace Patient_Monitoring.Controllers
{
    public class AuthController : Controller
    {
        private readonly PatientMonitoringDbContext _db;
        private readonly IJwtService _jwt;
        public AuthController(PatientMonitoringDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }
        // GET: /Auth/RegisterPatient
        [HttpGet]
        public IActionResult RegisterPatient() => View();
        // POST: /Auth/RegisterPatient
        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromForm] Patient_Details model, [FromForm] string password)
        {
            if (!ModelState.IsValid) return View(model);
            if (await _db.Patient_Details.AnyAsync(p => p.Email == model.Email))
            {
                ModelState.AddModelError("email", "Email already registered as patient");
                return View(model);
            }
            model.Password = PasswordHasher.Hash(password); // adjust field name to your model's password property
            _db.Patient_Details.Add(model);
            await _db.SaveChangesAsync();
            var token = _jwt.GenerateToken(model.PatientID.ToString(), model.Email, "Patient");
            Response.Cookies.Append("AuthToken", token, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });
            return RedirectToAction("Dashboard", "Patient");
        }
        // GET: /Auth/RegisterDoctor
        [HttpGet]
        public IActionResult RegisterDoctor() => View();
        // POST: /Auth/RegisterDoctor
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor([FromForm] Doctor_Details model, [FromForm] string password)
        {
            if (!ModelState.IsValid) return View(model);
            if (await _db.Doctor_Details.AnyAsync(d => d.Email == model.Email))
            {
                ModelState.AddModelError("email", "Email already registered as doctor");
                return View(model);
            }
            model.Password = PasswordHasher.Hash(password);
            _db.Doctor_Details.Add(model);
            await _db.SaveChangesAsync();
            var token = _jwt.GenerateToken(model.DoctorID.ToString(), model.Email, "Doctor");
            Response.Cookies.Append("AuthToken", token, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });
            return RedirectToAction("Dashboard", "Doctor");
        }
        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login() => View();
        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Provide email and password");
                return View();
            }
            // check patient table
            var patient = await _db.Patient_Details.SingleOrDefaultAsync(p => p.Email == email);
            if (patient != null && PasswordHasher.Verify(password, patient.Password))
            {
                var token = _jwt.GenerateToken(patient.PatientID.ToString(), patient.Email, "Patient");
                Response.Cookies.Append("AuthToken", token, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });
                return RedirectToAction("Dashboard", "Patient");
            }
            // check doctor table
            var doctor = await _db.Doctor_Details.SingleOrDefaultAsync(d => d.Email == email);
            if (doctor != null && PasswordHasher.Verify(password, doctor.Password))
            {
                var token = _jwt.GenerateToken(doctor.DoctorID.ToString(), doctor.Email, "Doctor");
                Response.Cookies.Append("AuthToken", token, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true, Secure = false });
                return RedirectToAction("Dashboard", "Doctor");
            }
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
}
