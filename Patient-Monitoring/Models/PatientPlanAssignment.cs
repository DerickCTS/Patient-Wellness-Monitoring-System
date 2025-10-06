using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{

    [Table("PatientPlanAssignments")]
    public class PatientPlanAssignment
    {
        [Key]
        [DisplayName("Assignment Id")]
        public string AssignmentId { get; set; } = null!;

        [Required]
        [DisplayName("Patient Id")]
        public string PatientId { get; set; } = null!;

        [Required]
        [DisplayName("Plan Id")]
        public string PlanId { get; set; } = null!;

        [Required]
        [DisplayName("Assigned By")]
        public string AssignedByDoctorId { get; set; } = null!;

        [Required]
        [Range(1, 100)]
        [DisplayName("Frequency Count")]
        public int FrequencyCount { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Frequency Unit")]
        public string FrequencyUnit { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Patient Id")]
        public bool IsActive { get; set; }

        [StringLength(35)]
        public string? OverrideGoal { get; set; }

        // Navigation properties
        public WellnessPlan AssignedWellnessPlan { get; set; } = null!;
        public  ICollection<AssignmentPlanDetail>? AssignmentPlanDetails { get; set; }
        public ICollection<DailyTaskLog> DailyTaskLogs { get; set; } = null!;
        public Doctor AssigningDoctor { get; set; } = null!;
        public Patient AssignedPatient { get; set; } = null!;
    }
}