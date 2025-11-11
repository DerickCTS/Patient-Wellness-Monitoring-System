using Azure;
using Microsoft.AspNetCore.Identity;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.DTOs.Authentication;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly PasswordHasher<Patient> _patientPasswordHasher;
        private readonly PasswordHasher<Doctor> _doctorPasswordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IJwtService jwtService)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _patientPasswordHasher = new PasswordHasher<Patient>();
            _doctorPasswordHasher = new PasswordHasher<Doctor>();
            _jwtService = jwtService;
        }

        #region Register New Patient
        public async Task<(bool success, string message)> RegisterPatient(PatientRegisterDTO patient)
        {
            var patientDetail = await _patientRepository.GetPatientByEmailAsync(patient.Email);

            if (patientDetail != null)
            {
                return (false, "Email already registered as patient");
            }

            if (patient.ConfirmPassword != patient.Password)
            {
                return (false, "Confirm Password does not match the inputted password");
            }

            Patient newPatient = new Patient
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactNumber = patient.EmergencyContactNumber,    
                BloodGroup = patient.BloodGroup,
                RegistrationDate = DateTime.UtcNow,
                Password = patient.Password
            };

            newPatient.Password = _patientPasswordHasher.HashPassword(newPatient, patient.Password);

            bool result = await _patientRepository.AddNewPatientAsync(newPatient);

            if (result)
            {
                return (true, "Registration successful");
            }
            else
            {
                return (false, "Registration failed due to an internal error");
            }
            
        }
        #endregion


        #region Register New Doctor
        public async Task<(bool success, string message)> RegisterDoctor(DoctorRegisterDTO doctor)
        {
            var doctorDetails = await _doctorRepository.GetDoctorByEmailAsync(doctor.Email);

            if (doctorDetails != null)
            {
                return (false, "Email already registered as doctor");
            }

            if (doctor.Password != doctor.ConfirmPassword)
            {
                return (false, "Confirm Password does not match the inputted password");
            }

            Doctor newDoctor = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                ContactNumber = doctor.ContactNumber,
                Education = doctor.Education,
                Email = doctor.Email,
                Password = doctor.Password
            };

            newDoctor.Password = _doctorPasswordHasher.HashPassword(newDoctor, doctor.Password);

            bool result = await _doctorRepository.AddNewDoctorAsync(newDoctor);

            if (!result)
            {
                return (false, "Registration failed due to an internal error");
            }

            return (true, "Registration successful");
        }
        #endregion


        #region Login
        public async Task<(bool success, string? token, string? refreshToken, AuthResponseDto response)> Login(UserLoginDTO user)
        {
            if (user.Role == "Patient")
            {
                var patient = await _patientRepository.GetPatientByEmailAsync(user.Email);
                var response = new AuthResponseDto();
                if (patient == null)
                {
                    response.message = "Patient does not exists!";
                    return (false, null, null, response);
                }

                var result = _patientPasswordHasher.VerifyHashedPassword(patient, patient.Password, user.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    response.message = "Verification failed! Please enter correct password.";
                    return (false, null, null, response);
                }

                var token = _jwtService.GenerateAccessToken(patient, "Patient", out string jwtId);

                var refreshToken = await _jwtService.GenerateRefreshToken(jwtId, patient.PatientId, Enums.UserType.Patient);

                response.message = "Login successful";
                response.firstName = patient.FirstName;
                response.lastName = patient.LastName;
                response.userId = patient.PatientId;
                response.imageUrl = patient.ProfileImage;
                
                return (true, token, refreshToken, response);
            }
            else
            {
                var doctor = await _doctorRepository.GetDoctorByEmailAsync(user.Email);
                var response = new AuthResponseDto();

                if (doctor == null)
                {
                    response.message = "Doctor does not exists!";
                    return (false, null, null, response);
                    
                }

                var result = _doctorPasswordHasher.VerifyHashedPassword(doctor, doctor.Password, user.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    response.message = "Verification failed! Please enter correct password.";
                    return (false, null, null, response);
                }

                var token = _jwtService.GenerateAccessToken(doctor, "Doctor", out string jwtId);

                var refreshToken = await _jwtService.GenerateRefreshToken(jwtId, doctor.DoctorId, Enums.UserType.Doctor);

                response.message = "Login successful";
                response.firstName = doctor.FirstName;
                response.lastName = doctor.LastName;
                response.userId = doctor.DoctorId;
                response.specialization = doctor.Specialization;
                response.imageUrl = doctor.ProfileImage;

                return (true, token, refreshToken, response);
               
            }
        }

        #endregion
    }
}