using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class DoctorTimeOff
    {
        [Key]
        [Required]
        [Display(Name = "Time Off ID")]
        public int TimeOffId { get; set; } // Primary Key


        [Required]
        [Display(Name = "Doctor ID")]
        public required int DoctorId { get; set; } // Foreign Key to Doctor


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


        public Doctor? Doctor { get; set; }
    }
}