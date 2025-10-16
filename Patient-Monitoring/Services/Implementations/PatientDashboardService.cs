using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interfaces;

namespace Patient_Monitoring.Services.Implementations
{
    public class PatientDashboardService : IPatientDashboardService
    {
        private readonly PatientMonitoringDbContext _context;
        private readonly ILogger<PatientDashboardService> _logger;

        public PatientDashboardService(
            PatientMonitoringDbContext context,
            ILogger<PatientDashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PatientDashboardDTO> GetPatientDashboardDataAsync(string patientId)
        {
            try
            {
                var patientInfo = await GetPatientInfoAsync(patientId);
                if (patientInfo == null)
                {
                    _logger.LogWarning($"Patient not found: {patientId}");
                    return null;
                }

                var dashboardData = new PatientDashboardDTO
                {
                    PatientInfo = patientInfo,
                    AssignedDoctor = await GetAssignedDoctorAsync(patientId),
                    ActivePrescriptions = await GetActivePrescriptionsAsync(patientId),
                    RecentTaskLogs = await GetRecentTaskLogsAsync(patientId, 10)
                };

                return dashboardData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting dashboard data for patient: {patientId}");
                throw;
            }
        }

        public async Task<PatientInfoDTO> GetPatientInfoAsync(string patientId)
        {
            try
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientID == patientId);

                if (patient == null)
                    return null;

                return new PatientInfoDTO
                {
                    PatientId = patient.PatientID,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName ?? string.Empty,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    ContactNumber = patient.ContactNumber,
                    Email = patient.Email,
                    Address = patient.Address,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactNumber = patient.EmergencyContactNumber,
                    RegistrationDate = patient.RegistrationDate,
                    ProfileImage = patient.ProfileImage ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting patient info: {patientId}");
                throw;
            }
        }

        public async Task<PatientInfoDTO> UpdatePatientInfoAsync(string patientId, UpdatePatientInfoDTO updateDTO)
        {
            try
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientID == patientId);

                if (patient == null)
                {
                    _logger.LogWarning($"Patient not found for update: {patientId}");
                    return null;
                }

                // Update patient information
                patient.FirstName = updateDTO.FirstName;
                patient.LastName = updateDTO.LastName;
                patient.Email = updateDTO.Email;
                patient.ContactNumber = updateDTO.ContactNumber;
                patient.DateOfBirth = updateDTO.DateOfBirth;
                patient.Gender = updateDTO.Gender;
                patient.Address = updateDTO.Address;
                patient.EmergencyContactName = updateDTO.EmergencyContactName;
                patient.EmergencyContactNumber = updateDTO.EmergencyContactNumber;

                if (!string.IsNullOrEmpty(updateDTO.ProfileImage))
                {
                    patient.ProfileImage = updateDTO.ProfileImage;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Patient info updated successfully: {patientId}");

                return await GetPatientInfoAsync(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating patient info: {patientId}");
                throw;
            }
        }

        public async Task<string> UploadProfileImageAsync(string patientId, string base64Image)
        {
            try
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientID == patientId);

                if (patient == null)
                {
                    _logger.LogWarning($"Patient not found for image upload: {patientId}");
                    return null;
                }

                patient.ProfileImage = base64Image;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Profile image uploaded successfully: {patientId}");

                return base64Image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading profile image: {patientId}");
                throw;
            }
        }

        public async Task<AssignedDoctorDTO> GetAssignedDoctorAsync(string patientId)
        {
            try
            {
                var doctorMapping = await _context.PatientDoctorMapper
                    .Include(pdm => pdm.Doctor)
                    .Where(pdm => pdm.PatientID == patientId)
                    .OrderByDescending(pdm => pdm.AssignedDate)
                    .FirstOrDefaultAsync();

                if (doctorMapping == null)
                {
                    _logger.LogWarning($"No assigned doctor found for patient: {patientId}");
                    return null;
                }

                var doctor = doctorMapping.Doctor;

                // Get total number of patients for this doctor
                var totalPatients = await _context.PatientDoctorMapper
                    .CountAsync(pdm => pdm.DoctorID == doctor.DoctorId);

                // Get total visits count
                var totalVisits = await _context.Appointments
                    .CountAsync(a => a.PatientId == patientId &&
                                   a.DoctorId == doctor.DoctorId &&
                                   a.Status == "completed");

                // Get last visit date
                var lastVisit = await _context.Appointments
                    .Where(a => a.PatientId == patientId &&
                              a.DoctorId == doctor.DoctorId &&
                              a.Status == "completed")
                    .OrderByDescending(a => a.AppointmentDate)
                    .Select(a => a.AppointmentDate)
                    .FirstOrDefaultAsync();

                // Get next available slot
                //var nextAvailable = await GetNextAvailableSlot(doctor.DoctorID);

                // Calculate duration since assignment
                var duration = CalculateDuration(doctorMapping.AssignedDate);

               

                return new AssignedDoctorDTO
                {
                    DoctorId = doctor.DoctorId,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Specialization = doctor.Specialization,
                    ContactNumber = doctor.ContactNumber,
                    Email = doctor.Email,
                    Image = doctor.ProfileImage ?? "https://images.unsplash.com/photo-1559839734-2b71ea197ec2?w=400&h=400&fit=crop&crop=face",
                    
                    Schedule = "Monday - Friday: 9:00 AM - 5:00 PM",
                    Rating = 4.8M,
                    Experience = DateTime.Now.Year - doctor.DoctorSince.Year,
                    Education = doctor.Education,
                    Languages = new List<string> { "English" },
                    TotalPatients = totalPatients,
                    //NextAvailable = nextAvailable,
                    
                    Relationship = new DoctorRelationshipDTO
                    {
                        AssignedDate = doctorMapping.AssignedDate,
                        Duration = duration,
                        TotalVisits = totalVisits,
                        LastVisit = lastVisit != default ? lastVisit : doctorMapping.AssignedDate,
                        
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting assigned doctor: {patientId}");
                throw;
            }
        }

        public async Task<List<PrescriptionDTO>> GetActivePrescriptionsAsync(string patientId)
        {
            try
            {
                var prescriptions = await _context.Prescriptions
                    .Where(p => p.PatientId == patientId &&
                               //p.Status == "active" && // Removed because 'Status' does not exist on Prescription
                              p.EndDate >= DateTime.Now)
                    .OrderByDescending(p => p.StartDate)
                    .Select(p => new PrescriptionDTO
                    {
                        PrescriptionId = p.PrescriptionId,
                        PatientId = p.PatientId,
                        MedicationName = p.MedicationName,
                        Dosage = p.Dosage,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        PrescribingDoctorId = p.PrescribingDoctorId,
                        Instruction = p.Instructions ?? string.Empty,
                        Status = p.EndDate >= DateTime.Now ? "active" : "inactive"
                    })
                    .ToListAsync();

                return prescriptions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting active prescriptions: {patientId}");
                throw;
            }
        }

        public async Task<List<DailyTaskLogDTO>> GetRecentTaskLogsAsync(string patientId, int limit = 10)
        {
            try
            {
                var taskLogs = await _context.DailyTaskLogs
                    .Include(dtl => dtl.PatientPlan) // Changed from PatientPlanAssignment to PatientPlan
                    .Where(dtl => dtl.PatientPlan.PatientId == patientId &&
                                dtl.Status == "completed")
                    .OrderByDescending(dtl => dtl.TaskDate)
                    .Take(limit)
                    .Select(dtl => new DailyTaskLogDTO
                    {
                        LogId = dtl.LogId,
                        AssignmentId = dtl.AssignmentId,
                        TaskDate = dtl.TaskDate,
                        Status = dtl.Status,
                        CompletedAt = dtl.CompletedAt,
                        PatientNotes = dtl.PatientNotes
                    })
                    .ToListAsync();

                return taskLogs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting recent task logs: {patientId}");
                throw;
            }
        }

        #region Helper Methods

        //private async Task<string> GetNextAvailableSlot(string doctorId)
        //{
        //    try
        //    {
        //        var availability = await _context.DoctorAvailabilities
        //            .Where(da => da.DoctorID == doctorId &&
        //                       da.DayOfWeek == DateTime.Now.DayOfWeek.ToString() &&
        //                       da.IsAvailable)
        //            .FirstOrDefaultAsync();

        //        if (availability != null)
        //        {
        //            return $"Today at {availability.StartTime:hh\\:mm tt}";
        //        }

        //        return "Contact for availability";
        //    }
        //    catch
        //    {
        //        return "Contact for availability";
        //    }
        //}

        private string CalculateDuration(DateTime assignedDate)
        {
            var timeSpan = DateTime.Now - assignedDate;
            var months = (int)(timeSpan.TotalDays / 30);
            var years = months / 12;

            if (years > 0)
            {
                return $"{years} year{(years > 1 ? "s" : "")}";
            }
            else if (months > 0)
            {
                return $"{months} month{(months > 1 ? "s" : "")}";
            }
            else
            {
                var days = (int)timeSpan.TotalDays;
                return $"{days} day{(days > 1 ? "s" : "")}";
            }
        }

//        private static PatientInfoDTO EmptyPatientInfo() => new PatientInfoDTO
//{
//    PatientId = "",
//    FirstName = "",
//    LastName = "",
//    DateOfBirth = DateTime.MinValue,
//    Gender = "",
//    ContactNumber = "",
//    Email = "",
//    Address = "",
//    EmergencyContactName = "",
//    EmergencyContactNumber = "",
//    RegistrationDate = DateTime.MinValue,
//    ProfileImage = ""
//};

//private static AssignedDoctorDTO EmptyAssignedDoctor() => new AssignedDoctorDTO
//{
//    DoctorId = "",
//    FirstName = "",
//    LastName = "",
//    Specialization = "",
//    ContactNumber = "",
//    Email = "",
//    Image = "",
//    Schedule = "",
//    Rating = 0,
//    Experience = 0,
//    Education = "",
//    Languages = new List<string>(),
//    TotalPatients = 0,
//    Relationship = new DoctorRelationshipDTO
//    {
//        AssignedDate = DateTime.MinValue,
//        Duration = "",
//        TotalVisits = 0,
//        LastVisit = DateTime.MinValue
//    }
//};
        #endregion
    }
}
