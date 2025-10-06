using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    [Table("WellnessPlanDetails")]
    public class WellnessPlanDetail
    {
        [Key]
        [DisplayName("Detail Id")]
        public string DetailId { get; set; } = null!;

        [Required]
        [DisplayName("Plan Id")]
        public string PlanId { get; set; } = null!;

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
        public long DisplayOrder { get; set; }

        public WellnessPlan WellnessPlan { get; set; } = null!;
    }
}