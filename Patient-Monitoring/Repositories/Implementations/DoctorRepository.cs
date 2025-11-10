using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;

namespace Patient_Monitoring.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public DoctorRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        //✅ Confirmed
        public async Task<Doctor?> GetDoctorByEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }


        public async Task<bool> AddNewDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            return await _context.SaveChangesAsync() > 0;
        }

        #region DoctorDashboard
        // Helper method to get the Patient IDs associated with the current doctor using the actual junction table
        

        // -------------------------------------------------------------------
        //                       I M P L E M E N T A T I O N S 
        // -------------------------------------------------------------------

        // --- Profile Data ---
        public async Task<DoctorProfileDto> GetDoctorProfileAsync(int doctorId)
        {
            // Assumes Doctors table has a primary key that matches doctorId
            // Also assumes Doctor model has properties like FirstName, LastName, Specialty, etc.
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null) return null;

            return new DoctorProfileDto
            {
                // Fields mapped directly from the Doctor model
                DoctorId = $"DOC{doctorId:D3}",
                DoctorName = $"Dr. {doctor.FirstName} {doctor.LastName}",
                Specialty = doctor.Specialization,
                Email = doctor.Email,
                ContactNumber = doctor.ContactNumber,
                // Assuming properties like License, Education, and SinceDate exist on the Doctor model

                Joined = doctor.DoctorSince,
                Education = doctor.Education,
                RegisteredSince = doctor.DoctorSince
            };
        }

        // Retrieve total number of patients diagnosed by the doctor
        public async Task<int> GetTotalPatientsCountAsync(int doctorId)
        {
            return await _context.Diagnoses
                .Include(d => d.Appointment)
                .Where(d => d.Appointment.DoctorId == doctorId)
                .Select(d => d.Appointment.PatientId)
                .Distinct()
                .CountAsync();
        }

        // Retrieves total number of active prescriptions prescribed by the doctor
        public async Task<int> GetTotalActivePrescriptionsCountAsync(int doctorId)
        {
            return await _context.Prescriptions.Where(p => p.PrescribingDoctorId == doctorId && p.EndDate > DateTime.Now).CountAsync();   
        }

        // Retrieves total number of wellness plans assigned by the doctor
        public async Task<int> GetTotalWellnessPlansCountAsync(int doctorId)
        {
            return await _context.PatientPlanAssignments.Where(ppa => ppa.AssignedByDoctorId == doctorId).CountAsync();
        }

        // Retrieves total number of today's appointments for the doctor
        public async Task<int> GetTodaysAppointmentsCountAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate.Date == DateTime.Today.Date)
                .CountAsync();
        }

        // Retrieves total number of appointments completed today for the doctor
        public async Task<int> GetAppointmentsCompletedTodayAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.AppointmentDate <= DateTime.Now)
                .CountAsync();
        }

        // Retrieves weekly appointment summary(approved and rejected count) for the doctor
        public async Task<List<Appointment>> GetWeeklyAppointmentSummaryAsync(int doctorId, DateTime startOfWeek)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date >= startOfWeek.Date && a.AppointmentDate.Date < startOfWeek.Date.AddDays(7))
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        //Retrieve Doctor's Weekly Availability
        public async Task<List<DoctorAvailability>> GetWeeklyScheduleAsync(int doctorId)
        {
            return await _context.DoctorAvailabilities
                .Where(da => da.DoctorId == doctorId)
                .ToListAsync();
        }

        //Retrieve Todays Appointment 
        public async Task<List<Appointment>> GetTodaysAppointmentsAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == DateTime.Today.Date && a.Status == "Confirmed")
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Retrieve Recent Prescriptions
        public async Task<List<Prescription>> GetRecentPrescriptionsAsync(int doctorId)
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.MedicationSchedules)
                .Where(d => d.PrescribingDoctorId == doctorId)
                .OrderByDescending(d => d.PrescriptionId)
                .Take(3)
                .ToListAsync();
        }

        // Appointment table has a direct DoctorId (confirmed by schema)


        // --- Patient Breakdown Metrics ---
        // FIX: Using the Patient IDs from the junction table for filtering
        //public async Task<(int Total, int NewThisMonth, int Active, int Inactive, int PreviousTotal)> GetPatientBreakdownMetrics(int doctorId, DateTime startOfCurrentMonth, DateTime startOfPreviousMonth)
        //{
        //    var patientIds = GetDoctorPatientIds(doctorId);
        //    var today = DateTime.Today;

        //    // Total count comes directly from the junction table
        //    int total = await patientIds.CountAsync();

        //    // Filter patients who are linked AND registered this month
        //    int newThisMonth = await _context.Patients
        //        .Where(p => patientIds.Contains(p.PatientId) && p.RegistrationDate >= startOfCurrentMonth)
        //        .CountAsync();

        //    // Find patients linked to this doctor who had appointments in the last 6 months (via DoctorId on Appointment)
        //    var recentPatientIdsWithAppointments = await _context.Appointments
        //        .Where(a => a.DoctorId == doctorId && a.AppointmentDate >= today.AddMonths(-6))
        //        .Select(a => a.PatientId)
        //        .Distinct()
        //        .ToListAsync();

        //    // Active patients are those linked via mapper AND who had a recent appointment with this doctor
        //    int active = recentPatientIdsWithAppointments.Intersect(await patientIds.ToListAsync()).Count();
        //    int inactive = total - active;

        //    // Previous total count based on patient registration date
        //    int previousTotal = await _context.Patients
        //        .Where(p => patientIds.Contains(p.PatientId) && p.RegistrationDate < startOfCurrentMonth && p.RegistrationDate >= startOfPreviousMonth)
        //        .CountAsync();

        //    return (total, newThisMonth, active, inactive, previousTotal);
        //}

        //// --- Prescription Breakdown Metrics ---
        //// FIX: Using the Patient IDs from the junction table to filter prescriptions
        //public async Task<(int TotalActive, int NewThisWeek, int ExpiringSoon, int PreviousPeriodCount)> GetPrescriptionBreakdownMetrics(int doctorId, DateTime startOfCurrentPeriod, DateTime startOfPreviousPeriod)
        //{
        //    var patientIds = GetDoctorPatientIds(doctorId);
        //    var today = DateTime.Today;

        //    var activePrescriptions = _context.Prescriptions
        //        .Where(p => patientIds.Contains(p.PatientId) && p.EndDate >= today);

        //    int totalActive = await activePrescriptions.CountAsync();
        //    int newThisWeek = await activePrescriptions.CountAsync(p => p.StartDate >= today.AddDays(-7));
        //    int expiringSoon = await activePrescriptions.CountAsync(p => p.EndDate <= today.AddDays(30));

        //    int previousPeriodCount = await _context.Prescriptions
        //        .Where(p => patientIds.Contains(p.PatientId) && p.StartDate >= startOfPreviousPeriod && p.StartDate < startOfCurrentPeriod)
        //        .CountAsync();

        //    return (totalActive, newThisWeek, expiringSoon, previousPeriodCount);
        //}

        //// --- Wellness Plan Metrics ---
        //// FIX: Using the Patient IDs from the junction table to filter assignments
        //public async Task<(int Total, int Active, int Pending, int Completed, int PreviousActive)> GetWellnessPlanMetrics(int doctorId, DateTime startOfPreviousMonth)
        //{
        //    var patientIds = GetDoctorPatientIds(doctorId);

        //    var assignments = _context.PatientPlanAssignments
        //        .Where(ppa => patientIds.Contains(ppa.PatientId));

        //    int total = await assignments.CountAsync();
        //    var today = DateTime.Today;

        //    int active = await assignments.CountAsync(ppa => ppa.IsActive == true && ppa.EndDate >= today);
        //    int pending = await assignments.CountAsync(ppa => ppa.StartDate > today);
        //    int completed = await assignments.CountAsync(ppa => ppa.EndDate < today);

        //    // Placeholder for previous period active count for % change
        //    int previousActive = await _context.PatientPlanAssignments
        //        .CountAsync(ppa => patientIds.Contains(ppa.PatientId) && ppa.IsActive == true && ppa.StartDate < today && ppa.StartDate >= startOfPreviousMonth);

        //    return (total, active, pending, completed, previousActive);
        //}

        // --- Weekly Appointments Summary ---
        // Appointment table has a direct DoctorId (confirmed by schema)
        //public async Task<Dictionary<DayOfWeek, Tuple<int, int>>> GetWeeklyAppointmentSummaryAsync(int doctorId, DateTime startOfWeek)
        //{
        //    var endOfWeek = startOfWeek.AddDays(7).Date;

        //    // 1. Filter and load relevant data into memory using .ToListAsync()
        //    var appointments = await _context.Appointments
        //        .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date >= startOfWeek.Date && a.AppointmentDate.Date < endOfWeek)
        //        .ToListAsync(); // <-- This forces client-side evaluation

        //    // 2. Perform the grouping and aggregation in C# memory
        //    return appointments
        //        .GroupBy(a => a.AppointmentDate.DayOfWeek)
        //        .Select(g => new
        //        {
        //            Day = g.Key,
        //            Completed = g.Count(a => a.Status == "Completed"),
        //            Cancelled = g.Count(a => a.Status == "Cancelled")
        //        })
        //        .ToDictionary(x => x.Day, x => new Tuple<int, int>(x.Completed, x.Cancelled));
        //}


        

        

        //public async Task<DailyAppointmentSummaryDto> GetAppointmentsCompletedTodayAsync(int doctorId, DateTime date)
        //{
        //    // Define the start and end of the target day (midnight to midnight)
        //    DateTime startOfDay = date.Date;
        //    DateTime endOfDay = date.Date.AddDays(1);

        //    // Fetch all appointments for the current doctor today
        //    var appointments = await _context.Appointments
        //        .AsNoTracking()
        //        .Where(a => a.DoctorId == doctorId &&
        //                    a.AppointmentDate >= startOfDay &&
        //                    a.AppointmentDate < endOfDay)
        //        .ToListAsync();

        //    // Calculate metrics
        //    int completed = appointments.Count(a => a.Status == "Completed");
        //    int remaining = appointments.Count(a => a.Status == "Scheduled" || a.Status == "Pending"); // Assuming "Scheduled" or "Pending" are remaining
        //    int total = appointments.Count;

        //    return new DailyAppointmentSummaryDto
        //    {
        //        Completed = completed,
        //        Remaining = remaining,
        //        Total = total
        //    };
        //}
        #endregion

    }
}
