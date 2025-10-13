//using Patient_Monitoring.DTOs;
//using Patient_Monitoring.Repository.Interfaces;
//using Patient_Monitoring.Services.Interfaces;

//namespace Patient_Monitoring.Services.Implementations
//{
//    public class ProgressService : IProgressService
//    {
//        private readonly IProgressRepository _repository;

//        public ProgressService(IProgressRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<IEnumerable<PlanCardDTO>> GetAssignedPlansAsync(string patientId, string timeframe)
//        {
//            // Simple logic for timeframe filter
//            var today = DateTime.Today;
//            var startDate = timeframe.ToLower() == "this week" ? today.AddDays(-(int)today.DayOfWeek) : today;
//            var endDate = startDate.AddDays(6);

//            var wellnessTasks = await _repository.GetWellnessTasksForPatientAsync(patientId, startDate, endDate);

//            return wellnessTasks.Select(log => new PlanCardDTO
//            {
//                LogId = log.LogId,
//                PlanName = log.PatientPlanAssignment.WellnessPlan.PlanName,
//                ImageUrl = log.PatientPlanAssignment.WellnessPlan.ImageURL,
//                Status = log.Status,
//                IsActive = log.PatientPlanAssignment.IsActive
//            });
//        }

//        public async Task<PlanDetailDTO?> GetPlanDetailsAsync(int logId)
//        {
//            var log = await _repository.GetTaskLogByIdAsync(logId);
//            if (log == null) return null;

//            var assignment = await _repository.GetAssignmentByIdAsync(log.AssignmentId);
//            if (assignment == null) return null;

//            var dto = new PlanDetailDTO
//            {
//                TaskName = assignment.WellnessPlan.PlanName,
//                Goal = assignment.WellnessPlan.Goal,
//                Description = assignment.OverrideDescription ?? assignment.WellnessPlan.Description,
//                AssignedBy = "Dr. Anand Kumar", // Placeholder - get from joined Doctor table
//                Frequency = $"{assignment.FrequencyCount} times per {assignment.FrequencyUnit}"
//            };

//            // Override Logic: Check for custom details first
//            var customDetails = (await _repository.GetCustomPlanDetailsAsync(assignment.AssignmentId)).ToList();

//            if (customDetails.Any())
//            {
//                dto.Instructions = customDetails.Where(d => d.DetailType == "Instruction").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//                dto.Benefits = customDetails.Where(d => d.DetailType == "Benefit").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//                dto.Safety = customDetails.Where(d => d.DetailType == "Safety").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//            }
//            else // Fall back to template details
//            {
//                var templateDetails = (await _repository.GetTemplatePlanDetailsAsync(assignment.PlanId)).ToList();
//                dto.Instructions = templateDetails.Where(d => d.DetailType == "Instruction").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//                dto.Benefits = templateDetails.Where(d => d.DetailType == "Benefit").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//                dto.Safety = templateDetails.Where(d => d.DetailType == "Safety").Select(d => new PlanDetailItemDTO { Content = d.Content }).ToList();
//            }

//            return dto;
//        }

//        public async Task<bool> UpdateTaskStatusAsync(int logId, string newStatus)
//        {
//            var log = await _repository.GetTaskLogByIdAsync(logId);
//            if (log == null) return false;

//            log.Status = newStatus;
//            log.CompletedAt = newStatus == "Completed" ? DateTime.UtcNow : null;

//            return await _repository.UpdateTaskLogAsync(log);
//        }
//    }
//}
