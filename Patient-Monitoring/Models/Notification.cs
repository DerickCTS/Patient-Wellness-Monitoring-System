using Patient_Monitoring.Enums;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Notification
    {
        [Key]
        [Display(Name = "Notification ID")]
        [Required]
        public required string NotificationId { get; set; } 


        [Required]
        [StringLength(20, ErrorMessage = "Notification Titile cannot exceed more than 20 characters")]
        public required string Title { get; set; }


        [Required]
        [StringLength(150, ErrorMessage = "Notification Message cannot exceed more than 150 characters")]
        public required string Message { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Scheduled At")]
        public DateTime ScheduledAt { get; set; }


        [Required]
        public bool IsRead { get; set; } = false;


        [Required]
        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }


        [Required]
        [Display(Name = "Patient ID")]
        public required string PatientId { get; set; } 

        public Patient Patient { get; set; } = null!;
    }
}
