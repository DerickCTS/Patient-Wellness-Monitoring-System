
using System.ComponentModel.DataAnnotations;
using Patient_Monitoring.Enums;

namespace Patient_Monitoring.Models
{
    public class Notification
    {
        [Key]
        [Display(Name = "Notification ID")]
        [Required]
        public int NotificationId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Notification Titile cannot exceed more than 100 characters")]
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
        public NotificationType Type { get; set; }
       

        [Required]
        [Display(Name = "Patient Id")]
        public required int PatientId { get; set; }


        public Patient Patient { get; set; } = null!;
    }
}
