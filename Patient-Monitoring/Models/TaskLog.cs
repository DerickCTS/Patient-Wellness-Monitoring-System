using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class TaskLog
    {
        [Key]
        [DisplayName("Log Id")]
        [Required]
        public required string LogId { get; set; }


        [Required]
        [DisplayName("Assignment Id")]
        public required string AssignmentId { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }


        [Required]
        [StringLength(50)]
        public required string Status { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Completed At")]
        public DateTime? CompletedAt { get; set; }


        [DataType(DataType.MultilineText)]
        [DisplayName("Patient Notes")]
        public string? PatientNotes { get; set; }

        // For quantifiable tasks like "Drink 8 glasses of water"

        //public int? CurrentProgress { get; set; }

        public PatientPlanAssignment PatientPlan { get; set; } = null!;
    }

}
