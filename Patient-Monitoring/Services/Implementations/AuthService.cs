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
        private readonly PasswordHasher<Patient> _patientPasswordHasher;
        private readonly PasswordHasher<Doctor> _doctorPasswordHasher;

        public AuthService(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _patientPasswordHasher = new PasswordHasher<Patient>();
            _doctorPasswordHasher = new PasswordHasher<Doctor>();
        }

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
                DoctorID = "D" + Guid.NewGuid().ToString(),
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
        #endregion

        #region Login
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

                if (result == PasswordVerificationResult.Failed)
                {
                    return (false, "Verification failed! Please enter correct password.");
                }

                return (true, null);   
            }
            else { 
                var doctor = await _doctorRepository.GetByEmail(user.Email);

                if (doctor == null)
                {
                    return (false, "Doctor does not exists!");
                }

                var result = _doctorPasswordHasher.VerifyHashedPassword(doctor, doctor.Password, user.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return (false, "Verification failed! Please enter correct password.");
                }

                return (true, null);
            }
        }

        #endregion
    }
}
