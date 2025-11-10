using System;
using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs.Appointment
{
    // DTO for setting weekly recurring availability
    public class DoctorAvailabilityDto
    {
        // Nullable for creation (set by DB), required for updates
        public int? AvailabilityId { get; set; }

        [Required]
        // DoctorId is int (e.g., 1, 2, 3)
        public required int DoctorId { get; set; }

        [Required]
        [StringLength(10)]
        public required string DayOfWeek { get; set; } // e.g., "Monday"

        [Required]
        public required TimeSpan StartTime { get; set; }

        [Required]
        public required TimeSpan EndTime { get; set; }

        // Determines if this is a standing weekly schedule
        public bool IsRecurring { get; set; } = true;
    }
}