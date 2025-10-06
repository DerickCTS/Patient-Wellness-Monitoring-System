using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{

    [Table("AssignmentPlanDetails")]
    public class AssignmentPlanDetail
    {
        [Key]
        [Required]
        [DisplayName("Custom Detail Id")]
        public string CustomDetailId { get; set; } = null!;

        [Required]
        [DisplayName("Assignment Id")]
        public string AssignmentId { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [DisplayName("Detail Type")]
        public string DetailType { get; set; } = null!;

        [Required]
        [StringLength(600)]
        [DataType(DataType.Text)]
        public string Content { get; set; } = null!;

        [Required]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

        public PatientPlanAssignment PatientPlan { get; set; } = null!;
    }
}