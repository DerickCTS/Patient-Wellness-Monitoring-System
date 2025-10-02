using Azure;
using Microsoft.AspNetCore.Identity;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly PasswordHasher<Patient_Detail> _patientPasswordHasher;
        private readonly PasswordHasher<Doctor_Detail> _doctorPasswordHasher;
        private readonly IJWTService2 _jwtService;

        public AuthService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IJWTService2 jwtService)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _patientPasswordHasher = new PasswordHasher<Patient_Detail>();
            _doctorPasswordHasher = new PasswordHasher<Doctor_Detail>();
            _jwtService = jwtService;
        }

        #region Register New Patient
        public async Task<(bool success, string message)> RegisterPatient(PatientRegisterDTO patient)
        {
            var patientDetail = await _patientRepository.GetByEmail(patient.Email);

            if (patientDetail != null)
            {
                return (false, "Email already registered as patient");
            }

            if (patient.ConfirmPassword != patient.Password)
            {
                return (false, "Confirm Password does not match the inputted password");
            }

            Patient_Detail newPatient = new Patient_Detail
            {
                PatientID = "P" + Guid.NewGuid().ToString(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContact = patient.EmergencyContact,
                RegistrationDate = DateTime.UtcNow,
                Password = patient.Password
            };

            newPatient.Password = _patientPasswordHasher.HashPassword(newPatient, patient.Password);

            await _patientRepository.AddPatient(newPatient);

            return (true, "Registration successful");
        }
        #endregion

        #region Register New Doctor
        public async Task<(bool success, string message)> RegisterDoctor(DoctorRegisterDTO doctor)
        {
            var doctorDetails = await _doctorRepository.GetByEmail(doctor.Email);

            if (doctorDetails != null)
            {
                return (false, "Email already registered as doctor");
            }

            if (doctor.Password != doctor.ConfirmPassword)
            {
                return (false, "Confirm Password does not match the inputted password");
            }

            Doctor_Detail newDoctor = new Doctor_Detail
            {
                DoctorID = "D" + Guid.NewGuid().ToString(),
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
                ContactNumber = doctor.ContactNumber,
                Email = doctor.Email,
                Password = doctor.Password
            };

            newDoctor.Password = _doctorPasswordHasher.HashPassword(newDoctor, doctor.Password);

            await _doctorRepository.AddDoctor(newDoctor);

            return (true, "Registration successful");
        }
        #endregion

        #region Login
        public async Task<(bool success, string? message, string? token, string? refreshToken)> Login(UserLoginDTO user)
        {
            if (user.Role == "Patient")
            {
                var patient = await _patientRepository.GetByEmail(user.Email);
                if (patient == null)
                {
                    return (false, "Patient does not exists!", null, null);
                }

                var result = _patientPasswordHasher.VerifyHashedPassword(patient, patient.Password, user.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return (false, "Verification failed! Please enter correct password.", null, null);
                }

                var token = _jwtService.GenerateAccessToken(patient, "Patient", out string jwtId);

                var refreshToken = await _jwtService.GenerateRefreshToken(jwtId, patient.PatientID, Enums.UserType.Patient);

                return (true, null, token, refreshToken);   
            }
            else { 
                var doctor = await _doctorRepository.GetByEmail(user.Email);

                if (doctor == null)
                {
                    return (false, "Doctor does not exists!", null, null);
                }

                var result = _doctorPasswordHasher.VerifyHashedPassword(doctor, doctor.Password, user.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return (false, "Verification failed! Please enter correct password.", null, null);
                }

                var token = _jwtService.GenerateAccessToken(doctor, "Doctor", out string jwtId);

                var refreshToken = await _jwtService.GenerateRefreshToken(jwtId, doctor.DoctorID, Enums.UserType.Doctor);

                return (true, null, token, refreshToken);
            }
        }

        #endregion
    }
}
