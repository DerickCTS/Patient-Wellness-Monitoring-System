using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Patient_Monitoring.Models
{
    public class DailyTaskLog
    {
        [Key]
        [DisplayName("Log Id")]
        public string LogId { get; set; } = null!;


        [Required]
        [DisplayName("Assignment Id")]
        public string AssignmentId { get; set; } = null!;


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Task Date")]
        public DateTime TaskDate { get; set; }


        [Required]
        [StringLength(50)]
        public string Status { get; set; } = null!;


        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Completed At")]
        public DateTime CompletedAt { get; set; }


        [DataType(DataType.MultilineText)]
        [DisplayName("Patient Notes")]
        public string? PatientNotes { get; set; }

        // For quantifiable tasks like "Drink 8 glasses of water"

        //public int? CurrentProgress { get; set; }

        public PatientPlanAssignment PatientPlan { get; set; } = null!;
    }

}
