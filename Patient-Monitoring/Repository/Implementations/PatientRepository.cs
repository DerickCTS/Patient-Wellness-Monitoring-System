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
        // Fix for CS1061: Remove .ThenInclude(d => d.User) since Doctor does not have a 'User' property.
        // Update the following methods:

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            // Assuming PatientID is a string representation of a Guid
            string patientId = id.ToString();
            var patient = await _context.Patients
                .Include(p => p.PersonalizedDoctorMapper)
                    .ThenInclude(m => m.Doctor)
                .FirstOrDefaultAsync(p => p.PatientID == patientId);

            if (patient == null)
                throw new InvalidOperationException($"Patient with ID '{patientId}' not found.");
            return patient;

        }

        public async Task<Patient> GetByPatientIdAsync(string patientId)
        {
            var patient = await _context.Patients
                .Include(p => p.PersonalizedDoctorMapper)
                    .ThenInclude(m => m.Doctor)
                .FirstOrDefaultAsync(p => p.PatientID == patientId);
            if (patient == null)
                throw new InvalidOperationException($"Patient with ID '{patientId}' not found.");
            return patient;

        }

        // Replace all instances of 'RegisteredDate' with 'RegistrationDate' in PatientRepository methods

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.PatientID)
                .Include(p => p.PersonalizedDoctorMapper)
                .OrderByDescending(p => p.RegistrationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetUnmappedPatientsAsync()
        {
            // Fix: Patient does not have PersonalizedDoctorId property.
            // Instead, check if PersonalizedDoctorMapper is null.
            return await _context.Patients
                .Include(p => p.PatientID)
                .Where(p => p.PersonalizedDoctorMapper == null)
                .OrderByDescending(p => p.RegistrationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(Guid doctorId)
        {
            string doctorIdStr = doctorId.ToString();
            return await _context.Patients
                .Include(p => p.PersonalizedDoctorMapper)
                .Where(p => p.PersonalizedDoctorMapper != null
                    && p.PersonalizedDoctorMapper.Doctor != null
                    && p.PersonalizedDoctorMapper.DoctorID == doctorIdStr)
                .OrderByDescending(p => p.RegistrationDate)
                .ToListAsync();
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            return await _context.Patients.CountAsync();
        }
        // Fix for CS0029: Cannot implicitly convert type 'string' to 'int'
        // In PatientManagementDTO, PatientId is of type int, but Patient.PatientID is string.
        // Use a conversion, e.g., int.TryParse, or set to 0 if not convertible.

        public async Task<IEnumerable<PatientManagementDTO>> GetAllPatientsWithAssignedDoctorAsync()
        {
            var patients = await _context.Patients
                .GroupJoin(
                    _context.PatientDoctorMapper.Include(m => m.Doctor),
                    p => p.PatientID,
                    m => m.PatientID,
                    (p, mappingGroup) => new PatientManagementDTO
                    {
                        // Fix: Cannot use int.TryParse with 'out var id' in an expression tree.
                        // Instead, parse outside the expression tree.
                        PatientId = ParsePatientId(p.PatientID),
                        FirstName = p.FirstName,
                        LastName = p.LastName ?? string.Empty,
                        Email = p.Email,
                        AssignedDoctorName = mappingGroup.Any()
                            ? $"{mappingGroup.First().Doctor!.FirstName} {mappingGroup.First().Doctor.LastName}"
                            : "Unmapped"
                    })
                .OrderBy(dto => dto.LastName)
                .ToListAsync();

            return patients;
        }

        // Helper method to parse PatientID string to int
        private static int ParsePatientId(string patientId)
        {
            int id;
            return int.TryParse(patientId, out id) ? id : 0;
        }

        // Patient Management: Add new patient
        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        // Change the return type of GetPatientsWithoutMappingAsync to match the interface
        public async Task<IEnumerable<object>> GetPatientsWithoutMappingAsync()
        {
            var mappedPatientIds = _context.PatientDoctorMapper.Select(m => m.PatientID);

            // Cast the result to IEnumerable<object> to match the interface signature
            return await _context.Patients
                                 .Where(p => !mappedPatientIds.Contains(p.PatientID))
                                 .OrderBy(p => p.LastName)
                                 .Cast<object>()
                                 .ToListAsync();
        }

        // --- Existing Patient Login/Profile methods would go here ---
        // public Task<Patient?> GetPatientByEmailAsync(string email) { ... }
    }
}
        #endregion

        
