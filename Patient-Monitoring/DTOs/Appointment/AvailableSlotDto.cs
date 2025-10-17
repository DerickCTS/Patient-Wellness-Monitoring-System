using System;
namespace Patient_Monitoring.DTOs.Appointment
{
    public class AvailableSlotDto
    {
        public int SlotID { get; set; }
        public DateTime StartDateTime { get; set; }
        // Helper properties for UI display
        public string DateString => StartDateTime.ToString("MMM dd, yyyy");
        public string TimeString => StartDateTime.ToString("h:mm tt");
    }
}