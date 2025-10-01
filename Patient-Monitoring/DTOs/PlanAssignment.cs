namespace Patient_Monitoring.DTOs
{
    public class PlanAssignment
    {
        public string PlanID { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCustom { get; set; }
    }
}
