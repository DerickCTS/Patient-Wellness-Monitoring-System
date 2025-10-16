using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Patient_Monitoring.Models
{
    public class AppointmentSlot
    {
        [Key]
        [Required]
        [Display(Name = "Slot ID")]
        public int SlotId { get; set; } // Primary Key


        [Required]
        [Display(Name = "Doctor ID")]
        public required string DoctorId { get; set; } // Foreign Key to Doctor


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

        public Doctor Doctor { get; set; } = null!;

        public Appointment? Appointment { get; set; }
    }
}