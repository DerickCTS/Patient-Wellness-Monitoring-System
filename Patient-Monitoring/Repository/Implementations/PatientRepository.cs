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

namespace Patient_Monitoring.Repository.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientMonitoringDbContext _context;
        private readonly ILogger<PatientRepository> _logger;
        
        public PatientRepository(PatientMonitoringDbContext context, ILogger<PatientRepository> logger)
        {
            _context = context;
            _logger = logger;
           
        }


        #region Get Patient By Email
        public async Task<Patient?> GetByEmail(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }

        #endregion


        #region Add New Patient to DB
        public async Task AddPatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                throw new InvalidOperationException($"Patient with ID '{patientId}' not found.");
            return patient;
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<PatientDashboardDTO> GetPatientDashboardDataAsync(string patientId)
        {
            // Example implementation, adjust as needed for your actual dashboard data
            var patient = await _context.Patients
                .Where(p => p.PatientID == patientId)
                .Select(p => new PatientDashboardDTO
                {
                    PatientInfo = new PatientInfoDTO
                    {
                        PatientId = p.PatientID,
                        FirstName = p.FirstName,
                        LastName = p.LastName ?? string.Empty,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        ContactNumber = p.ContactNumber,
                        Email = p.Email,
                        Address = p.Address,
                        EmergencyContactName = p.EmergencyContactName,
                        EmergencyContactNumber = p.EmergencyContactNumber,
                        RegistrationDate = p.RegistrationDate,
                        ProfileImage = p.ProfileImage ?? string.Empty
                    },
                    AssignedDoctor = null!, // Set this properly as needed
                    ActivePrescriptions = new List<PrescriptionDTO>(), // Set this properly as needed
                    RecentTaskLogs = new List<DailyTaskLogDTO>() // Set this properly as needed
                })
                .FirstOrDefaultAsync();

            return patient!;
        }
    }
}
        #endregion

        
