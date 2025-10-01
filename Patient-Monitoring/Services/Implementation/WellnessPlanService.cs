using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Services.Interface;
using Patient_Monitoring.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patient_Monitoring.Services.Implementation;

/// <summary>
/// Implementation of the service layer, handling the flow of business logic 
/// and mapping entities from the repository to DTOs for the controller.
/// </summary>
public class WellnessPlanService : IWellnessPlanService
{
    private readonly IWellnessPlanRepository _repository;

    /// <summary>
    /// Initializes a new instance of the WellnessPlanService.
    /// </summary>
    /// <param name="repository">The injected data access repository.</param>
    public WellnessPlanService(IWellnessPlanRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves full patient details including diagnosis, doctor, and assigned plans.
    /// </summary>
    public async Task<PatientDetails?> GetPatientDetails(string patientId, string patientName)
    {
        // 1. Find patient (Repository interaction)
        var patient = await _repository.GetPatientByIdOrNameAsync(patientId, patientName);
        if (patient == null)
        {
            return null; // Patient not found
        }

        // 2. Get diagnosis
        var diagnosis = await _repository.GetPatientDiagnosisAsync(patient.PatientID);

        // Handle case where diagnosis record is missing but patient exists
        if (diagnosis == null)
        {
            return new PatientDetails // Using PatientDetails DTO
            {
                PatientID = patient.PatientID,
                PatientFirstName = patient.FirstName,
                PatientLastName = patient.LastName,
                Description = "Diagnosis record missing.", // Default message
                AssignedPlans = await GetPlansForDto(patient.PatientID)
            };
        }

        // 3. Retrieve related details concurrently for efficiency
        var diseaseTask = _repository.GetDiseaseByIdAsync(diagnosis.DiseaseId);
        var doctorTask = _repository.GetSpecializedDoctorByPatientIdAsync(patient.PatientID);
        var plansTask = GetPlansForDto(patient.PatientID);

        await Task.WhenAll(diseaseTask, doctorTask, plansTask);

        var disease = diseaseTask.Result;
        var specializedDoctor = doctorTask.Result;
        var assignedPlans = plansTask.Result;

        // 4. Construct the PatientDetails DTO (Business logic/Mapping)
        string? specializedDoctorName = null;
        if (specializedDoctor != null)
        {
            specializedDoctorName = $"{specializedDoctor.FirstName} {specializedDoctor.LastName}";
        }

        return new PatientDetails // Final DTO construction
        {
            PatientID = patient.PatientID,
            PatientFirstName = patient.FirstName,
            PatientLastName = patient.LastName,
            DiseaseName = disease?.DiseaseName,
            Description = diagnosis.Description,
            SpecializedDoctorName = specializedDoctorName,
            AssignedPlans = assignedPlans
        };
    }

    /// <summary>
    /// Helper to fetch and map entity plans to PlanAssignment DTOs.
    /// </summary>
    private async Task<IEnumerable<PlanAssignment>> GetPlansForDto(string patientId)
    {
        var plans = await _repository.GetAssignedPlansByPatientIdAsync(patientId);
        return plans.Select(p => new PlanAssignment // Mapping to PlanAssignment DTO
        {
            PlanID = p.PlanID,
            PlanName = p.PlanName,
            Description = p.Description,
            IsCustom = p.is_custom
        }).ToList();
    }

    /// <summary>
    /// Assigns a new or existing plan to a patient.
    /// </summary>
    public async Task<(PlanAssignment?, string?)> AssignPlanAsync(string patientId, Wellness_Plan newPlan)
    {
        // 1. Check if patient exists
        var patientExists = await _repository.GetPatientByIdOrNameAsync(patientId, string.Empty);
        if (patientExists == null)
        {
            return (null, "Patient not found.");
        }

        // 2. Check if plan is already assigned (Business Rule)
        var isAssigned = await _repository.IsPlanAssignedAsync(patientId, newPlan.PlanID);
        if (isAssigned)
        {
            return (null, $"Plan {newPlan.PlanID} is already assigned to patient {patientId}.");
        }

        // 3. Add plan definition if it doesn't exist
        var planExists = await _repository.PlanExistsAsync(newPlan.PlanID);
        if (!planExists)
        {
            _repository.AddNewPlanAsync(newPlan);
        }

        // 4. Create and add the mapping object
        var patientPlanMapper = new Patient_Plan_Mapper
        {
            AssignmentID = Guid.NewGuid().ToString(),
            PatientId = patientId,
            PlanId = newPlan.PlanID,
            Status = "Assigned" // Default status
        };
        _repository.AddPlanAssignmentAsync(patientPlanMapper);

        // 5. Commit changes
        await _repository.SaveChangesAsync();

        // 6. Return the mapped PlanAssignment DTO
        var assignedPlanDto = new PlanAssignment // Mapping the result back
        {
            PlanID = newPlan.PlanID,
            PlanName = newPlan.PlanName,
            Description = newPlan.Description,
            IsCustom = newPlan.is_custom
        };

        return (assignedPlanDto, null); // Return DTO and null error message
    }
}
