using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs.PatientDashboard;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System; // Required for DateTime and Exception

namespace Patient_Monitoring.Services.Implementations
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientDashboardService(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // 1. MONOLITHIC ENDPOINT IMPLEMENTATION
        public async Task<PatientDashboardDto> GetDashboardData(int patientId)
        {
            var patient = await _context.Patients
                .SingleOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null) return null;

            var dashboardData = new PatientDashboardDto
            {
                PatientProfile = MapPatientProfile(patient),
                PersonalDoctor = await GetPersonalDoctorDetails(patient.PatientId),
                UpcomingAppointments = await GetUpcomingAppointments(patient.PatientId),
                ActivePrescriptions = await GetActivePrescriptions(patient.PatientId),
                RecentActivities = await GetRecentActivities(patient.PatientId),
               
            };

            return dashboardData;
        }

        // 6. PUBLIC ENDPOINT IMPLEMENTATION: Update Patient Profile (NEW)
        /// <summary>
        /// Updates the profile details for the given patient ID.
        /// </summary>
        /// <returns>True if the patient was found and updated, false otherwise.</returns>
        public async Task<bool> UpdatePatientProfile(int patientId, UpdatePatientProfileDto updateDto)
        {
            var patient = await _context.Patients
                .SingleOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null)
            {
                return false;
            }

            try
            {
                // Apply updates only if the DTO fields are not null or empty
               // if (!string.IsNullOrEmpty(updateDto.FirstName)) patient.FirstName = updateDto.FirstName;
               // if (!string.IsNullOrEmpty(updateDto.LastName)) patient.LastName = updateDto.LastName;
                if (!string.IsNullOrEmpty(updateDto.ContactNumber)) patient.ContactNumber = updateDto.ContactNumber;
                if (!string.IsNullOrEmpty(updateDto.Address)) patient.Address = updateDto.Address;
                if (!string.IsNullOrEmpty(updateDto.EmergencyContactName)) patient.EmergencyContactName = updateDto.EmergencyContactName;
                if (!string.IsNullOrEmpty(updateDto.EmergencyContactNumber)) patient.EmergencyContactNumber = updateDto.EmergencyContactNumber;
                if (!string.IsNullOrEmpty(updateDto.Email)) patient.Email = updateDto.Email;

                // Save changes to the database
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // In a real application, you would log this exception (e.g., using ILogger)
                Console.WriteLine($"Error updating patient profile for ID {patientId}: {ex.Message}");
                return false;
            }
        }

        // 2. PUBLIC ENDPOINT IMPLEMENTATION: Upcoming Appointments
        public async Task<List<AppointmentSummaryDto>> GetUpcomingAppointments(int patientId)
        {
            // Logic extracted from the original private helper method
            var upcoming = await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate > DateTime.Now && a.Status == "Confirmed")
                .OrderBy(a => a.AppointmentDate)
                .Take(2) // Limiting as per UI widget design
                .Select(a => new AppointmentSummaryDto
                {
                    AppointmentId = a.AppointmentId, // Assuming AppointmentId exists in model
                    Title = a.Reason, // Using Reason as the title for simplicity
                    // Note: If Doctor navigation property is not included, this might require explicit join or Include
                    DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                    AppointmentDateTime = a.AppointmentDate,
                    Status = a.Status, // confirmed/pending
                    Description = a.Reason // Using Reason for description
                })
                .ToListAsync();

            return upcoming;
        }

        // 3. PUBLIC ENDPOINT IMPLEMENTATION: Active Prescriptions
        public async Task<List<PrescriptionSummaryDto>> GetActivePrescriptions(int patientId)
        {
            // Logic extracted from the original private helper method
            var active = await _context.Prescriptions
                .Where(p => p.PatientId == patientId && p.EndDate >= DateTime.Today)
                .Take(2) // Limiting as per UI widget design
                .Select(p => new PrescriptionSummaryDto
                {
                    PrescriptionId = p.PrescriptionId, // Assuming PrescriptionId exists in model
                    MedicationName = p.MedicationName,
                    // Combining Dosage and Instructions as per DTO definition
                    DosageAndInstructions = $"{p.Dosage} - {p.Instructions}",
                    EndDate = p.EndDate,
                    Status = "Active" // Simplified status logic
                })
                .ToListAsync();

            return active;
        }

        // 4. PUBLIC ENDPOINT IMPLEMENTATION: Recent Activities
        public async Task<List<TaskLogSummaryDto>> GetRecentActivities(int patientId)
        {
            // Logic extracted from the original private helper method
            var logs = await _context.TaskLogs
                .Include(l => l.PatientPlan)
                .ThenInclude(pa => pa.AssignedWellnessPlan)
                //.Where(l => l.DueDate.Date == DateTime.Now.Date && l.PatientPlan.PatientId == patientId)
                .Where(l => l.DueDate.Date == DateTime.Now.AddDays(+1).Date && l.PatientPlan.PatientId == patientId)
                .OrderByDescending(l => l.DueDate)
                .Take(2) // Limiting as per UI widget design
                .Select(l => new TaskLogSummaryDto
                {
                    DueDate = l.DueDate,
                    TaskName = l.PatientPlan.AssignedWellnessPlan.PlanName,
                    Status = l.Status
                })
                .ToListAsync();

            return logs;
        }

        // 5. PUBLIC ENDPOINT IMPLEMENTATION: Wellness Progress

        // --- PRIVATE HELPER METHODS (Reused and/or Kept for Internal Mapping) ---

        private PatientProfileDto MapPatientProfile(Patient patient)
        {
            // Maps data from the 'Patients' model to PatientProfileDTO
            return new PatientProfileDto
            {
                FullName = patient.FirstName + " " + patient.LastName,
                PatientId = patient.PatientId,
                Email = patient.Email,
                ContactNumber = patient.ContactNumber,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactNumber = patient.EmergencyContactNumber,
                RegistrationDate = patient.RegistrationDate
            };
        }

        private async Task<DoctorDetailsDto> GetPersonalDoctorDetails(int patientId)
        {
            // Joins PatientDoctorMapper -> Doctors
            var mapping = await _context.PatientDoctorMapper
                .Include(pd => pd.Doctor)
                .FirstOrDefaultAsync(pd => pd.PatientId == patientId);

            if (mapping == null) return new DoctorDetailsDto();

            var doctor = mapping.Doctor;

            // Calculate TotalVisits and LastVisit from Appointments table
            var visits = await _context.Appointments
                .Where(a => a.PatientId == patientId && a.DoctorId == doctor.DoctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            // NOTE: The UI has a 'Latest Notes' field. This data must come from a 'Notes' or 'MedicalHistory' table.
           // string latestNote = "Patient shows excellent progress and adherence to the plan."; // Hardcoded placeholder

            return new DoctorDetailsDto
            {
                FullName = doctor.FirstName + " " + doctor.LastName,
                Specialization = doctor.Specialization,
                DoctorId = doctor.DoctorId,
                Qualification = doctor.Education, // Using 'Education' for MD/Fellowship
                PatientSince = mapping.AssignedDate,
                TotalVisits = visits.Count,
                LastVisit = visits.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
             //   Languages = "English, Spanish", // Hardcoded as per UI
              //  LatestNote = latestNote
            };
        }
    }
}