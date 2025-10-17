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

        public async Task<List<TodaysAppointmentCardDto>> GetTodaysAppointmentsAsync(int doctorId)
        {
            var appointments = await _diagnosisRepository.GetTodaysAppointmentsForDoctorAsync(doctorId);

            // Map the database entities to the DTO required by the UI
            return appointments.Select(app => new TodaysAppointmentCardDto
            {
                AppointmentId = app.AppointmentId,
                PatientName = $"{app.Patient.FirstName} {app.Patient.LastName}",
                Gender = app.Patient.Gender,
                Age = (DateTime.Today.Year - app.Patient.DateOfBirth.Year - (app.Patient.DateOfBirth.DayOfYear > DateTime.Today.DayOfYear ? 1 : 0)),
                AppointmentTime = $"{app.AppointmentDate:hh:mm tt} - {app.AppointmentSlot.EndDateTime:hh:mm tt}",
                ContactNumber = app.Patient.ContactNumber,
                PatientId = app.Patient.PatientID,
                ChiefComplaint = app.Reason
            }).ToList();
        }

        public async Task<PatientDiagnosisDetailDto?> GetPatientDiagnosisDetailsAsync(int appointmentId)
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

        public async Task<bool> SaveDiagnosisAndPrescriptionsAsync(int appointmentId, int doctorId, SaveDiagnosisDto data)
        {
            var appointment = await _diagnosisRepository.GetAppointmentWithPatientDetailsAsync(appointmentId);
            if (appointment == null) return false;

            var newDiagnoses = data.Diagnoses.Select(d => new Diagnosis
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DiseaseId = d.DiseaseId,
                Description = d.Description
            }).ToList();

            var newPrescriptions = new List<Prescription>();
            foreach (var prescription in data.Prescriptions)
            {
                var newPrescription = new Prescription
                {
                    PatientId = appointment.PatientId,
                    PrescribingDoctorId = doctorId,
                    AppointmentId = appointmentId,
                    MedicationName = prescription.MedicationName,
                    Dosage = prescription.Dosage,
                    StartDate = prescription.StartDate,
                    EndDate = prescription.EndDate,
                    Instructions = prescription.Instructions
                };

                var schedules = prescription.Schedules.Select(schedule => new MedicationSchedule
                {
                    TimeOfDay = schedule.TimeOfDay,
                    Quantity = schedule.Quantity,
                    PrescriptionId = newPrescription.PrescriptionId
                }).ToList();

                // Assigning the created schedules back to the parent's navigation property.
                newPrescription.MedicationSchedules = schedules;

                newPrescriptions.Add(newPrescription);
            }

            await _diagnosisRepository.AddDiagnosesAsync(newDiagnoses);
            await _diagnosisRepository.AddPrescriptionsAsync(newPrescriptions);
            await _diagnosisRepository.SaveChangesAsync();

            return true;
        }
    }
    
}