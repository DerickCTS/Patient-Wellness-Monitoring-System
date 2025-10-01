using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Models
{
    public class Patient_Plan_Mapper
    {
        // Removed: internal readonly object? Plan;

        [Required(ErrorMessage = "Assignment ID is required.")]
        [StringLength(10, ErrorMessage = "Assignment ID cannot exceed 10 characters.")]
        [Display(Name = "Assignment ID")]
        [Key]
        public required string AssignmentID { get; set; } // Primary Key

        [Required(ErrorMessage = "Patient ID is required.")]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        public required string PatientId { get; set; } // Foreign Key to Patient_Details

        [Required(ErrorMessage = "Plan ID is required.")]
        [StringLength(10, ErrorMessage = "Plan ID cannot exceed 10 characters.")]
        [Display(Name = "Plan ID")]
        public required string PlanId { get; set; } // Foreign Key to Wellness_Plan

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        [Display(Name = "Status")]
        public required string Status { get; set; }

        // --- Next Steps: Add Navigation Properties (Optional but Recommended) ---
        // public Patient_Details? Patient { get; set; }
        // public Wellness_Plan? Plan { get; set; }
    }
}
