using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Services.Implementations
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly IDiagnosisRepository _diagnosisRepository;

        public DiagnosisService(IDiagnosisRepository diagnosisRepository)
        {
            _diagnosisRepository = diagnosisRepository;
        }

        public async Task<List<TodaysAppointmentCardDto>> GetTodaysAppointmentsAsync(string doctorId)
        {
            var appointments = await _diagnosisRepository.GetTodaysAppointmentsForDoctorAsync(doctorId);

            // Map the database entities to the DTO required by the UI
            return appointments.Select(app => new TodaysAppointmentCardDto
            {
                AppointmentId = app.AppointmentId,
                PatientName = $"{app.Patient.FirstName} {app.Patient.LastName}",
                Gender = app.Patient.Gender,
                // Calculate age from Date of Birth
                Age = (DateTime.Today.Year - app.Patient.DateOfBirth.Year - (app.Patient.DateOfBirth.DayOfYear > DateTime.Today.DayOfYear ? 1 : 0)),
                AppointmentTime = $"{app.AppointmentDate:hh:mm tt} - {app.AppointmentSlot.EndDateTime:hh:mm tt}",
                ContactNumber = app.Patient.ContactNumber,
                PatientId = app.Patient.PatientID,
                ChiefComplaint = app.Reason
            }).ToList();
        }

        public async Task<PatientDiagnosisDetailDto?> GetPatientDiagnosisDetailsAsync(string appointmentId)
        {
            var appointment = await _diagnosisRepository.GetAppointmentWithPatientDetailsAsync(appointmentId);

            if (appointment == null)
            {
                return null; // Appointment not found
            }

            // Map the comprehensive appointment details to our DTO
            var detailsDto = new PatientDiagnosisDetailDto
            {
                PatientInfo = new PatientDetail
                {
                    Name = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                    PatientId = appointment.Patient.PatientID,
                    Age = (DateTime.Today.Year - appointment.Patient.DateOfBirth.Year - (appointment.Patient.DateOfBirth.DayOfYear > DateTime.Today.DayOfYear ? 1 : 0)),
                    DateOfBirth = appointment.Patient.DateOfBirth,
                    Gender = appointment.Patient.Gender,
                    ContactNumber = appointment.Patient.ContactNumber,
                    Email = appointment.Patient.Email,
                    Address = appointment.Patient.Address,
                    EmergencyContact = $"{appointment.Patient.EmergencyContactNumber} ({appointment.Patient.EmergencyContactName})",
                    ChiefComplaint = appointment.Reason
                },
                // You can expand these DTOs later to show more detail if needed
                ExistingDiagnoses = appointment.Diagnoses.Select(d => new ExistingDiagnosisDto
                {
                    DiseaseName = d.Disease.DiseaseName,
                    Description = d.Description
                }).ToList(),
                ExistingPrescriptions = appointment.Patient.Prescriptions.Select(p => new ExistingPrescriptionDto
                {
                    MedicationName = p.MedicationName,
                    Dosage = p.Dosage
                }).ToList()
            };

            return detailsDto;
        }

        public async Task<List<DiseaseDto>> GetAllDiseasesAsync()
        {
            var diseases = await _diagnosisRepository.GetAllDiseasesAsync();
            return diseases.Select(d => new DiseaseDto
            {
                DiseaseId = d.DiseaseId,
                DiseaseName = d.DiseaseName
            }).ToList();
        }

        public async Task<bool> SaveDiagnosisAndPrescriptionsAsync(string appointmentId, string doctorId, SaveDiagnosisDto data)
        {
            var appointment = await _diagnosisRepository.GetAppointmentWithPatientDetailsAsync(appointmentId);
            if (appointment == null) return false;

            // --- (The code for newDiagnoses remains the same) ---
            var newDiagnoses = data.Diagnoses.Select(d => new Diagnosis
            {
                DiagnosisId = Guid.NewGuid().ToString(),
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DiseaseId = d.DiseaseId,
                Description = d.Description
            }).ToList();

            // =======================================================
            // REVISED LOGIC FOR PRESCRIPTIONS
            // =======================================================
            var newPrescriptions = new List<Prescription>();
            foreach (var p_dto in data.Prescriptions)
            {
                // 1. Create the parent Prescription object first and generate its ID.
                var newPrescription = new Prescription
                {
                    PrescriptionId = Guid.NewGuid().ToString(),
                    PatientId = appointment.PatientId,
                    PrescribingDoctorId = doctorId,
                    AppointmentId = appointmentId,
                    MedicationName = p_dto.MedicationName,
                    Dosage = p_dto.Dosage,
                    StartDate = p_dto.StartDate,
                    EndDate = p_dto.EndDate,
                    Instructions = p_dto.Instructions
                };

                // 2. Now, create the child MedicationSchedule objects.
                var schedules = p_dto.Schedules.Select(s_dto => new MedicationSchedule
                {
                    ScheduleId = Guid.NewGuid().ToString(),
                    TimeOfDay = s_dto.TimeOfDay,
                    Quantity = s_dto.Quantity,
                    // 3. Link the child to the parent using the ID we just created.
                    PrescriptionId = newPrescription.PrescriptionId
                }).ToList();

                // 4. Assign the created children back to the parent's navigation property.
                newPrescription.MedicationSchedules = schedules;

                // 5. Add the fully constructed Prescription object (with its children) to the final list.
                newPrescriptions.Add(newPrescription);
            }
            // =======================================================

            // Use a transaction to ensure all data is saved, or none is.
            await using var transaction = await _diagnosisRepository.BeginTransactionAsync();
            try
            {
                if (newDiagnoses.Any()) await _diagnosisRepository.AddDiagnosesAsync(newDiagnoses);
                if (newPrescriptions.Any()) await _diagnosisRepository.AddPrescriptionsAsync(newPrescriptions);

                await _diagnosisRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
    
}