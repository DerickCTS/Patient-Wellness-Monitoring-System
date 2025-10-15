
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;



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
                PatientId = request.PatientId,
                AssignedByDoctorId = request.DoctorId,
                FrequencyCount = request.FrequencyCount,
                FrequencyUnit = request.FrequencyUnit,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            // If condition is met, when the doctor assigns a plan using template. If PlanName is null, then it is a template.
            if (string.IsNullOrEmpty(request.PlanName))
            {
                assignment.PlanId = request.PlanId;

                await _repository.AddPatientPlanAssignment(assignment);

                // This if condition is met, if the doctor has modified the template details - i.e Instruction, Benefits, Safety & Description
                // If DetailsModifed == true => Data was modified
                // else DetailsModified == false => Data was not modified

                if (request.DetailsModified)
                {
                    // Doctor has modified the template details. So we add new details into AssignmentPlanDetails table.
                    var assignmentDetails = request.Details.Select(d => new AssignmentPlanDetail
                    {
                        CustomDetailId = Guid.NewGuid().ToString(),
                        AssignmentId = assignment.AssignmentId,
                        DetailType = d.detail_type,
                        Content = d.content,
                    }).ToList();

                    await _repository.AddAssignmentDetailsAsync(assignmentDetails);
                }
            }
            else
            {
                // Insert into 3 tables: WellnessPlan, WellnessPlanDetails, PatientPlanAssignment
                // Inserting into WellnessPlan table
                var newWellnessPlan = new WellnessPlan
                {
                    PlanId = Guid.NewGuid().ToString(),
                    Category = request.Category,
                    IsTemplate = false,
                    PlanName = request.PlanName,
                    Goal = request.Goal,
                    ImageUrl = request.ImageUrl,
                };

                await _repository.AddWellnessPlanAsync(newWellnessPlan);

                assignment.PlanId = newWellnessPlan.PlanId;
                await _repository.AddPatientPlanAssignment(assignment);

                // Inserting into AssignmentPlanDetails table
                var newAssignmentPlanDetails = request.Details.Select(d => new AssignmentPlanDetail
                {
                    CustomDetailId = Guid.NewGuid().ToString(),
                    AssignmentId = assignment.AssignmentId,
                    DetailType = d.detail_type,
                    Content = d.content,
                }).ToList();

                await _repository.AddAssignmentDetailsAsync(newAssignmentPlanDetails);
            }


            return "Plan Assigned Successfully";
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