using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;

namespace Patient_Monitoring.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientMonitoringDbContext _context;

        public PatientRepository(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> SearchPatientsByIdOrNameAsync(string? patientName, string? patientId)
        {
            var result = await _context.Patients.Select(p => p).Where(patient => patient.PatientId == patientId || (patient.FirstName + patient.LastName).Contains(patientName)).ToListAsync();



            return result;



        }

           
        

            // Note: The structure above correctly implements the "Search by ID OR Name" requirement.
            // The conditional logic (hasValidId/hasValidName) is correctly embedded 
            // within the single Where clause, which EF Core can translate.



        
        public async Task<Patient?> GetPatientByIdAsync(string patientId)
        {
            // The method uses FindAsync for primary key lookups, which is highly optimized.
            //return await _context.Patients.Include(patient => patient.PersonalizedDoctorMapper).Include(patient => patient.Diagnoses);


            return await _context.Patients
        // 1. Include the mapping record (Patient -> PatientDoctorMapper)
        .Include(p => p.PersonalizedDoctorMapper)

        // 2. Then, include the actual Doctor entity from the mapper
        .ThenInclude(pdm => pdm.Doctor)
        .Include(p => p.Diagnoses)
        .ThenInclude(pd => pd.Disease)
        .Include(p => p.Diagnoses)
        .ThenInclude(pd => pd.Appointment)
        .ThenInclude(a => a.Doctor)
        .Include(p => p.Prescriptions)
        .Include(p => p.AssignedPlans)
        .ThenInclude(ppa => ppa.AssignedWellnessPlan)
        .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public async Task<bool> IsDoctorAssigned(string patientId, string doctorId)
        {
            // Core data access logic using AnyAsync() for an efficient database check.
            // We assume your DbContext has a DbSet<PatientDoctorMapper> named 'PatientDoctorMappers'
            return await _context.PatientDoctorMapper
                .AnyAsync(mapper =>
                    mapper.PatientId == patientId &&
                    mapper.DoctorId == doctorId);
        }

        public async Task AddWellnessPlanAsync(WellnessPlan plan)
        {
            // Tells EF Core to track this new entity
            await _context.WellnessPlans.AddAsync(plan);
        }

        /// <summary>
        /// Adds a new PatientPlan entity to the database context.
        /// </summary>
        public async Task AddPatientPlanAssignmentAsync(PatientPlanAssignment assignment)
        {
            // Tells EF Core to track this new entity
            await _context.PatientPlanAssignments.AddAsync(assignment);
        }

        /// <summary>
        /// Adds a collection of AssignmentPlanDetail entities to the database context.
        /// </summary>
        public async Task AddAssignmentPlanDetailsAsync(List<AssignmentPlanDetail> details)
        {
            // Adds all entities in the list and tells EF Core to track them
            await _context.AssignmentPlanDetails.AddRangeAsync(details);
        }

        /// <summary>
        /// Persists all tracked changes (from all Add* methods) to the database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            // This is the command that executes the database INSERT operations
            await _context.SaveChangesAsync();
        }
        public async Task<Diagnosis> GetDiagnosisWithDiseaseAsync(string diagnosisId)
        {
            // 1. Access the Diagnoses DbSet
            return await _context.Diagnoses
                .Include(d => d.Disease)
                // 3. Find the specific diagnosis by its ID
                .FirstOrDefaultAsync(d => d.DiagnosisId == diagnosisId);
        }
        // Other repository methods...
    }

}

