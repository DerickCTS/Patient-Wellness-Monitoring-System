using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class General_Wellness_Plans
    {
        [Display(Name = "Plan ID")]
        [Required] public required string PlanID { get; set; } // Primary Key

        [StringLength(100, ErrorMessage = "Plan name cannot exceed 100 characters.")]
        [Display(Name = "Plan Name")]

        [Required] public required string PlanName { get; set; }

        [Range(1, 365, ErrorMessage = "Recommended duration must be between 1 and 365 days.")]
        [Display(Name = "Recommended Duration (Days)")]

        [Required] public required int Recommended_Duration { get; set; }

        [Display(Name = "Frequency Count")]
        [Range(1, 100, ErrorMessage = "Frequency count must be a positive number.")]

        [Required] public required string Frequency_Count { get; set; }
        [Display(Name = "Frequency Unit")]
        [Required] public required string Frequency_Unit { get; set; } // Foreign Key to Patient_Details

        public string? Description { get; set; }

    }
}
