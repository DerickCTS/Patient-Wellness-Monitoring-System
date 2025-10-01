using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interface;

namespace Patient_Monitoring.Repository.Implementation
{
    /// <summary>
    /// Provides concrete implementation for data access using Entity Framework Core.
    /// This repository fulfills the contract defined by IWellnessPlanRepository.
    /// </summary>
    public class WellnessPlanRepository : IWellnessPlanRepository
    {
        private readonly PatientMonitoringDbContext _context;

        /// <summary>
        /// Initializes a new instance of the WellnessPlanRepository.
        /// </summary>
        /// <param name="context">The Entity Framework Core DbContext.</param>
        public WellnessPlanRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

       
        public async Task<Patient_Detail?> GetPatientByIdAsync(string patientId)
        {
            return await _context.Patient_Details
                .FirstOrDefaultAsync(p => p.PatientID == patientId);
        }

        /// <summary>
        /// Retrieves the primary diagnosis record for a given patient ID.
        /// </summary>
        public async Task<Patient_Diagnosis?> GetPatientDiagnosisAsync(string patientId)
        {
            return await _context.Patient_Diagnoses
                .FirstOrDefaultAsync(pd => pd.PatientID == patientId);
        }

        /// <summary>
        /// Retrieves the disease details by its ID.
        /// </summary>
        public async Task<Disease?> GetDiseaseByIdAsync(string diseaseId)
        {
            return await _context.Diseases
                .FirstOrDefaultAsync(d => d.DiseaseId == diseaseId);
        }

        /// <summary>
        /// Retrieves the specialized doctor details mapped to the patient.
        /// </summary>
        public async Task<Doctor_Detail?> GetSpecializedDoctorByPatientIdAsync(string patientId)
        {
            // 1. Find the mapping record
            var mapper = await _context.Patient_Doctor_Mapper
                .FirstOrDefaultAsync(pdm => pdm.PatientID == patientId);

            if (mapper == null) return null;

            // 2. Use the DoctorID from the mapper to get doctor details
            return await _context.Doctor_Details
                .FirstOrDefaultAsync(d => d.DoctorID == mapper.DoctorID);
        }

        /// <summary>
        /// Retrieves all wellness plan details that are assigned to the specified patient.
        /// </summary>
        public async Task<IEnumerable<Wellness_Plan>> GetAssignedPlansByPatientIdAsync(string patientId)
        {
            // 1. Get the list of Plan IDs assigned to the patient via the mapper table
            var assignedPlanIds = await _context.Patient_Plan_Mapper
                .Where(pp => pp.PatientId == patientId)
                .Select(pp => pp.PlanId)
                .ToListAsync();

            // 2. Retrieve the full plan details for those IDs
            return await _context.Wellness_Plans
                .Where(p => assignedPlanIds.Contains(p.PlanID))
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a plan definition exists in the database.
        /// </summary>
        public async Task<bool> PlanExistsAsync(string planId)
        {
            return await _context.Wellness_Plans.AnyAsync(p => p.PlanID == planId);
        }

        /// <summary>
        /// Checks if a specific plan is already assigned to a specific patient.
        /// </summary>
        public async Task<bool> IsPlanAssignedAsync(string patientId, string planId)
        {
            return await _context.Patient_Plan_Mapper
                .AnyAsync(ppm => ppm.PatientId == patientId && ppm.PlanId == planId);
        }

        // --- Write Methods ---

        /// <summary>
        /// Adds a new wellness plan definition to the database context.
        /// </summary>
        public void AddNewPlanAsync(Wellness_Plan newPlan)
        {
            _context.Wellness_Plans.Add(newPlan);
        }

        /// <summary>
        /// Adds a new patient-plan assignment record to the database context.
        /// </summary>
        public void AddPlanAssignmentAsync(Patient_Plan_Mapper patientPlanMapper)
        {
            _context.Patient_Plan_Mapper.Add(patientPlanMapper);
        }

        // --- Persistence ---

        /// <summary>
        /// Persists all changes tracked by the DbContext to the underlying database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}





