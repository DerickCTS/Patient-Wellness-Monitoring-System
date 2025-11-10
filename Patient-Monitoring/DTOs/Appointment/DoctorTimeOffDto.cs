using System;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs.Appointment
{
    // DTO for setting specific time off (vacation, block time)
    public class DoctorTimeOffDto
    {
        // Nullable for creation (set by DB), required for updates
        public int? TimeOffId { get; set; }

        [Required]
        // DoctorId is int (e.g., 1, 2, 3)
        public required int DoctorId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [StringLength(255)]
        public string? Reason { get; set; }
    }
}