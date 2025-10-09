namespace Patient_Monitoring.DTOs
{
    public class PlanDetailDTO
    {
        public string TaskName { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string AssignedBy { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<PlanDetailItemDTO> Instructions { get; set; } = new();
        public List<PlanDetailItemDTO> Benefits { get; set; } = new();
        public List<PlanDetailItemDTO> Safety { get; set; } = new();
    }

    public class PlanDetailItemDTO
    {
        public string Content { get; set; } = null!;
    }
}
