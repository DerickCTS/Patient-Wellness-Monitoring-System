using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Patient_Monitoring.Utils;

namespace Patient_Monitoring.Repository.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientMonitoringDbContext _context;
        private readonly ILogger<PatientRepository> _logger;
        private readonly PasswordHasher _passwordHasher;

        public PatientRepository(PatientMonitoringDbContext context, ILogger<PatientRepository> logger, PasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }


        #region Get Patient By Email
        public async Task<Patient_Detail?> GetByEmail(string email)
        {
            return await _context.Patient_Details.FirstOrDefaultAsync(p => p.Email == email);
        }

        #endregion


        #region Add New Patient to DB
        public async Task AddPatient(Patient_Detail patient)
        {
            await _context.Patient_Details.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        #endregion

        public async Task<Patient_Detail> GetPatientByIdAsync(string patientId)
        {
            return await _context.Patient_Details.FirstOrDefaultAsync(p => p.PatientID == patientId);
        }

        public async Task UpdatePatientAsync(Patient_Detail patient)
        {
            _context.Patient_Details.Update(patient);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Fetches all data required for the patient profile dashboard UI.
        /// </summary>
        public async Task<PatientDashboardDto> GetPatientDashboardDataAsync(string patientId)
        {
            try
            {
                // 1. Fetch Patient and Assigned Doctor
                var patient = await _context.Patient_Details
                    .Include(p => p.DoctorMappers)
                        .ThenInclude(pm => pm.Doctor)
                    .FirstOrDefaultAsync(p => p.patient_id == patientId);

                if (patient == null) return null;

                var doctorMapper = patient.DoctorMappers.FirstOrDefault(); // Get the primary doctor
                var assignedDoctor = doctorMapper?.Doctor;

                // 2. Fetch related data sections
                var upcomingAppointments = await _context.Appointments
                    .Where(a => a.PatientID == patientId && a.Appointment_Date_Time.Add(a.Appointment_Date_Time) >= DateTime.Now)
                    .OrderBy(a => a.appointment_date)
                    .Take(5)
                    .ToListAsync();

                var activePrescriptions = await _context.Prescriptions
                    .Where(p => p.patient_id == patientId && p.end_date >= DateTime.Today)
                    .ToListAsync();

                var recentActivities = await _context.DailyTaskLogs
                    .Where(a => a.patient_id == patientId)
                    .OrderByDescending(a => a.task_date)
                    .Take(3)
                    .ToListAsync();

                var totalVisits = await _context.Appointments
                    .CountAsync(a => a.PatientID == patientId && a.appointment_status == "completed");

                var lastVisit = await _context.Appointments
                    .Where(a => a.PatientID == patientId && a.appointment_status == "completed")
                    .OrderByDescending(a => a.appointment_date)
                    .Select(a => (DateTime?)a.appointment_date)
                    .FirstOrDefaultAsync();

                // 3. Mapping Logic
                var dashboardDto = new PatientDashboardDto
                {
                    PatientProfile = MapPatientProfile(patient),
                    AssignedDoctor = assignedDoctor != null ? MapDoctorDetails(assignedDoctor) : null,
                    CareRelationship = MapCareRelationship(patient, assignedDoctor, totalVisits, lastVisit),
                    UpcomingAppointments = upcomingAppointments.Select(a => MapAppointment(a, assignedDoctor)).ToList(),
                    ActivePrescriptions = activePrescriptions.Select(MapPrescription).ToList(),
                    RecentActivity = recentActivities.Select(MapActivityLog).ToList()
                };

                return dashboardDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching patient dashboard data for ID: {PatientId}", patientId);
                return null;
            }
        }

        /// <summary>
        /// Updates patient contact info based on the provided DTO.
        /// </summary>
        public async Task<bool> UpdatePatientContactInfoAsync(string patientId, UpdateContactInfoDto updateDto)
        {
            var patient = await _context.Patient_Details.FirstOrDefaultAsync(p => p.PatientID == patientId);
            if (patient == null) return false;

            patient.email = updateDto.Email;
            patient.contact_number = updateDto.ContactNumber;
            patient.address = updateDto.Address;
            // Note: The schema has 'emergency_contact' as a single VARCHAR. 
            // We assume it stores name and number, which we will update here:
            patient.emergency_contact = $"{updateDto.EmergencyContactName} - {updateDto.EmergencyContactNumber}";

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates the patient's password securely.
        /// </summary>
        public async Task<bool> UpdatePatientPasswordAsync(string patientId, ChangePasswordDto passwordDto)
        {
            var patient = await _context.Patient_Details.FirstOrDefaultAsync(p => p.PatientID == patientId);
            if (patient == null) return false;

            // 1. Verify current password
            // Assuming your Patient_Detail model has a property for the hashed password, e.g., 'password_hash'
            if (!_passwordHasher.VerifyPassword(passwordDto.CurrentPassword, patient.password_hash))
            {
                return false; // Current password verification failed
            }

            // 2. Hash and update new password
            patient.password_hash = _passwordHasher.HashPassword(passwordDto.NewPassword);

            await _context.SaveChangesAsync();
            return true;
        }

        // --- Private Mapping Helpers ---

        private PatientProfileDto MapPatientProfile(Patient_Detail p)
        {
            // Split emergency contact string back into name/number for display
            var parts = p.emergency_contact?.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            return new PatientProfileDto
            {
                PatientId = p.PatientID,
                Name = $"{p.FirstName} {p.LastName}",
                PatientIdentifier = p.PatientID,
                Email = p.Email,
                ContactNumber = p.ContactNumber,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                RegistrationDate = p.RegistrationDate,
                Address = p.Address,
                EmergencyContactName = parts.Length > 0 ? parts[0] : "N/A",
                EmergencyContactNumber = parts.Length > 1 ? parts[1] : "N/A"
            };
        }

        private DoctorDetailsDto MapDoctorDetails(Doctor_Detail d)
        {
            return new DoctorDetailsDto
            {
                DoctorId = d.DoctorID,
                Name = $"{d.FirstName} {d.LastName}",
                Specialty = d.Specialization,
                DoctorIdentifier = d.DoctorID,
                //Education = d.Education,
                ContactInformation = $"{d.Email} / {d.ContactNumber}",
                // Placeholders used for Rating/Experience/Count as they require additional tables/logic
            };
        }

        private CareRelationshipDto MapCareRelationship(Patient_Detail patient, Doctor_Detail doctor, int totalVisits, DateTime? lastVisit)
        {
            var duration = (DateTime.Today - patient.RegistrationDate);
            int months = (DateTime.Today.Year - patient.RegistrationDate.Year) * 12 + DateTime.Today.Month - patient.RegistrationDate.Month;
            string durationString = months >= 12 ? $"{months / 12} years, {months % 12} months" : $"{months} months";
            if (months < 1) durationString = $"{duration.Days} days";

            return new CareRelationshipDto
            {
                PatientSince = patient.RegistrationDate,
                Duration = durationString,
                TotalVisits = totalVisits,
                LastVisit = lastVisit,
                // Placeholders used for fee, notes, schedule, next available time
            };
        }

        private AppointmentDto MapAppointment(Appointment a, Doctor_Detail doctor)
        {
            return new AppointmentDto
            {
                Id = a.AppointmentID,
                Title = a.Reason, // Using reason as title for the UI card
                Status = a.AppointmentStatus,
                DoctorName = $"{doctor?.FirstName} {doctor?.LastName}",
                DateTime = a.Appointment_Date_Time,
                Notes = a.Reason
            };
        }

        private PrescriptionDto MapPrescription(Prescription p)
        {
            return new PrescriptionDto
            {
                Id = p.prescription_id,
                Name = p.medication_name,
                DosageAndInstructions = $"{p.dosage} - {p.instruction}",
                EndDate = p.end_date,
                Status = p.end_date >= DateTime.Today ? "Active" : "Completed"
            };
        }

        private ActivityLogDto MapActivityLog(DailyTaskLog a)
        {
            return new ActivityLogDto
            {
                Id = a.log_id,
                Status = a.status,
                Date = a.task_date,
                Description = a.completed_at_notes
            };
        }
    }
}

    }
}
