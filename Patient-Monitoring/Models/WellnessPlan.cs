using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{

    public class WellnessPlan
    {
        [Key]
        [DisplayName("Plan Id")]
        public required string PlanId { get; set; }


        [Required]
        [StringLength(50)]
        [DisplayName("Plan Name")]
        public required string PlanName { get; set; }


        [Required]
        [StringLength(35)]
        public required string Goal { get; set; }


        [Required]
        [StringLength(255)]
        [DataType(DataType.Url)]
        public required string ImageUrl { get; set; }


        [Required]
        [StringLength(30)]
        public required string Category { get; set; }


        
        [DisplayName("Created By")]
        public  string? CreatedByDoctorId { get; set; }


      
        [DisplayName("Is Template")]
        public bool IsTemplate { get; set; }


        public ICollection<WellnessPlanDetail>? WellnessPlanDetails { get; set; }
        public ICollection<PatientPlanAssignment>? AssignedPatients { get; set; }
        [ForeignKey("CreatedByDoctorId")]
        public Doctor CreatedByDoctor { get; set; } = null!;
    }
}
