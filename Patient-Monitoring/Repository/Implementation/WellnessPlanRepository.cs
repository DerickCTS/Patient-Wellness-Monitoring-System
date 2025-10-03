using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interface;
namespace Patient_Monitoring.Repository.Implementation
{
    
    public class WellnessPlanRepository : IWellnessPlanRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public WellnessPlanRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wellness_Plan>> GetAssignedPlansByPatientIdAsync(string patientId)
        {      
            var assignedPlanIds = await _context.Patient_Plan_Mapper
                .Where(pp => pp.PatientId == patientId)
                .Select(pp => pp.PlanId)
                .ToListAsync();

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




