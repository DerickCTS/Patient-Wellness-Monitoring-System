using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs.PatientDashboard;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interfaces;
using System.Collections.Generic; // Added for List<T>
using System.Linq; // Added for LINQ extensions

namespace Patient_Monitoring.Services.Implementations
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientDashboardService(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // 1. MONOLITHIC ENDPOINT IMPLEMENTATION (remains the same)
        public async Task<PatientDashboardDto> GetDashboardData(int patientId)
        {
            var patient = await _context.Patients
                .SingleOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null) return null;

            var dashboardData = new PatientDashboardDto
            {
                PatientProfile = MapPatientProfile(patient),
                PersonalDoctor = await GetPersonalDoctorDetails(patient.PatientId),
                // These now call the new public methods
                UpcomingAppointments = await GetUpcomingAppointments(patient.PatientId),
                ActivePrescriptions = await GetActivePrescriptions(patient.PatientId),
                RecentActivities = await GetRecentActivities(patient.PatientId),
                WellnessProgress = await GetWellnessProgress(patient.PatientId)
            };

            return dashboardData;
        }

        // 2. PUBLIC ENDPOINT IMPLEMENTATION: Upcoming Appointments
        public async Task<List<AppointmentSummaryDto>> GetUpcomingAppointments(int patientId)
        {
            // Logic extracted from the original private helper method
            var upcoming = await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate > DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .Take(2) // Limiting as per UI widget design
                .Select(a => new AppointmentSummaryDto
                {
                    AppointmentId = a.AppointmentId, // Assuming AppointmentId exists in model
                    Title = a.Reason, // Using Reason as the title for simplicity
                    DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName, // Requires .Include(a => a.Doctor) if not already included
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
                .Where(l => l.PatientPlan.PatientId == patientId)
                .Where(l => l.Status == "completed" && l.CompletedAt != null)
                .OrderByDescending(l => l.CompletedAt)
                .Take(2) // Limiting as per UI widget design
                .Select(l => new TaskLogSummaryDto
                {
                    TaskLogId = l.LogId, // Assuming TaskLogId exists in model
                    DateCompleted = l.CompletedAt.Value.Date,
                    TaskName = l.PatientPlan.AssignedWellnessPlan.PlanName ?? "General Activity",
                    ShortDescription = l.PatientNotes ?? "Completed assigned task." // Using Description if available
                })
                .ToListAsync();

            return logs;
        }

        // 5. PUBLIC ENDPOINT IMPLEMENTATION: Wellness Progress
        public async Task<WellnessProgressDto> GetWellnessProgress(int patientId)
        {
            // Logic extracted from the original private helper method
            var assignment = await _context.PatientPlanAssignments
                .Include(pa => pa.AssignedWellnessPlan)
                .Where(pa => pa.PatientId == patientId && pa.IsActive)
                .FirstOrDefaultAsync();

            if (assignment == null || assignment.AssignedWellnessPlan == null)
            {
                // Return an empty/default DTO if no active plan is found
                return new WellnessProgressDto();
            }

            // NOTE: Progress calculation (e.g., 75%) logic should be refined here.
            int progressPercentage = 75; // Hardcoded placeholder

            return new WellnessProgressDto
            {
                PlanName = assignment.AssignedWellnessPlan.PlanName,
                Goal = assignment.AssignedWellnessPlan.Goal,
                ProgressPercentage = progressPercentage,
                MotivationQuote = "Feeling more energetic with regular exercise" // Hardcoded placeholder
            };
        }

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
                EmergencyContactNumber = patient.EmergencyContactNumber
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
            string latestNote = "Patient shows excellent progress and adherence to the plan."; // Hardcoded placeholder

            return new DoctorDetailsDto
            {
                FullName = doctor.FirstName + " " + doctor.LastName,
                Specialization = doctor.Specialization,
                DoctorId = doctor.DoctorId,
                Qualification = doctor.Education, // Using 'Education' for MD/Fellowship
                PatientSince = mapping.AssignedDate,
                TotalVisits = visits.Count,
                LastVisit = visits.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
                Languages = "English, Spanish", // Hardcoded as per UI
                LatestNote = latestNote
            };
        }

        // NOTE: The original private methods for appointments, prescriptions, activities, and progress 
        // have been replaced by the public interface methods above.
    }
}