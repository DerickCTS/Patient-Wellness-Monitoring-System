using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    [Table("WellnessPlans")]
    public class WellnessPlan
    {
        [Key]
        [DisplayName("Plan Id")]
        public string PlanId { get; set; } = null!;


        [Required]
        [StringLength(20)]
        [DisplayName("Plan Name")]
        public string PlanName { get; set; } = null!;


        [Required]
        [StringLength(35)]
        public string Goal { get; set; } = null!;


        [Required]
        [StringLength(255)]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; } = null!;


        [Required]
        [DisplayName("Created By")]
        public string CreatedByDoctorId { get; set; } = null!;
        

        public  ICollection<WellnessPlanDetail>? WellnessPlanDetails { get; set; }
        public  ICollection<PatientPlanAssignment>? AssignedPatients { get; set; }

        public Doctor CreatedByDoctor { get; set; } = null!;
    }
}