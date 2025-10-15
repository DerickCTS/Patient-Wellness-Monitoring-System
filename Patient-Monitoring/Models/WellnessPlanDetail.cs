using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{

    public class WellnessPlanDetail
    {
        [Key]
        [DisplayName("Detail Id")]
        public required string DetailId { get; set; }

        [Required]
        [DisplayName("Plan Id")]
        public required string PlanId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Detail Type")]
        public required string DetailType { get; set; }

        [Required]
        [StringLength(600)]
        [DataType(DataType.Text)]
        public required string Content { get; set; }


        [ForeignKey(nameof(PlanId))]
        public WellnessPlan WellnessPlan { get; set; } = null!;
    }
}
