namespace Patient_Monitoring.DTOs
{
    public class ManualSlotCreationDto
    {
   
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string DoctorID { get; set; }
    // You might add a field here for duration (e.g., 30 minutes)
    }
}