using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{

    public class PatientPlanAssignment
    {
        [Key]
        [DisplayName("Assignment Id")]
        public required string AssignmentId { get; set; } 


        [Required]
        [DisplayName("Patient Id")]
        public required string PatientId { get; set; } 


        [Required]
        [DisplayName("Plan Id")]
        public required string PlanId { get; set; } 


        [Required]
        [DisplayName("Assigned By")]
        public required string AssignedByDoctorId { get; set; } 


        [Required]
        [Range(1, 100)]
        [DisplayName("Frequency Count")]
        public int FrequencyCount { get; set; }


        [Required]
        [StringLength(10)]
        [DisplayName("Frequency Unit")]
        public required string FrequencyUnit { get; set; }


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


        // Navigation properties
        public WellnessPlan AssignedWellnessPlan { get; set; } = null!;
        public ICollection<AssignmentPlanDetail>? AssignmentPlanDetails { get; set; }
        public ICollection<DailyTaskLog> DailyTaskLogs { get; set; } = null!;
        public Doctor AssigningDoctor { get; set; } = null!;
        public Patient AssignedPatient { get; set; } = null!;
    }
}