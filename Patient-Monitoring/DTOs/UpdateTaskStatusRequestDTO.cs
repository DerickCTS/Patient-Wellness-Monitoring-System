namespace Patient_Monitoring.DTOs
{
    public class UpdateTaskStatusRequestDTO
    {   
        // "Completed" or "Pending"
        public string NewStatus { get; set; } = null!;
    }
}
