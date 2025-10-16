using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Patient_Monitoring.Models
{
    public class AppointmentSlot
    {
        [Key]
        [Required]
        public int SlotId { get; set; } // Primary Key

        [Required]
        [StringLength(450)]
        public required string DoctorID { get; set; } // Foreign Key to Doctor

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Slot Start Time")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Slot End Time")]
        public DateTime EndDateTime { get; set; }

        [Required]
        [Display(Name = "Is Booked")]
        public bool IsBooked { get; set; } = false; // Default to available

        // Navigation property for Foreign Key relationship
        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }

        // Navigation property for 1-to-0/1 relationship (an Appointment may use this slot)
        public Appointment? Appointment { get; set; }
    }
}