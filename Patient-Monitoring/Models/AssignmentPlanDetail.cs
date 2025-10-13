using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    public class AssignmentPlanDetail
    {
        [Key]
        [Required]
        [DisplayName("Custom Detail Id")]
        public required string CustomDetailId { get; set; }

        [Required]
        [DisplayName("Assignment Id")]
        public required string AssignmentId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Detail Type")]
        public required string DetailType { get; set; }

        [Required]
        [StringLength(600)]
        [DataType(DataType.Text)]
        public required string Content { get; set; }


        [ForeignKey(nameof(AssignmentId))]
        public PatientPlanAssignment PatientPlan { get; set; } = null!;
    }
}