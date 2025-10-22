using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs.Appointment
{
    public class ManualSlotCreationDto
    {
   
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DoctorId { get; set; }
    // You might add a field here for duration (e.g., 30 minutes)
    }
}