using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Patient_Monitoring.Repository.Implementations
{
    public class PatientDoctorMappingRepository(PatientMonitoringDbContext context) : IPatientDoctorMappingRepository
    {
        private readonly PatientMonitoringDbContext _context = context;

        public required ICollection<PatientDoctorMapper> PatientDoctorMappings { get; set; }
        // Fix for CS1061: Replace d.AssignedPatient.PatientId with d.AssignedPatient.Patient.PatientID
        // Also, update usages of PatientId to PatientID in LINQ queries and DTO assignments

        public async Task<int> GetUnmappedPatientsCountAsync()
        {
            // Find patient IDs that exist in the Patient table but not in the Mapping table.
            var mappedPatientIds = _context.PatientDoctorMapper.Select(m => m.PatientID);

            return await _context.Patients
                                 .CountAsync(p => !mappedPatientIds.Contains(p.PatientID));
        }

        // Dashboard Overview: Doctor Assignments Table summary
        public async Task<IEnumerable<DoctorAssignmentSummaryDTO>> GetDoctorAssignmentSummariesAsync()
        {
            var summaries = await _context.Doctors
                .Select(d => new DoctorAssignmentSummaryDTO
                {
                    DoctorId = Convert.ToInt32(d.DoctorId), // Use Convert.ToInt32 instead of int.TryParse with out var
                    DoctorName = $"{d.FirstName} {d.LastName}",
                    Specialization = d.Specialization,
                    PatientsAssigned = d.AssignedPatient != null ? 1 : 0,
                    MaxCapacity = 10,
                    AssignedPatients = d.AssignedPatient != null
                        ? new List<PatientSummaryDTO>
                            {
                                new PatientSummaryDTO
                                {
                                    PatientId = Convert.ToInt32(d.AssignedPatient.Patient.PatientID), // Use Convert.ToInt32 here as well
                                    FirstName = d.AssignedPatient.Patient.FirstName,
                                    LastName = d.AssignedPatient.Patient.LastName?? string.Empty
                                }
                            }
                        : new List<PatientSummaryDTO>()
                })
                .OrderBy(d => d.DoctorName)
                .ToListAsync();

            return summaries;
        }

        // Mapping Logic: Check doctor capacity
        public async Task<int> GetPatientCountForDoctorAsync(int doctorId)
        {
            return await _context.PatientDoctorMapper
                                 .CountAsync(m => m.DoctorID == doctorId.ToString());
        }

        // Fix for CS0738 and CS0535: Ensure method signatures match the interface exactly

        public async Task<PatientDoctorMapper?> GetMappingByPatientIdAsync(int patientId)
        {
            return await _context.PatientDoctorMapper
                                 .FirstOrDefaultAsync(m => m.PatientID == patientId.ToString());
        }

        // Mapping Logic: Create new mapping
        public async Task<PatientDoctorMapper?> CreateMappingAsync(PatientDoctorMapper newMapping)
        {
            _context.PatientDoctorMapper.Add(newMapping);
            await _context.SaveChangesAsync();
            return newMapping;
        }

        // Mapping Logic: Delete existing mapping
        public async Task<bool> DeleteMappingByPatientIdAsync(int patientId)
        {
            var existingMapping = await GetMappingByPatientIdAsync(patientId);
            if (existingMapping == null) return false;

            _context.PatientDoctorMapper.Remove(existingMapping);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
