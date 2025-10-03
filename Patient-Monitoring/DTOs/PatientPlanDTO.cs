using Patient_Monitoring.Models;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class PatientPlanDTO
    {
        [Required(ErrorMessage = "Plan Name is required.")]
        [StringLength(100, ErrorMessage = "Plan Name cannot exceed 100 characters.")]
        [Display(Name = "Plan Name")]
        public required string PlanName { get; set; }

        [Required(ErrorMessage = "Recommended Duration is required.")]
        [Range(1, 365, ErrorMessage = "Recommended Duration must be between 1 and 365 days.")]
        [Display(Name = "Recommended Duration (Days)")]
        public required int Recommended_Duration { get; set; }

        [Required(ErrorMessage = "Frequency Count is required.")]
        [StringLength(50, ErrorMessage = "Frequency Count cannot exceed 50 characters.")]
        [Display(Name = "Frequency Count")]
        public required string Frequency_Count { get; set; }

        [Required(ErrorMessage = "Frequency Unit is required.")]
        [StringLength(50, ErrorMessage = "Frequency Unit cannot exceed 50 characters.")]
        [Display(Name = "Frequency Unit")]
        public required string Frequency_Unit { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Custom plan status is required.")]
        [Display(Name = "Is Custom Plan")]
        public bool is_custom { get; set; }
    }
}
