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

        // Retrieve Doctor Details
        public async Task<Doctor> GetDoctorProfileAsync(int doctorId)
        {
            return await _context.Doctors.FindAsync(doctorId);
        }
        #endregion

    }
}
