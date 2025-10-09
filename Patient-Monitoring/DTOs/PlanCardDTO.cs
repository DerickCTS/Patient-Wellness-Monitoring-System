namespace Patient_Monitoring.DTOs
{
    public class PlanCardDTO
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Status { get; set; } = null!; // "Pending", "Completed"
        public bool IsActive { get; set; }
    }
}
