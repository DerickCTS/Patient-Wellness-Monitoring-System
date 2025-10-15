// In a 'DTOs' folder

public class PlanDetailDto
{
    public string detail_type { get; set; } // Instructions, Safety, Benefits, etc.
    public string content { get; set; }

    //Reminde me to delete this later
    public int display_order { get; set; }
    public string PlanID { get; internal set; }
}

public class AssignPlanRequestDto
{
   
    public string PlanId { get; set; } 
   // public string PatientId { get; set; }
    public string PlanName { get; set; } // Only for Create from Scratch
    public string Goal { get; set; } // Only for Create from Scratch
    public string ImageUrl { get; set; } // Only for Create from Scratch

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
    public string PatientId { get; internal set; }
    public string DoctorId { get; internal set; }
}



public class TemplateDetailsDto : AssignPlanRequestDto
{
    
}