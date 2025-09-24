using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Custom_Wellness_Plans
    {
        [Display(Name = "Plan ID")]
        [Required] public required string PlanID { get; set; } // Primary Key
        [Display(Name = "Patient ID")]
        [Required] public required string PatientID { get; set; }// Foreign Key to Patient_Details
        [Display(Name = "Plan Name")]
        [Required] public required string PlanName { get; set; }
        [Display(Name = "Recommended Duration (Days)")]

        [Required] public required int Recommended_Duration { get; set; }

        [Display(Name = "Frequency Count")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Frequency count must be a number.")]

        [Required] public required string Frequency_Count { get; set; }
        [Display(Name = "Frequency Unit")]
        [Required] public required string Frequency_Unit { get; set; } // Foreign Key to Patient_Details
        public string? Description { get; set; }

    }
}
