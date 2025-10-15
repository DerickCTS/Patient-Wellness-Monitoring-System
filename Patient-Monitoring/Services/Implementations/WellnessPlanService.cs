using Patient_Monitoring.Models;
using Patient_Monitoring.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Services.Interface;



namespace Patient_Monitoring.Services.Implementations
{
    public class WellnessPlanService : IWellnessPlanService
    {
        private readonly IWellnessPlanRepository _repository;

        public WellnessPlanService(IWellnessPlanRepository repository)
        {
            _repository = repository;
        }

        // --- Use Template Flow ---

        public async Task<IEnumerable<TemplateCardDto>> GetAvailableTemplateCards()
        {
            var templates = await _repository.GetAllTemplateCards();
            return templates.Select(t => new TemplateCardDto
            {
                PlanId = t.PlanId,
                PlanName = t.PlanName,
                Goal = t.Goal,
                ImageUrl = t.ImageUrl,
            });
        }

        public async Task<TemplateDetailsDto> GetTemplateDetailsForFormAsync(string planId)
        {
            var plan = await _repository.GetTemplatePlanAsync(planId);

            if (plan == null) return null;

            var planDetails = await _repository.GetTemplatePlanDetailsAsync(planId);

            var dto = new TemplateDetailsDto
            {
                PlanId = plan.PlanId,
                PlanName = plan.PlanName,
                // Read-only on the form
                Goal = plan.Goal,         // Read-only on the form
                ImageUrl = plan.ImageUrl, // Read-only on the form
                Details = planDetails.Select(pd => new PlanDetailDto
                {
                    content = pd.Content,
                    detail_type = pd.DetailType,
                    
                }).ToList()
            };

            // Assignment specific fields (Frequency, Dates) are left empty for the doctor to fill
            return dto;
        }

        // --- Assignment Logic (Handles both Use Template and Create from Scratch) ---

        public async Task<string> AssignNewPlanAsync(AssignPlanRequestDto request)
        {
            // 1. Create PatientPlan
            var assignment = new PatientPlanAssignment
            {
                AssignmentId = Guid.NewGuid().ToString(), // Generate a unique ID
              
                FrequencyCount = request.FrequencyCount,
                FrequencyUnit = request.FrequencyUnit,
               // PatientId=request.PatientId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            // a. Handle 'Use Template' flow
            if (!string.IsNullOrEmpty(request.PlanId))
            {
                // Template flow
                assignment.PlanId = request.PlanId;

                if (request.DetailsModified)
                {
                    // Case 1: Template data was modified (DetailsModified == true)
                    // Insert into AssignmentPlanDetails
                    await InsertAssignmentDetails(assignment.AssignmentId, request.Details);
                }
                // Case 2: Template data was NOT modified (DetailsModified == false)
                // The assignment.plan_id links to the template, and no AssignmentPlanDetails are needed.
            }
            // b. Handle 'Create from Scratch' flow
            else
            {
                // Scratch flow - Plan Name, Goal, etc. are not stored in AssignedWellnessPlan table.
                // Insert into AssignmentPlanDetails (all details must be included here)
                // We also need to store the plan name/goal/image somehow, perhaps in the assignment table 
                // or a custom field if the ERD allowed it, but based on the current ERD,
                // we'll primarily use AssignmentPlanDetails for the custom content.
                // NOTE: The ERD doesn't show where PlanName/Goal go for a scratch plan, 
                // so we assume it *must* go into AssignmentPlanDetails as a special type.

                // Add Name/Goal/Image as special 'details' for a scratch plan
                request.Details.Add(new PlanDetailDto { detail_type = "PlanName", content = request.PlanName, display_order = -3 });
                request.Details.Add(new PlanDetailDto { detail_type = "PlanGoal", content = request.Goal, display_order = -2 });
                request.Details.Add(new PlanDetailDto { detail_type = "PlanImage", content = request.ImageUrl, display_order = -1 });

                await InsertAssignmentDetails(assignment.AssignmentId, request.Details);

            }

            // 2. Add the main assignment record
            var result = await _repository.AddAssignmentAsync(assignment);

            return result.AssignmentId;
        }

        private async Task InsertAssignmentDetails(string assignmentId, List<PlanDetailDto> details)
        {
            if (details == null || !details.Any()) return;

            var assignmentDetails = details.Select(d => new AssignmentPlanDetail
            {
                CustomDetailId = Guid.NewGuid().ToString(),
                AssignmentId = assignmentId,
                DetailType = d.detail_type,
                Content = d.content,
               

            }).ToList();

            await _repository.AddAssignmentDetailsAsync(assignmentDetails);
        }
    }
}