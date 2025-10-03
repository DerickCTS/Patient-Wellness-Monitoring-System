using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Patient_Plan_Mapper
    {
        [Required(ErrorMessage = "Assignment ID is required.")]
        [StringLength(10, ErrorMessage = "Assignment ID cannot exceed 10 characters.")]
        [Display(Name = "Assignment ID")]
        [Key]
        public required string AssignmentID { get; set; } // Primary Key

        [Required(ErrorMessage = "Patient ID is required.")]
        [StringLength(10, ErrorMessage = "Patient ID cannot exceed 10 characters.")]
        [Display(Name = "Patient ID")]
        public required string PatientId { get; set; }

        public Patient_Detail Patient { get; set; } = null!;

        [Required(ErrorMessage = "Plan ID is required.")]
        [StringLength(10, ErrorMessage = "Plan ID cannot exceed 10 characters.")]
        [Display(Name = "Plan ID")]
        public required string PlanId { get; set; }

        public Wellness_Plan Plan { get; set; } = null!;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        [Display(Name = "Status")]
        public required string Status { get; set; }
    }
}
