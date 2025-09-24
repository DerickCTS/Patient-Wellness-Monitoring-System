using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class General_Wellness_Plans
    {
        [Required] public required string PlanID { get; set; } // Primary Key
        [Required] public required string PlanName { get; set; }
        [Required] public required int Recommended_Duration { get; set; }
        [Required] public required string Frequency_Count { get; set; }
        [Required] public required string Frequency_Unit { get; set; } // Foreign Key to Patient_Details
        public string? Description { get; set; }

    }
}
