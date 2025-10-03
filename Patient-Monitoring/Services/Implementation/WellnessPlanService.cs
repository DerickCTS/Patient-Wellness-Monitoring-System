using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interface;
using Patient_Monitoring.Repository.Interface;

namespace Patient_Monitoring.Services.Implementation
{
    public class WellnessPlanService : IWellnessPlanService
    {
        private readonly IWellnessPlanRepository _wellnessPlanRepository;
        private readonly IPatientRepository _patientRepository;
        public WellnessPlanService(IWellnessPlanRepository wellnessPlanRepository, IPatientRepository patientRepository)
        {
            _wellnessPlanRepository = wellnessPlanRepository;
            _patientRepository = patientRepository;
        }

        public async Task<PatientWellnessDTO?> GetPatientDetailsAsync(string patientId, string patientName)
        {
       
            var patient = await _patientRepository.GetPatientDataByIdOrNameAsync(patientId, patientName);

            if (patient == null)
            {
                return null; 
            }

            var plans = await _wellnessPlanRepository.GetAssignedPlansByPatientIdAsync(patient.PatientID);

            var patientPlans = plans.Select(p => new PatientPlanDTO 
            {
                PlanName = p.PlanName,
                Description = p.Description,
                Frequency_Count = p.Frequency_Count,
                Frequency_Unit = p.Frequency_Unit,
                is_custom = p.is_custom,
                Recommended_Duration = p.Recommended_Duration
            }).ToList();

            var diagnoses = patient.PatientDiagnoses.Select(diagnosis => new DiagnosisDTO
            {
                Disease = diagnosis.Disease.DiseaseName,
                Description = diagnosis.Description
            }).ToList();

            return new PatientWellnessDTO 
            {
                PatientID = patient.PatientID,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Diagnoses = diagnoses,
                AssignedPlans = patientPlans
            };
        }
       
        //private async Task<IEnumerable<PatientPlanDTO>> GetPlansForPatient(string patientId)
        //{
        //    var plans = await _repository.GetAssignedPlansByPatientIdAsync(patientId);
        //    return plans.Select(p => new PatientPlanDTO 
        //    {
        //        PlanID = p.PlanID,
        //        PlanName = p.PlanName,
        //        Description = p.Description,
        //        IsCustom = p.is_custom
        //    }).ToList();
        //}

        //public async Task<(PatientPlanDTO?, string?)> AssignPlanAsync(string patientId, Wellness_Plan newPlan)
        //{
        //    // 1. Check if patient exists
        //    var patientExists = await _repository.GetPatientByIdOrNameAsync(patientId, string.Empty);
        //    if (patientExists == null)
        //    {
        //        return (null, "Patient not found.");
        //    }

        //    // 2. Check if plan is already assigned (Business Rule)
        //    var isAssigned = await _repository.IsPlanAssignedAsync(patientId, newPlan.PlanID);
        //    if (isAssigned)
        //    {
        //        return (null, $"Plan {newPlan.PlanID} is already assigned to patient {patientId}.");
        //    }

        //    // 3. Add plan definition if it doesn't exist
        //    var planExists = await _repository.PlanExistsAsync(newPlan.PlanID);
        //    if (!planExists)
        //    {
        //        _repository.AddNewPlanAsync(newPlan);
        //    }

        //    // 4. Create and add the mapping object
        //    var patientPlanMapper = new Patient_Plan_Mapper
        //    {
        //        AssignmentID = Guid.NewGuid().ToString(),
        //        PatientId = patientId,
        //        PlanId = newPlan.PlanID,
        //        Status = "Assigned" // Default status
        //    };
        //    _repository.AddPlanAssignmentAsync(patientPlanMapper);

        //    // 5. Commit changes
        //    await _repository.SaveChangesAsync();

        //    // 6. Return the mapped PlanAssignment DTO
        //    var assignedPlanDto = new PatientPlanDTO // Mapping the result back
        //    {
        //        PlanID = newPlan.PlanID,
        //        PlanName = newPlan.PlanName,
        //        Description = newPlan.Description,
        //        IsCustom = newPlan.is_custom
        //    };

        //    return (assignedPlanDto, null); // Return DTO and null error message
        //}
    }

}

