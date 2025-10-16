using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Implementations;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Interfaces;
//using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;

namespace Patient_Monitoring.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly PasswordHasher<Patient> _patientPasswordHasher;
        private readonly PasswordHasher<Doctor> _doctorPasswordHasher;
        private readonly  IAdminRepository _adminRepository;
        private readonly  IJwtService _jwtService;

        // Change the property to nullable to resolve CS8618
        //public object? AspNetCorePasswordVerificationResult { get; private set; }

        //public AuthService(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        //{
        //    _patientRepository = patientRepository;
        //    _doctorRepository = doctorRepository;
        //    _patientPasswordHasher = new PasswordHasher<Patient>();
        //    _doctorPasswordHasher = new PasswordHasher<Doctor>();
        //}

        #region Register New Patient
        public async Task<bool> RegisterPatient(PatientRegisterDTO patient)
        {
            var patientDetail = await _patientRepository.GetByEmail(patient.Email);

            if (patientDetail != null)
            {
                return false;
            }

            Patient newPatient = new Patient
            {
                PatientID = "P" + Guid.NewGuid().ToString(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactNumber = patient.EmergencyContactNumber,
                RegistrationDate = DateTime.UtcNow,
                Password = patient.Password
            };

            newPatient.Password = _patientPasswordHasher.HashPassword(newPatient, patient.Password);

            await _patientRepository.AddPatient(newPatient);

            return true;
        }
        #endregion

        #region Register New Doctor
        public async Task<bool> RegisterDoctor(DoctorRegisterDTO doctor)
        {
            var doctorDetails = await _doctorRepository.GetByEmail(doctor.Email);

            if (doctorDetails != null)
            {
                return false;
            }

            Doctor newDoctor = new Doctor
            {
                DoctorId = "D" + Guid.NewGuid().ToString(),
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                ContactNumber = doctor.ContactNumber,
                Email = doctor.Email,
                Password = doctor.Password,
                Education = doctor.Education,
            };

            newDoctor.Password = _doctorPasswordHasher.HashPassword(newDoctor, doctor.Password);

            await _doctorRepository.AddDoctor(newDoctor);

            return true;
        }
        // Replace all usages of PasswordVerificationResult.Failed comparison with integer comparison
        // because there are two PasswordVerificationResult enums from different namespaces in scope.
        // Use explicit cast to int for both sides to avoid ambiguity.
        public async Task<(bool success, string? message)> Login(UserLoginDTO user)
        {
            if (user.Role == "Patient")
            {
                var patient = await _patientRepository.GetByEmail(user.Email);
                if (patient == null)
                {
                    return (false, "Patient does not exists!");
                }

                var result = _patientPasswordHasher.VerifyHashedPassword(patient, patient.Password, user.Password);

                // Fix: Compare as integers to avoid CS0019
                if ((int)result == 0) // 0 is Failed
                {
                    return (false, "Verification failed! Please enter correct password.");
                }

                return (true, null);
            }
            else
            {
                var doctor = await _doctorRepository.GetByEmail(user.Email);

                if (doctor == null)
                {
                    return (false, "Doctor does not exists!");
                }

                var result = _doctorPasswordHasher.VerifyHashedPassword(doctor, doctor.Password, user.Password);

                // Fix: Compare as integers to avoid CS0019
                if ((int)result == 0) // 0 is Failed
                {
                    return (false, "Verification failed! Please enter correct password.");
                }

                return (true, null);
            }
        }

        public async Task<AdminResponseDTO> AdminLogin(AdminLoginDTO loginDto)
        {
            // 1. Find the admin by email
            var admin = await _adminRepository.GetAdminByEmailAsync(loginDto.Email);

            if (admin == null)
            {
                throw new InvalidOperationException("Admin not found.");
            }

            // 2. Verify the password
            var passwordHasher = new PasswordHasher();
            var result = passwordHasher.VerifyHashedPassword(admin.PasswordHash, loginDto.Password);

            if ((int)result == 0) // 0 is Failed
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // 3. Generate JWT token with Admin role claim
            var token = _jwtService.GenerateToken(admin.AdminId.ToString(), admin.Email, "Admin");

            // 4. Return response DTO
            return new AdminResponseDTO
            {
                AdminId = admin.AdminId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Token = token
            };
        }

        // Update the constructor to accept IJwtService
        public AuthService(
                IPatientRepository patientRepository,
                IDoctorRepository doctorRepository,
                IAdminRepository adminRepository,
                IJwtService jwtService)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _adminRepository = adminRepository;
            _jwtService = jwtService;
            _patientPasswordHasher = new PasswordHasher<Patient>();
            _doctorPasswordHasher = new PasswordHasher<Doctor>();
        }
    }
}
#endregion
