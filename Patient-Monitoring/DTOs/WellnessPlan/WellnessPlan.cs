// In a 'DTOs' folder
namespace Patient_Monitoring.DTOs.WellnessPlan
{
    public class PlanDetailDto
    {
        public string detail_type { get; set; } // Instructions, Safety, Benefits, etc.
        public string content { get; set; }

    }

    public class AssignPlanRequestDto
    {
        public IFormFile? ImageFile { get; set; }
        public int? PlanId { get; set; } 
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string? PlanName { get; set; } // Only for Create from Scratch
        public string? Goal { get; set; } // Only for Create from Scratch
        public string Category { get; set; }
        public string? ImageUrl { get; set; } // Only for Create from Scratch

        // Assignment Specifics (From the form - required for both)
        public int FrequencyCount { get; set; }
        public string FrequencyUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Details (Instructions, Safety, Benefits)
        // For 'Use Template': This contains the template data *if* it was modified.
        // For 'Create from Scratch': This contains the new plan details.
        public List<PlanDetailDto> Details { get; set; } = new List<PlanDetailDto>();

        // Special field for template flow
        // Indicates if the Details list represents modified template data
        public bool DetailsModified { get; set; }
    }

    public class AssignPlanRequestScratchDto
    {
        public IFormFile ImageFile { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Category { get; set; }
        public string PlanName { get; set; }
        public string Goal { get; set; }
        public int FrequencyCount { get; set; }
        public string FrequencyUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Details { get; set; } // JSON string

    }

    public class TemplateDetailsDto : AssignPlanRequestDto
    {

    }
}

