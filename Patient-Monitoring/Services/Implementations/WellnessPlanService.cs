
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System.Text.Json;



namespace Patient_Monitoring.Services.Implementations
{
    public class WellnessPlanService : IWellnessPlanService
    {
        private readonly IWellnessPlanRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WellnessPlanService(IWellnessPlanRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Retrieving Available Template Cards
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
        #endregion


        #region Retrieving Template Data For Form-Population
        public async Task<TemplateDetailsDto> GetTemplateDetailsForFormAsync(int planId)
        {
            var plan = await _repository.GetTemplatePlanAsync(planId);

            if (plan == null) return null;

            var planDetails = await _repository.GetTemplatePlanDetailsAsync(planId);

            var dto = new TemplateDetailsDto
            {
                PlanId = plan.PlanId,
                PlanName = plan.PlanName,
                Goal = plan.Goal,
                ImageUrl = plan.ImageUrl,
                Details = planDetails.Select(pd => new PlanDetailDto
                {
                    content = pd.Content,
                    detail_type = pd.DetailType,

                }).ToList()
            };
            return dto;
        }
        #endregion


        #region Assigne New Plan - Template and Scratch
        public async Task<string> AssignNewPlanAsync(AssignPlanRequestDto request)
        {
            var assignment = new PatientPlanAssignment
            {
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
                    AssignmentId = assignment.AssignmentId,
                    DetailType = d.detail_type,
                    Content = d.content,
                }).ToList();

                await _repository.AddAssignmentDetailsAsync(newAssignmentPlanDetails);
            }


            return "Plan Assigned Successfully";
        }

        #endregion

        public async Task<string> AssignNewPlanScratchAsync(AssignPlanRequestScratchDto request)
        {
            // 1. Handle the image upload
            string imageUrl = null;
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                // Save the file to your images folder
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "wellness_plans");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/wellness_plans/{uniqueFileName}";
            }

            // 2. Parse the Details JSON string
            var details = JsonSerializer.Deserialize<List<PlanDetailDto>>(request.Details);


            var assignment = new PatientPlanAssignment
            {
                PatientId = request.PatientId,
                AssignedByDoctorId = request.DoctorId,
                FrequencyCount = request.FrequencyCount,
                FrequencyUnit = request.FrequencyUnit,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };


           

            // Insert into 3 tables: WellnessPlan, WellnessPlanDetails, PatientPlanAssignment
            // Inserting into WellnessPlan table
            var newWellnessPlan = new WellnessPlan
                {
                    Category = request.Category,
                    IsTemplate = false,
                    PlanName = request.PlanName,
                    Goal = request.Goal,
                    ImageUrl = imageUrl,
                };

                await _repository.AddWellnessPlanAsync(newWellnessPlan);

                assignment.PlanId = newWellnessPlan.PlanId;
                await _repository.AddPatientPlanAssignment(assignment);

                // Inserting into AssignmentPlanDetails table
                //var newAssignmentPlanDetails = request.Details.Select(d => new AssignmentPlanDetail
                //{
                //    AssignmentId = assignment.AssignmentId,
                //    DetailType = d.detail_type,
                //    Content = d.content,
                //}).ToList();

            // 4. Add plan details
                var planDetails = new List<AssignmentPlanDetail>();
            details.ForEach(details => 
                {
                    planDetails.Add( new AssignmentPlanDetail
                    {
                        AssignmentId = assignment.AssignmentId,
                        DetailType = details.detail_type,
                        Content = details.content
                    });
                });
            

            await _repository.AddAssignmentDetailsAsync(planDetails);
            


            return "Plan Assigned Successfully";
        }
    }
}