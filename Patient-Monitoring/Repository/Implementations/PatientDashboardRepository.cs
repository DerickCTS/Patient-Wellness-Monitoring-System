using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;

namespace Patient_Monitoring.Repository.Implementations
{
    public class PatientDashboardRepository : IPatientDashboardRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientDashboardRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientID == patientId)
                ?? throw new InvalidOperationException($"Patient with ID '{patientId}' not found.");
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            try
            {
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        

        public async Task<List<Prescription>> GetActivePrescriptionsAsync(string patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.PatientId == patientId &&
                          DateTime.UtcNow < p.EndDate  &&
                          p.EndDate >= DateTime.Now)
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<List<DailyTaskLog>> GetRecentTaskLogsAsync(string patientId, int limit)
        {
            return await _context.DailyTaskLogs
                .Include(dtl => dtl.PatientPlan)
                .Where(dtl => dtl.PatientPlan.PatientId == patientId &&
                            dtl.Status == "completed")
                .OrderByDescending(dtl => dtl.TaskDate)
                .Take(limit)
                .ToListAsync();
        }

        // This is for returning the no.of registered patients under a personalized doctor.
        //public async Task<int> GetDoctorTotalPatientsAsync(string doctorId)
        //{
        //    return await _context.PatientDoctorMapper
        //        .CountAsync(pdm => pdm.DoctorID == doctorId && pdm.IsActive);
        //}

        public async Task<int> GetPatientTotalVisitsAsync(string patientId, string doctorId)
        {
            return await _context.Appointments
                .CountAsync(a => a.PatientId == patientId &&
                               a.DoctorId == doctorId &&
                               a.Status == "completed");
        }

        public async Task<DateTime?> GetPatientLastVisitAsync(string patientId, string doctorId)
        {
            var lastAppointment = await _context.Appointments
                .Where(a => a.PatientId == patientId &&
                          a.DoctorId == doctorId &&
                          a.Status == "completed")
                .OrderByDescending(a => a.AppointmentDate)
                .FirstOrDefaultAsync();

            return lastAppointment?.AppointmentDate;
        }

        //public async Task<DoctorAvailability> GetDoctorNextAvailabilityAsync(string doctorId)
        //{
        //    var today = DateTime.Now.DayOfWeek.ToString();

        //    return await _context.DoctorAvailabilities
        //        .Where(da => da.DoctorID == doctorId &&
        //                   da.DayOfWeek == today &&
        //                   da.IsAvailable)
        //        .FirstOrDefaultAsync();
        //}
    }
}
