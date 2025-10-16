using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Patient_Monitoring.Models
{
    public class DoctorTimeOff
    {
        [Key]
        [Required]
        public int TimeOffID { get; set; } // Primary Key

        [Required]
        [StringLength(450)]
        public required string DoctorID { get; set; } // Foreign Key to Doctor

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date & Time")]
        public DateTime EndDateTime { get; set; }

        [StringLength(255)]
        public string? Reason { get; set; }

        // Navigation property for Foreign Key relationship
        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }
    }
}