using Microsoft.AspNetCore.Identity;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Patient_Monitoring.Services.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private const int MAX_PATIENT_CAPACITY = 10;

        // Inject all necessary repositories. IPasswordHasher is removed as it's static.
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientDoctorMappingRepository _mappingRepository;

        public AdminDashboardService(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IPatientDoctorMappingRepository mappingRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _mappingRepository = mappingRepository;
        }

        // --- Dashboard Overview Logic ---
        public async Task<DashboardOverviewDTO> GetDashboardOverviewAsync()
        {
            var totalPatientsTask = _patientRepository.GetTotalPatientsCountAsync();
            var totalDoctorsTask = _doctorRepository.GetTotalDoctorsCountAsync();
            var appointmentsTask = _appointmentRepository.GetScheduledAppointmentsCountAsync();
            var unmappedPatientsTask = _mappingRepository.GetUnmappedPatientsCountAsync();
            var doctorAssignmentsTask = _mappingRepository.GetDoctorAssignmentSummariesAsync();

            await Task.WhenAll(
                totalPatientsTask,
                totalDoctorsTask,
                appointmentsTask,
                unmappedPatientsTask,
                doctorAssignmentsTask);

            return new DashboardOverviewDTO
            {
                TotalPatients = totalPatientsTask.Result,
                TotalDoctors = totalDoctorsTask.Result,
                TotalScheduledAppointments = appointmentsTask.Result,
                UnmappedPatients = unmappedPatientsTask.Result,
                DoctorAssignments = doctorAssignmentsTask.Result.ToList()
            };
        }

        // --- Patient Management Logic ---
        public async Task<IEnumerable<PatientManagementDTO>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllPatientsWithAssignedDoctorAsync();
        }

        public async Task<PatientManagementDTO> AddPatientAsync(PatientRegisterDTO newPatient)
        {
            var patient = new Patient
            {
                PatientID = "P-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper(),
                FirstName = newPatient.FirstName,
                LastName = newPatient.LastName,
                Email = newPatient.Email,
                Password = newPatient.Password,
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Gender = "Unknown",
                ContactNumber = "N/A",
                Address = "N/A",
                EmergencyContactName = "N/A",
                EmergencyContactNumber = "N/A",
            };

            await _patientRepository.AddPatient(patient);

            // You may need to fetch the patient again to get its ID/Code if needed
            var addedPatient = await _patientRepository.GetByEmail(patient.Email);

            return new PatientManagementDTO
            {
                PatientId = addedPatient != null && int.TryParse(addedPatient.PatientID.Split('-').Last(), out var pid) ? pid : 0,
                FirstName = addedPatient?.FirstName ?? "",
                LastName = addedPatient?.LastName ?? "",
                Email = addedPatient?.Email ?? "",
                AssignedDoctorName = "Unmapped"
            };
        }

        // --- Doctor Management Logic ---
        public async Task<IEnumerable<DoctorManagementDTO>> GetAllDoctorsWithAssignmentsAsync()
        {
            return await _doctorRepository.GetAllDoctorsWithAssignmentCountAsync(MAX_PATIENT_CAPACITY);
        }

        public async Task<DoctorManagementDTO> AddDoctorAsync(DoctorRegisterDTO newDoctor)
        {
            var doctor = new Doctor
            {
                // Required properties initialized to resolve CS0903/CS0117 errors
                DoctorId = "D-" + Guid.NewGuid().ToString().Substring(0, 3).ToUpper(),
                FirstName = newDoctor.FirstName,
                LastName = newDoctor.LastName,
                Email = newDoctor.Email,
                Specialization = newDoctor.Specialization,
                Password = newDoctor.Password,
                //PasswordHash = PasswordHasher.Hash(newDoctor.Password), // Static utility call

                // Placeholder/Required fields
                Education = "N/A",
                ContactNumber = "N/A",
            };

            await _doctorRepository.AddDoctorAsync(doctor);
            var addedDoctor = await _doctorRepository.GetByEmail(doctor.Email);

            return new DoctorManagementDTO
            {
                DoctorId = addedDoctor != null && int.TryParse(addedDoctor.DoctorId.Split('-').Last(), out var pid) ? pid : 0,
                // Replace this line:
                // DoctorIDCode = addedDoctor?.DoctorID ?? "",

                // With this line:
                FullName = $"{addedDoctor?.FirstName ?? ""} {addedDoctor?.LastName ?? ""}",
                Email = addedDoctor?.Email ?? "",
                Specialization = addedDoctor?.Specialization ?? "",
                PatientsAssignedCount = 0,
                MaxCapacity = MAX_PATIENT_CAPACITY
            };
        }

        // --- Mapping Logic ---
        public async Task<IEnumerable<PatientDoctorMappingDTO>> GetUnmappedPatientsAsync()
        {
            var patients = await _patientRepository.GetPatientsWithoutMappingAsync();

            // Fix: Cast or map each patient object to the correct type that has PatientId, etc.
            // Assuming the returned objects are of type Patient or a DTO with the required properties.
            return patients.Select(p =>
            {
                dynamic patient = p;
                return new PatientDoctorMappingDTO
                {
                    PatientId = patient.PatientId,
                    PatientName = $"{patient.FirstName} {patient.LastName}",
                    PatientEmail = patient.Email,
                    AssignedDoctorId = null,
                    AssignedDoctorName = ""
                };
            }).ToList();
        }

        public async Task<IEnumerable<DoctorManagementDTO>> GetAvailableDoctorsForMappingAsync()
        {
            var allDoctors = await GetAllDoctorsWithAssignmentsAsync();
            return allDoctors.Where(d => d.PatientsAssignedCount < MAX_PATIENT_CAPACITY);
        }

        public async Task<bool> AssignPatientToDoctorAsync(int patientId, int doctorId)
        {
            var patientCount = await _mappingRepository.GetPatientCountForDoctorAsync(doctorId);
            if (patientCount >= MAX_PATIENT_CAPACITY)
            {
                return false;
            }

            var existingMapping = await _mappingRepository.GetMappingByPatientIdAsync(patientId);
            if (existingMapping != null)
            {
                await _mappingRepository.DeleteMappingByPatientIdAsync(patientId);
            }

            var newMapping = new PatientDoctorMapper
            {
                PatientID = patientId.ToString(),
                DoctorID = doctorId.ToString(),
                AssignedDate = DateTime.UtcNow
            };

            var result = await _mappingRepository.CreateMappingAsync(newMapping);
            return result != null;
        }

        public async Task<bool> DeletePatientDoctorMappingAsync(int patientId)
        {
            return await _mappingRepository.DeleteMappingByPatientIdAsync(patientId);
        }

        // --- Appointments Logic ---
        public async Task<IEnumerable<AppointmentDTO>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsDetailsAsync();
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);

            if (appointment != null && appointment.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            {
                return await _appointmentRepository.DeleteAppointmentAsync(appointmentId);
            }

            return false;
        }
    }
}